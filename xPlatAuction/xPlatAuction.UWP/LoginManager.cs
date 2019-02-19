using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using System.Linq;
using System;
using Windows.Security.Credentials;


[assembly: Xamarin.Forms.Dependency(typeof(xPlatAuction.UWP.LoginManager))]
namespace xPlatAuction.UWP
{
    public class LoginManager : ILoginManager
    {
        private static MobileServiceClient msClient;

        public MobileServiceUser GetCachedUser(IMobileServiceClient client)
        {
            PasswordVault vault = new PasswordVault();
            try
            {
                var creds = vault.FindAllByResource(client.MobileAppUri.OriginalString).FirstOrDefault();
                if(creds != null)
                {
                    var user = new MobileServiceUser(creds.UserName);
                    creds.RetrievePassword();
                    user.MobileServiceAuthenticationToken = creds.Password;
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task LoginUser(MobileServiceClient client)
        {
            msClient = client;

            var user = await client.LoginAsync(MobileServiceAuthenticationProvider.Facebook, 
                                               Constants.LOGIN_RETURN_URI_SCHEME);
            CacheUserCredentials(client.MobileAppUri.OriginalString, user);
        }

        private void CacheUserCredentials(string mobileAppUrl, MobileServiceUser user)
        {
            PasswordVault vault = new PasswordVault();
            vault.Add(new PasswordCredential(mobileAppUrl, user.UserId, user.MobileServiceAuthenticationToken));
        }

        public static void ResumeLoginWithUrl(Uri url)
        {
            msClient.ResumeWithURL(url);
        }
    }
}
