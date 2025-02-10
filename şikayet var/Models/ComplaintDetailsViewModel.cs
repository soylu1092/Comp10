using Microsoft.AspNetCore.Mvc;

namespace şikayet_var.Models
{
    public class ComplaintDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new();
        public List<string>? ImagePath { get; set; }
    }

    public class CommentViewModel
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}