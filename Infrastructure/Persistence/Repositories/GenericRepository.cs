using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : BaseEntity<TKey>
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false)
        {
            if(typeof(T) == typeof(Product))
            {
                if (trackChanges) return await _context.Products.Include(P=>P.productBrand).Include(P=>P.productType).ToListAsync() as IEnumerable<T>;
                return await _context.Products.Include(P => P.productBrand).Include(P => P.productType).AsNoTracking().ToListAsync() as IEnumerable<T>;
            }
            if (trackChanges) return await _context.Set<T>().ToListAsync();
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }


        public async Task<T?> GetAsync(TKey id)
        {
            if (typeof(T) == typeof(Product))
            {
                return await _context.Products.Include(P => P.productBrand).Include(P => P.productType).FirstOrDefaultAsync(P=>P.Id == id as int?) as T;
            }
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
           _context.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(ISpecifications<T, TKey> Spec, bool trackChanges = false)
        {
           return await ApplySpecifications(Spec).ToListAsync();
        }

        public async Task<T?> GetAsync(ISpecifications<T, TKey> Spec, TKey id)
        {
            return await ApplySpecifications(Spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecifications<T,TKey> spec)
        {
            return SpecificationEvaluator.GetQuery(spec,_context.Set<T>());
        }
    }
}
