using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace xPlatAuction
{
    public interface ILoginManager
    {
        Task LoginUser(MobileServiceClient client);

        MobileServiceUser GetCachedUser(IMobileServiceClient client);
    }
}
