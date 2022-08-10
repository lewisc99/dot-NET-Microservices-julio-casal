using Microsoft.AspNetCore.Mvc;
using Play.Catolog.Service.Models;
using Play.Catolog.Service.Models.Dtos;
using Play.Catolog.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Play.Catolog.Service.Models.Dtos.Dtos;

namespace Play.Catolog.Service.Controllers
{
    // https://localhost:5001/items
    [ApiController]
    [Route("items")]
  
    public class ItemsController : ControllerBase
    {


        private readonly IRepository<Item> itemsRepository;


        public ItemsController(IRepository<Item> itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }




        [HttpGet]
        public async Task <IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());

            return items;
        }



        // GET /  items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {

            var item = await itemsRepository.GetAsync(id);


          
            if (item == null)
            {
                return NotFound(); 
            }



            return item.AsDto();


        }

        //post //
        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {

            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);



            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);


        }



        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync (Guid id, UpdateItemDto updateItemDto)
        {


            Item existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await itemsRepository.UpdateAsync(existingItem);


            return NoContent();


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {

            Item Item = await itemsRepository.GetAsync(id);

            if (Item == null)
            {
                return NotFound();
            }


            await itemsRepository.RemoveAsync(Item.Id);

            return NoContent();

        }

    }
}
