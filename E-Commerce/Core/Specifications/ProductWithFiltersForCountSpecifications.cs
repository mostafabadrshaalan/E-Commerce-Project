using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecifications : BaseSpecifications<Product>
    {
        public ProductWithFiltersForCountSpecifications(ProductSpecsParams productParams)
            : base(product =>
                  (string.IsNullOrEmpty(productParams.Search) || product.Name.Contains(productParams.Search)) &&
                  (!productParams.BrandId.HasValue || product.BrandId == productParams.BrandId) &&
                  (!productParams.CategoryId.HasValue || product.CategoryId == productParams.CategoryId)
                  )
        {

        }
    }
}
