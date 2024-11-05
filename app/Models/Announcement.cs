using System;

namespace Lebiru.Announce.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; } // Markdown content
        public string ImageUrl { get; set; } // Optional image URL
    }
}
