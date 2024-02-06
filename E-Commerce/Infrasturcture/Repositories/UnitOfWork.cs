using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrasturcture.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContextR context;
        private Hashtable _repositories;

        public UnitOfWork(StoreDbContextR context)
        {
            this.context = context;
        }
        public async Task<int> Complete()
            => await context.SaveChangesAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repoType = typeof(GenericRepository<>);
                var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(TEntity)), context);
                _repositories.Add(type, repoInstance);
            }
            return (IGenericRepository<TEntity>)_repositories[type];
        }
    }
}
