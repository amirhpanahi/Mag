﻿namespace Mag.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int NewsId { get; set; }
        public string? CommentText { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string? RegisterDatePersian { get; set; }
        public int ParentId { get; set; }
        public StatusName? Status { get; set; }

        public enum StatusName
        {
            Publish,
            Delete,
            WaitingForConfirm,
            RejectedByAdmin
        }
    }
}
