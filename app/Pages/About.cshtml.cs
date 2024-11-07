using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lebiru.Announce.Pages
{
    public class AboutModel : PageModel
    {
        public void OnGet()
        {
            ViewData["HeroTitle"] = "About Us"; // Set default title for About page
            ViewData["BannerMessage"] = null;   // Optionally, set default banner message or leave null
        }
    }
}
