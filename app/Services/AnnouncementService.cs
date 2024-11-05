using System.Collections.Generic;
using System.Linq;
using Lebiru.Announce.Models;

namespace Lebiru.Announce.Services
{
    public class AnnouncementService
    {
        private readonly List<Announcement> _announcements = new();

        public List<Announcement> GetAll() => _announcements;

        public Announcement Get(int id) => _announcements.FirstOrDefault(a => a.Id == id);

        public void Add(Announcement announcement) => _announcements.Add(announcement);

        public void Update(Announcement updatedAnnouncement)
        {
            var announcement = Get(updatedAnnouncement.Id);
            if (announcement == null) return;

            announcement.Title = updatedAnnouncement.Title;
            announcement.Date = updatedAnnouncement.Date;
            announcement.Content = updatedAnnouncement.Content;
            announcement.ImageUrl = updatedAnnouncement.ImageUrl;
        }

        public void Delete(int id) => _announcements.RemoveAll(a => a.Id == id);
    }
}
