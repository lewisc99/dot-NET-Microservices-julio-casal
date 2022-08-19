using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Play.Inventory.Service.Dtos.DtosItems;

namespace Play.Inventory.Service.Controllers
{



    [ApiController]
    [Route("items")]
    public class ItemsController:ControllerBase
    {
        private readonly IRepository<InventoryItem> itemsRepository;

        private readonly CatalogClient catalogClient;



        public ItemsController(IRepository<InventoryItem> itemsRepository, CatalogClient catalogClient)
        {
            this.itemsRepository = itemsRepository;
            this.catalogClient = catalogClient;
        }


        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }


            var catalogItems = await catalogClient.getCatalogItemsAsync();
            var inventoryItemEntities = await itemsRepository.GetAllAsync(item => item.UserId == userId);

            var inventoryItemsDtos = inventoryItemEntities.Select(
                inventoryItem =>
                {
                    var catalogItem = catalogItems.Single(catalogItem => catalogItem.id == inventoryItem.CatalogItemId); //return a single object

                    return inventoryItem.asDto(catalogItem.Name, catalogItem.Description);
                });




            return Ok(inventoryItemsDtos);
        }



        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
        {

           

            var inventoryItem = await itemsRepository.GetAsync(
                item => item.UserId == grantItemsDto.userId && item.CatalogItemId == grantItemsDto.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    UserId =  Guid.NewGuid(),
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };


                await itemsRepository.CreateAsync(inventoryItem);
                }
            else
            {
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await itemsRepository.UpdateAsync(inventoryItem);
            }

            return Ok();


        }

    }
}
