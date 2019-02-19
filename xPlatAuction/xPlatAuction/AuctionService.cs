using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using xPlatAuction.Models;

namespace xPlatAuction
{
    public class AuctionService
    {
        private static MobileServiceClient azClient;

        public AuctionService(string serviceBaseUri)
        {
            azClient = new MobileServiceClient(serviceBaseUri);
        }

        public async Task<IEnumerable<Auction>>GetAuctions()
        {
            var table = azClient.GetTable<Auction>();
            return await table.ReadAsync();
        }

        public async Task<IEnumerable<AuctionItem>> GetItems(string auctionId)
        {
            var table = azClient.GetTable<AuctionItem>();
            var query = table.Where(ai=>ai.AuctionId == auctionId);
            return await table.ReadAsync(query);
        }

        public async Task<Bid> PlaceBid(Bid newBid)
        {
            var table = azClient.GetTable<Bid>();
            await table.InsertAsync(newBid);
            return newBid;
        }

        public async Task<IEnumerable<MyAuctionItem>> GetMyItems()
        {
            //NOTE: use non-generic overload to return data
            //without serialization
            return await azClient.InvokeApiAsync<IEnumerable<MyAuctionItem>>("MyItems", HttpMethod.Get, null);
        }

        public async Task Login()
        {
            var result = new LoginResult();

            ILoginManager mgr = Xamarin.Forms.DependencyService.Get<ILoginManager>();

            if (mgr != null)
            {
                try
                {
                    await mgr.LoginUser(azClient);
                    result.Succeeded = azClient.CurrentUser != null;
                }
                catch (Exception ex)
                {
                    result.Succeeded = false;
                    result.ErrorMessage = ex.Message;
                }
            }
            else
            {
                result.Succeeded = false;
                result.ErrorMessage = "Login manager is not implemented for the current platform.";
            }

            //notify interested subscribers of the login result.
            MessagingCenter.Send<AuctionService, LoginResult>(this, Constants.MSG_LOGIN_COMPLETE, result);

        }

        public async Task TryLoginWithCachedCredentialsAsync()
        {
            var result = new LoginResult();

            ILoginManager mgr = Xamarin.Forms.DependencyService.Get<ILoginManager>();

            if (mgr != null)
            {
                //try to get cached user credentials and test them
                try
                {
                    var user = mgr.GetCachedUser(azClient);
                    if (user != null)
                    {
                        // test accessing the service with the cached credentials
                        azClient.CurrentUser = user;
                        bool isCredentialValid = await TestLoginCredentialIsValidAsync();
                        if (isCredentialValid)
                        {
                            result.Succeeded = true;
                        }
                        else
                        {
                            result.Succeeded = false;
                            result.ErrorMessage = "Cached credentials expired, please login again.";
                        }
                    }
                }
                catch
                {
                    result.Succeeded  = false;
                    result.ErrorMessage = "Failed to use cached credentials, please login.";
                }
            }
  
            MessagingCenter.Send<AuctionService, LoginResult>(this, Constants.MSG_LOGIN_COMPLETE, result);
        }
        private async Task<bool> TestLoginCredentialIsValidAsync()
        {
            try
            {
                var result = await azClient.InvokeApiAsync("LoginValidation", HttpMethod.Get, null);
                return true;
            }
            catch (Exception ex)
            {
                //test for unauthorized
                return false;
            }   
        }

        public async Task RegisterForNotifications()
        {
            var notifyMgr = DependencyService.Get<INotificationManager>();

            if(notifyMgr != null && notifyMgr.IsDeviceRegisteredForNotifications())
            {
                try
                {
                    await notifyMgr.RegisterForNotifications(azClient);
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
