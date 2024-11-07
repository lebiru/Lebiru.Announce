using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lebiru.Announce.Models;

namespace Lebiru.Announce.Services
{
    public class AnnouncementService
    {
        private readonly List<Announcement> _announcements = new();
        private const string PostsDirectory = "Posts";

        public AnnouncementService() 
        {
            LoadAnnouncementsFromFiles();
        }

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

        private void LoadAnnouncementsFromFiles()
        {
            if (!Directory.Exists(PostsDirectory))
            {
                Directory.CreateDirectory(PostsDirectory);
            }

            var files = Directory.GetFiles(PostsDirectory, "*.txt"); // Load .txt files only
            foreach (var filePath in files)
            {
                var content = File.ReadAllText(filePath, Encoding.UTF8);
                var title = Path.GetFileNameWithoutExtension(filePath); // Use file name as title

                _announcements.Add(new Announcement
                {
                    Id = _announcements.Count + 1,
                    Title = title,
                    Content = content,
                    Date = File.GetCreationTime(filePath)
                });
            }
        }

        private void StartFileWatcher()
        {
            var watcher = new FileSystemWatcher(PostsDirectory, "*.txt")
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            watcher.Created += OnNewFileAdded;
            watcher.EnableRaisingEvents = true;
        }

        private void OnNewFileAdded(object sender, FileSystemEventArgs e)
        {
            var content = File.ReadAllText(e.FullPath, Encoding.UTF8);
            var title = Path.GetFileNameWithoutExtension(e.FullPath);

            _announcements.Add(new Announcement
            {
                Id = _announcements.Count + 1,
                Title = title,
                Content = content,
                Date = File.GetCreationTime(e.FullPath)
            });
        }

    }
}
