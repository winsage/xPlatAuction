using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace xPlatAuction
{
    public interface INotificationManager
    {
        Task RegisterForNotifications(MobileServiceClient client);

        bool IsDeviceRegisteredForNotifications();
    }
}
