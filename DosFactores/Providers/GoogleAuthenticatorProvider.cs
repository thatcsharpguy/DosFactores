using System.Threading.Tasks;
using DosFactores.Data;
using DosFactores.Models;
using Google.Authenticator;
using Microsoft.AspNetCore.Identity;

namespace DosFactores.Providers
{
    public class GoogleAuthenticatorProvider : IUserTwoFactorTokenProvider<ApplicationUser>
    {
        public static readonly string ProviderName = "GoogleAuthenticator";

        private readonly ApplicationDbContext _dbContext;

        public GoogleAuthenticatorProvider(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            var userName = await manager.GetUserNameAsync(user);
            var authenticator = new TwoFactorAuthenticator();
            var code = authenticator.GenerateSetupCode(userName, userName, 100, 100);
            return code.ManualEntryKey;
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                var authenticator = new TwoFactorAuthenticator();
                return authenticator.ValidateTwoFactorPIN(user.TfaKey, token);
            });
        }

        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                // Verify if the user is ellegible for 2FA
                return true;
            });
        }
    }
}
