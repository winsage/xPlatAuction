using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

[assembly: Xamarin.Forms.Dependency(typeof(xPlatAuction.iOS.NotificationManager))]
namespace xPlatAuction.iOS
{
    public class NotificationManager : INotificationManager
    {
        static Foundation.NSData deviceToken;

        public NotificationManager()
        {}

        public static void SetDeviceToken(Foundation.NSData token)
        {
            deviceToken = token;
        }

        public bool IsDeviceRegisteredForNotifications()
        {
            return deviceToken != null;
        }

        public async Task RegisterForNotifications(MobileServiceClient client)
        {
            var templates = GetNotificationTemplates();

            try
            {
                await client.GetPush().RegisterAsync(deviceToken, templates);
            }
            catch(System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private JObject GetNotificationTemplates()
        {
            const string templateBody = "{\"aps\": {\"alert\":{\"title\":\"New bid\", \"body\":\"{$(ItemName) + \' has a new bid of \' + $(BidAmount)}\"}}}";

            JObject templates = new JObject();
            templates["bidAlert"] = new JObject
         {
           {"body", templateBody}
         };

            return templates;
        }

    }
}
