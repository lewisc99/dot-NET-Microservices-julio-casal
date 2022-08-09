using Play.Catolog.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play.Catolog.Service.Repositories
{
    public interface IItemsRepository
    {


        public  Task CreateAsync(Item entity);


        public  Task<IReadOnlyCollection<Item>> GetAllAsync();

        public  Task<Item> GetAsync(Guid id);


        public  Task RemoveAsync(Guid id);


        public  Task UpdateAsync(Item entity);
       
    }
}