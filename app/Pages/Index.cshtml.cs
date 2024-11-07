using Lebiru.Announce.Models;
using Lebiru.Announce.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Lebiru.Announce.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AnnouncementService _announcementService;
        private readonly BannerService _bannerService;
        public string HeroTitle { get; private set; } = "Announcement Board";

        public IndexModel(AnnouncementService announcementService, BannerService bannerService)
        {
            _announcementService = announcementService;
            _bannerService = bannerService;
        }

        public List<Announcement> Announcements { get; private set; }
        public string BannerMessage { get; private set; }

        public void OnGet()
        {
            // Fetch announcements sorted by date (latest on top)
            Announcements = _announcementService.GetAll()
                .OrderByDescending(a => a.Date)
                .ToList();

            // Get the banner message, if available
            BannerMessage = _bannerService.GetBanner()?.Message;

            // Check if the environment variable is set and override the title if it is
            HeroTitle = Environment.GetEnvironmentVariable("FRONT_PAGE_HERO_TITLE") ?? HeroTitle;

            ViewData["HeroTitle"] = HeroTitle; // Set default title for About page
            ViewData["BannerMessage"] = BannerMessage;   // Optionally, set default banner message or leave null
        }
    }
}
