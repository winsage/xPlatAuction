using Microsoft.WindowsAzure.MobileServices;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Auth;
using xPlatAuction;

[assembly: Xamarin.Forms.Dependency(typeof(xPlatAuction.Droid.LoginManager))]
namespace xPlatAuction.Droid
{
    public class LoginManager : ILoginManager
    {
        public MobileServiceUser GetCachedUser(IMobileServiceClient client)
        {
            var ctx = Xamarin.Forms.Forms.Context;
            var store = AccountStore.Create(ctx);

            var account = store.FindAccountsForService(client.MobileAppUri.OriginalString).FirstOrDefault();
            if(account != null)
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

            var ctx = Xamarin.Forms.Forms.Context;

            var user = await client.LoginAsync(ctx,
                                               MobileServiceAuthenticationProvider.Twitter,
                                               Constants.LOGIN_RETURN_URI_SCHEME);

            CacheUserCredentials(client.MobileAppUri.OriginalString, user);
        }


        private void CacheUserCredentials(string mobileAppUrl, MobileServiceUser user)
        {
            
            var ctx = Xamarin.Forms.Forms.Context;
            var store = AccountStore.Create(ctx);

            var account = new Account(user.UserId);
            account.Properties.Add("token", user.MobileServiceAuthenticationToken);
            store.Save(account, mobileAppUrl);
        }
    }
}