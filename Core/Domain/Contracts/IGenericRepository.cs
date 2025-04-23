using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<T,TKey> where T : BaseEntity<TKey>
    {

        Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false);

        Task<T?> GetAsync(TKey id);

        Task<IEnumerable<T>> GetAllAsync(ISpecifications<T,TKey> Spec,bool trackChanges = false);

        Task<T?> GetAsync(ISpecifications<T, TKey> Spec, TKey id);
        Task AddAsync(T entity);


        void Update(T entity);


        void Delete(T entity);

    }
}
