using Mag.Models.Entities;

namespace Mag.Areas.Admin.Models.Dto.Like
{
    public class AddLikeDto
    {
        public string UserId { get; set; }
        public int NewsId { get; set; }
        public StatusLike statusLike { get; set; }
    }
}
