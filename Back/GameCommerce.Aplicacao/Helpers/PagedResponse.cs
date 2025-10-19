namespace GameCommerce.Aplicacao.Helpers
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public PaginationInfo Pagination { get; set; } = new PaginationInfo();
    }
}
