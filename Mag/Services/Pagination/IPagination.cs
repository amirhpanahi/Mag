namespace Mag.Services.Pagination
{
    public interface IPagination<T>
    {
        public int PageSize();
        public List<T> PaginatedLiset();
    }
}
