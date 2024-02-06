using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrasturcture.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreDbContextR context;

        public ProductRepository(StoreDbContextR context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyList<Brand>> GetBrands()
          => await context.Brands.ToListAsync();

        public async Task<IReadOnlyList<Category>> GetCategories()
          => await context.Categories.ToListAsync();

        public async Task<Product> GetProduct(int? id)
          => await context.Products
                          .Include(p => p.Brand)
                          .Include(p => p.Category)
                          .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IReadOnlyList<Product>> GetProducts()
          => await context.Products
                          .Include(p => p.Brand)
                          .Include(p => p.Category)
                          .ToListAsync();
    }
}
