using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications(ProductSpecsParams productParams)
            : base(product =>
                  (string.IsNullOrEmpty(productParams.Search) || product.Name.Contains(productParams.Search)) &&
                  (!productParams.BrandId.HasValue || product.BrandId == productParams.BrandId) &&
                  (!productParams.CategoryId.HasValue || product.CategoryId == productParams.CategoryId)
                  )
        {
            AddInclude(product => product.Brand);
            AddInclude(product => product.Category);
            AddPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(product => product.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(product => product.Price);
                        break;
                }
            }
            else
                AddOrderBy(product => product.Name);

        }

        public ProductWithBrandAndCategorySpecifications(int? id) : base(product => product.Id == id)
        {
            AddInclude(product => product.Brand);
            AddInclude(product => product.Category);
        }

    }
}
