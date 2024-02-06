using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(int? id);
        Task<IReadOnlyList<Product>> GetProducts();
        Task<IReadOnlyList<Category>> GetCategories();
        Task<IReadOnlyList<Brand>> GetBrands();
    }
}
