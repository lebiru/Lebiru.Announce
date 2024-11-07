using Lebiru.Announce.Models;
using Lebiru.Announce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Lebiru.Announce.Pages
{
    public class SearchModel : PageModel
    {
        private readonly AnnouncementService _announcementService;

        public SearchModel(AnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [BindProperty]
        public string Query { get; set; }

        public List<Announcement> Results { get; private set; } = new();

        public void OnGet()
        {
            Results = new List<Announcement>();
        }

        public IActionResult OnGetSearch(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                Results = _announcementService.GetAll()
                    .Where(a =>
                        a.Title.Contains(query, System.StringComparison.OrdinalIgnoreCase) ||
                        a.Content.Contains(query, System.StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(a => a.Date)
                    .ToList();
            }

            return Partial("_SearchResults", Results);
        }
    }
}
