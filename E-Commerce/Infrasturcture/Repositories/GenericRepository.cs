using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrasturcture.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContextR context;

        public GenericRepository(StoreDbContextR context)
        {
            this.context = context;
        }
        public async Task<T> GetByIdAsync(int? id)
            => await context.Set<T>().FindAsync(id);
        public void Add(T entity)
            => context.Set<T>().Add(entity);

        public void Update(T entity)
            => context.Set<T>().Update(entity);
        public void Delete(T entity)
            => context.Set<T>().Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await context.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllWithSpecifications(ISpecifications<T> specifications)
            => await ApplingSpecification(specifications).ToListAsync();

        public async Task<T> GetEntityWithSpecitfications(ISpecifications<T> specifications)
            => await ApplingSpecification(specifications).FirstOrDefaultAsync();

        public async Task<int> CountAsync(ISpecifications<T> specifications)
            => await ApplingSpecification(specifications).CountAsync();


        private IQueryable<T> ApplingSpecification(ISpecifications<T> specifications)
            => SpecificationsEvaluator<T>.GetQuery(context.Set<T>(), specifications);

    }
}
