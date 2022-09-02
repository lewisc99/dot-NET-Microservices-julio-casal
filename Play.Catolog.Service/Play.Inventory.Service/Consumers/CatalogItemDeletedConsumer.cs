using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Consumers
{


    //have to specify what is the type of message that this consumer is going to handle and that type of message is
    // actually coming from them the contracts that we're bringing in from catalog so in this in this case it's going to be
    //catalog item created
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {



        private readonly IRepository<CatalogItem> repository;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }

        //the message and so what we want to do when we consume the message is actually a storage in our local catalog items
        //database so to do that let's first introduce a repository that we can use for for that
        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {

            var message = context.Message;

            var item = await repository.GetAsync(message.itemId);

            if (item == null) //if null will create a new one
            {

                return;
            }

            await repository.RemoveAsync(message.itemId);



        }
    }
}
