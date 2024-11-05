using Lebiru.Announce.Models;
using System.IO;
using Newtonsoft.Json;

namespace Lebiru.Announce.Services
{
    public class BannerService
    {
        private const string BannerFilePath = "banner.json";

        public Banner GetBanner()
        {
            if (!File.Exists(BannerFilePath)) return null;

            var json = File.ReadAllText(BannerFilePath);
            return JsonConvert.DeserializeObject<Banner>(json);
        }

        public void SetBanner(string message)
        {
            var banner = new Banner { Message = message };
            var json = JsonConvert.SerializeObject(banner);
            File.WriteAllText(BannerFilePath, json);
        }

        public void ClearBanner()
        {
            if (File.Exists(BannerFilePath))
            {
                File.Delete(BannerFilePath);
            }
        }
    }
}
