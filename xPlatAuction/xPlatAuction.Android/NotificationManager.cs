using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

[assembly:Xamarin.Forms.Dependency(typeof(xPlatAuction.Droid.NotificationManager))]
namespace xPlatAuction.Droid
{
    public class NotificationManager : INotificationManager
    {
        private static string regId = Firebase.Iid.FirebaseInstanceId.Instance.Token;

        public NotificationManager(){}

        public async Task RegisterForNotifications(MobileServiceClient client)
        {
            await client.GetPush().RegisterAsync(regId,GetNotificationTemplates());
        }

        private JObject GetNotificationTemplates()
        {
            const string templateBody = "{\"data\":{\"ItemName\":\"$(ItemName)\", \"BidAmount\":\"$(BidAmount)\"}}";

            JObject templates = new JObject();
            templates["bidAlert"] = new JObject
         {
           {"body", templateBody}
         };

            return templates;
        }

        public static void SetRegistrationId(string registrationId)
        {
            regId = registrationId;
        }

        public bool IsDeviceRegisteredForNotifications()
        {
            return !string.IsNullOrEmpty(regId);
        }
    }
}
