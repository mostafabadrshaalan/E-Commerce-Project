using Core.Entities;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T:BaseEntity
    {
        void Add(T entity);
        void Update (T entity);
        void Delete (T entity);
        Task<T> GetByIdAsync(int? id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllWithSpecifications(ISpecifications<T> specifications);
        Task<T> GetEntityWithSpecitfications(ISpecifications<T> specifications);
        Task<int> CountAsync(ISpecifications<T> specifications);
    }
}
