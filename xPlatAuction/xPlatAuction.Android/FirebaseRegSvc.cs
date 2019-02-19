using System.Threading.Tasks;
using Android.App;
using Firebase.Iid;
using Microsoft.WindowsAzure.MobileServices;

namespace xPlatAuction.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseRegistrationService : FirebaseInstanceIdService
    {
        const string TAG = "FirebaseRegistrationService";

        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;

            NotificationManager.SetRegistrationId(refreshedToken);
            Xamarin.Forms.MessagingCenter.Send<AuctionService, bool>(
                App.GetAuctionService(), Constants.MSG_DEVICE_NOTIFY_REG_COMPLETE, false);
        }

    }
}
