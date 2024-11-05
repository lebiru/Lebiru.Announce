using Lebiru.Announce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lebiru.Announce.Pages.Banner
{
    // [Authorize]
    public class UpdateModel : PageModel
    {
        private readonly BannerService _bannerService;

        [BindProperty]
        public string Message { get; set; }

        public UpdateModel(BannerService bannerService)
        {
            _bannerService = bannerService;
        }

        public void OnGet()
        {
            var currentBanner = _bannerService.GetBanner();
            Message = currentBanner?.Message;
        }

        public IActionResult OnPostSetBanner()
        {
            if (!string.IsNullOrWhiteSpace(Message))
            {
                _bannerService.SetBanner(Message);
            }
            return RedirectToPage("/Banner/Update");
        }

        public IActionResult OnPostClearBanner()
        {
            _bannerService.ClearBanner();
            return RedirectToPage("/Banner/Update");
        }
    }
}
