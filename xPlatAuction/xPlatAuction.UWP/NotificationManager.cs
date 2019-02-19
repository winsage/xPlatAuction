using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

[assembly:Xamarin.Forms.Dependency(typeof(xPlatAuction.UWP.NotificationManager))]
namespace xPlatAuction.UWP
{
    public class NotificationManager : INotificationManager
    {
        private static string channelUri = string.Empty;

        internal static void SetChannelUri(string uri)
        {
            channelUri = uri;
        }

        public bool IsDeviceRegisteredForNotifications()
        {
            return !string.IsNullOrEmpty(channelUri);
        }

        public async Task RegisterForNotifications(MobileServiceClient client)
        {
            try
            {
                await client.GetPush().RegisterAsync(channelUri, 
                    GetNotificationTemplates());
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private JObject GetNotificationTemplates()
        {
            const string templateBody = "<toast><visual><binding template=\"ToastText02\"><text id=\"1\">{'New Bid on: ' + $(ItemName)}</text><text id=\"2\">{'Bid Amount: ' + $(BidAmount)}</text></binding></visual></toast>";
            var wnsHeader = new JObject();
            wnsHeader["X-WNS-Type"] = "wns/toast";

            JObject templates = new JObject();
            templates["bidAlert"] = new JObject
            {
                {"body", templateBody},
                {"headers", wnsHeader}
            };

            return templates;
        }
    }
}
