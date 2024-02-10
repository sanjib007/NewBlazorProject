namespace L3T.Infrastructure.Helpers.Extention
{
    public class GeneralPaginationQuery
    {
        public int PageNumber { get; set; } = 1;
        public bool Pagination { get; set; } = true;
        public int PageSize { get; set; } = 100000;
    }
}