using Core.Specifications;

namespace API.Helpers
{
    public class MaxPageIndex
    {
        public static int SetMaxPageIndex(ProductSpecsParams productParams, int totalCount)
        {
            var maxPageIndex = (int)Math.Ceiling(totalCount / (double)productParams.PageSize);

            productParams.PageIndex = maxPageIndex < productParams.PageIndex ? maxPageIndex : productParams.PageIndex;

            return productParams.PageIndex;
        }
    }
}
