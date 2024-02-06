using Core.Entities;
using Core.Entities.OrderAggergate;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Infrastructure.Data
{
    public class StoreDbContextRSeed
    {
        public static async Task SeedAsync(StoreDbContextR context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.Brands != null && !context.Brands.Any())
                {
                    var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<Brand>>(brandsData);

                    foreach (var brand in brands)
                        await context.Brands.AddAsync(brand);

                    await context.SaveChangesAsync();
                }

                if (context.Categories != null && !context.Categories.Any())
                {
                    var categoriesData = File.ReadAllText("../Infrastructure/Data/SeedData/categories.json");
                    var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

                    foreach (var category in categories)
                        await context.Categories.AddAsync(category);

                    await context.SaveChangesAsync();
                }

                if (context.Products != null && !context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    foreach (var product in products)
                        await context.Products.AddAsync(product);

                    await context.SaveChangesAsync();
                }

                if (context.DeliveryMethods != null && !context.DeliveryMethods.Any())
                {
                    var deliveryMethodsData = File.ReadAllText("../Infrasturcture/Data/SeedData/delivery.json");
                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                    foreach (var method in methods)
                        await context.DeliveryMethods.AddAsync(method);

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreDbContextRSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
