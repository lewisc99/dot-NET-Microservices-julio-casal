using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Play.Common.MongoDB;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using Polly;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Play.Inventory.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMongo()
                .AddMongoRepository< InventoryItem>("inventoryitems");

            Random jitterer = new Random();


            //to register catalog client
            services.AddHttpClient<CatalogClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44377");

                //add transient Http Error policy handle network failures 
                //handle http 5XX status code (server errros)
                //handle 408 status code (request timeout)

                //we specify the exception that will be coming out of the timeout of the timeout async call over
            }).AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
                    //first parameter is going to be the retry count
                    //how many times we want to retry in the presence of transient error failures
                    5, //how many times to wait to each retry
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),



                    //not necessary this parameter
                    //it's works to if you want to don't want to see what's going on behind the scenes
                    //what we're going to do is to as an instance of the service provider
                    onRetry: (outcome, timespan, retryAttemp) =>
                    {
                        var serviceProvider = services.BuildServiceProvider();
                        serviceProvider.GetService<ILogger<CatalogClient>>()?
                        .LogWarning($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttemp}");


                    }
                ))
            //to add circuit breaker
            .AddTransientHttpErrorPolicy(
                builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
                    //first parameter is 3  means =  going to be three so three failed requests are going to are going to go
                    //through the circuit breaker before the secret breaker actually notices that yeah there's a problem and we have to open the circuit

                    //second parameter is the duration

                    //third paramweter onBreak: means =  functions here to get an insight of what's going onbehind the scenes


                    3,
                    TimeSpan.FromSeconds(15), // it's going to wait 15 seconds uh before allowing any new request toactuallly try to reach the other end

                    // then we have functions to
                    //lock what's happening when the secret opens and when the circuit closes 
                    
                    onBreak: (outcome,timespan) =>
                    {
                        var serviceProvider = services.BuildServiceProvider();
                        serviceProvider.GetService<ILogger<CatalogClient>>()?
                        .LogWarning($"Opening the circuit for {timespan.TotalSeconds} seconds");

                    },
                    onReset: () =>
                    {
                        var serviceProvider = services.BuildServiceProvider();
                        serviceProvider.GetService<ILogger<CatalogClient>>()?
                        .LogWarning($"Closing the circuit...");
                    }

             ))
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(2)); //handle errror
            //you're saying that anytime we invoke anything under locahost 444377 we're going to wait at much
            //one second before giving up.


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Play.Inventory.Service", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Play.Inventory.Service v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
