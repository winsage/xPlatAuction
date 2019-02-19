using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Auth;
using xPlatAuction;

[assembly: Xamarin.Forms.Dependency(typeof(xPlatAuction.iOS.LoginManager))]
namespace xPlatAuction.iOS
{
    public class LoginManager : ILoginManager
    {
        private static MobileServiceClient msClient;

        public MobileServiceUser GetCachedUser(IMobileServiceClient client)
        {
            
            var store = AccountStore.Create();

            var account = store.FindAccountsForService(client.MobileAppUri.OriginalString).FirstOrDefault();
            if (account != null)
            {
                var user = new MobileServiceUser(account.Username);
                user.MobileServiceAuthenticationToken = account.Properties["token"];
                return user;
            }
            else
            {
                return null;
            }
        }
        public async Task LoginUser(MobileServiceClient client)
        {
            msClient = client;

            bool useFBSDK = true;

            var viewController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (useFBSDK)
            {
                
                var fbLoginMgr = new Facebook.LoginKit.LoginManager();
                var fbLoginResult = await fbLoginMgr.LogInWithReadPermissionsAsync(
					new string[] { "public_profile" }, viewController);
                string token = fbLoginResult.Token.TokenString;

                JObject tokenPayload = new JObject();
                tokenPayload.Add("access_token", token);

                var user = await client.LoginAsync(MobileServiceAuthenticationProvider.Facebook, 
                                                   tokenPayload);

                CacheUserCredentials(client.MobileAppUri.OriginalString, user);
            }
            else
            {
      
                var user = await client.LoginAsync(viewController, MobileServiceAuthenticationProvider.Facebook, 
                                                   Constants.LOGIN_RETURN_URI_SCHEME);

                CacheUserCredentials(client.MobileAppUri.OriginalString, user);
            }
        }
        public static void ResumeLoginWithUrl(Foundation.NSUrl url)
        {
            msClient.ResumeWithURL(url);
        }
        private void CacheUserCredentials(string mobileAppUrl, MobileServiceUser user)
        {
            var store = AccountStore.Create();

            var account = new Account(user.UserId);
            account.Properties.Add("token", user.MobileServiceAuthenticationToken);
            store.Save(account, mobileAppUrl);
        }
    }
}