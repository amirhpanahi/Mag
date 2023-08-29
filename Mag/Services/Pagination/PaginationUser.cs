using Mag.Services.Pagination.Dto;

namespace Mag.Services.Pagination
{
    public class PaginationUser : IPagination<UserPaginationDto>
    {
        public List<UserPaginationDto> ListT { get; set; }
        public int PerPage { get; set; }
        public int CurrentPage { get; set; }

        public int PageSize()
        {
            return Convert.ToInt32(Math.Ceiling(ListT.Count / (double)PerPage));
        }

        public List<UserPaginationDto> PaginatedLiset()
        {
            int start = (CurrentPage - 1) * PerPage;
            return ListT.OrderBy(x => x.DateRegister).Skip(start).Take(PerPage).ToList();
        }
    }
}
