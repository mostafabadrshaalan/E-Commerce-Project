namespace Core.Specifications
{
    public class ProductSpecsParams
    {
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public string? Sort { get; set; }
        private int _pageSize = 6;
        private const int MAXPAGESIZE= 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value >= MAXPAGESIZE ? MAXPAGESIZE : value;
        }
        public int PageIndex { get; set; } = 1;

        private string? search;

        public string? Search
        {
            get => search; 
            set => search = value.Trim().ToLower(); 
        }

    }
}