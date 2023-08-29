namespace Mag.Areas.Admin.Models.Dto.Comment
{
    public class CommentEditDto
    {
        public int Id { get; set; }
        public string? CommentText { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
