using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V7.App;
using Android.Util;
using Firebase.Messaging;

namespace xPlatAuction.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseNotificationService : FirebaseMessagingService
    {
        const string TAG = "FirebaseNotificationService";

        public override void OnMessageReceived(RemoteMessage message)
        {
            var itemName = message.Data["ItemName"];
            var bidAmount = message.Data["BidAmount"];

            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

           
            // Create the notification using the builder.
            var builder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.notification_template_icon_bg)
                .SetContentTitle($"New bid on {itemName}")
                .SetContentText($"{bidAmount} bid on {itemName}")
                .SetContentIntent(pendingIntent)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetAutoCancel(true);
            
            var notificationManager =
               GetSystemService(Context.NotificationService) as Android.App.NotificationManager;
            notificationManager.Notify(0, builder.Build());
        }
    }
}
