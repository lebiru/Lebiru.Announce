using Lebiru.Announce.Models;
using System.IO;
using Newtonsoft.Json;
using BCrypt.Net;

namespace Lebiru.Announce.Services
{
    public class AdminService
    {
        private const string AdminFilePath = "admin_credentials.json";

        public AdminCredentials GetAdminCredentials()
        {
            if (!File.Exists(AdminFilePath)) return null;

            var json = File.ReadAllText(AdminFilePath);
            return JsonConvert.DeserializeObject<AdminCredentials>(json);
        }

        public bool AdminExists() => File.Exists(AdminFilePath);

        public void CreateAdminAccount(string username, string password)
        {
            var credentials = new AdminCredentials
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            var json = JsonConvert.SerializeObject(credentials);
            File.WriteAllText(AdminFilePath, json);
        }

        public bool ValidateAdmin(string username, string password)
        {
            var credentials = GetAdminCredentials();
            if (credentials == null) return false;

            return credentials.Username == username && BCrypt.Net.BCrypt.Verify(password, credentials.PasswordHash);
        }
    }
}
