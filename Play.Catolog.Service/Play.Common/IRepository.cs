
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Play.Common
{
    public interface IRepository<T> where T : IEntity 
        // where T:IEntity means that has to be a class that implements IEntity interface that has a property Guid Id

    {


        public  Task CreateAsync(T entity);


        public  Task<IReadOnlyCollection<T>> GetAllAsync();
        public  Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T,bool>> filter ); //link expression

        public  Task<T> GetAsync(Guid id);
        public  Task<T> GetAsync(Expression<Func<T, bool>> filter); //return any entity that matches that filter.


        public  Task RemoveAsync(Guid id);


        public  Task UpdateAsync(T entity);
       
    }
}