namespace GameCommerce.Aplicacao.Helpers
{
    public class PaginationInfo
    {
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 0;
        public int TotalItems { get; set; } = 0;
        public int PageSize { get; set; } = 10;

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}
