namespace Mag.Models.Entities
{
    public class Like
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string UserId { get; set; }
        public StatusLike? StatusLike { get; set; }
    }
    public enum StatusLike 
    {
        Like,
        None
    }
}
