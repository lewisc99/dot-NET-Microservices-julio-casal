using Play.Catolog.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play.Catolog.Service.Repositories
{
    public interface IRepository<T> where T : IEntity 
        // where T:IEntity means that has to be a class that implements IEntity interface that has a property Guid Id

    {


        public  Task CreateAsync(T entity);


        public  Task<IReadOnlyCollection<T>> GetAllAsync();

        public  Task<T> GetAsync(Guid id);


        public  Task RemoveAsync(Guid id);


        public  Task UpdateAsync(T entity);
       
    }
}