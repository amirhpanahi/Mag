namespace Mag.Services.Pagination.Dto
{
    public class UserPaginationDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PicAddress { get; set; }
        public string? PicAlt { get; set; }
        public string? PicTitle { get; set; }
        public string DateRegisterPresian { get; set; }
        public string LastLoginDatePersian { get; set; }
        public DateTime? DateRegister { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}
