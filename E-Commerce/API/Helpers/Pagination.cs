namespace API.Helpers
{
    public class Pagination<TEntity>
    {
        public Pagination(int pageIndex, int pageSize, int totalCount, IReadOnlyList<TEntity> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IReadOnlyList<TEntity> Data { get; set; }

    }
}
