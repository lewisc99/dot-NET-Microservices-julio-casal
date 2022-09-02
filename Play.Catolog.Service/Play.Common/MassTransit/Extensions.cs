using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Play.Common.MassTransit
{
    public static class Extensions
    {



        //similar configuration as we used in Startup from Catalog.Service

        public static IServiceCollection AddMassTransientWithRabbitMq(this IServiceCollection services)
        {

        
            services.AddMassTransit(configure =>
            {
            //there's a few ways to register consumers and the way we're going to do it is that we're going to specify the assembly that should have all the
            //consumers already defined and that's going to be the uh the entry assembly for whichever microservice is invoking
            //so any consumer classes that are in that assembly will be registered by this method
           // and finally let's go ahead and return our services instance
                configure.AddConsumers(Assembly.GetEntryAssembly());



                configure.UsingRabbitMq((context, configurator) =>
                {

                                    //service provider is context
                    var configuration = context.GetService<IConfiguration>();

                    var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();


                    var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    configurator.Host(rabbitMQSettings.Host); 

                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));

                });
            });

            services.AddMassTransitHostedService();

            return services; //return service instances.
        }
    }
}
