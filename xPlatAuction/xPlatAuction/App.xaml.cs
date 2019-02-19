using System.Threading.Tasks;
using Xamarin.Forms;
using xPlatAuction.Models;

[assembly: Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]
namespace xPlatAuction
{
    public partial class App : Application
    {
        private static AuctionService _service;
        private static bool loginSucceeded = false;
        private static bool deviceNotificationRegComplete = false;

        static App()
        {
            _service = new AuctionService("https://xPlatAuctionM3.azurewebsites.net/");
        }
        public App()
        {
            InitializeComponent();

            //force login first
            MainPage = new Login();

            //wait for indication that login completed
            MessagingCenter.Subscribe<AuctionService, LoginResult>(this, Constants.MSG_LOGIN_COMPLETE, LoginComplete);

            //get notified when the device is register with provider for 
            //remote notifications
            MessagingCenter.Subscribe<AuctionService, bool>(this, Constants.MSG_DEVICE_NOTIFY_REG_COMPLETE, NotificationRegComplete);
        }

        public void LoginComplete(AuctionService svc, LoginResult result)
        {
            
            loginSucceeded = result.Succeeded;

            //was the login successful
            if (result.Succeeded)
            {

                Device.BeginInvokeOnMainThread(() =>
                    //login succeeded so we'll go to the main page now
                    MainPage = new NavigationPage(new Auctions())
                    );
                
                //stop listening for login result messages after success
                MessagingCenter.Unsubscribe<AuctionService>(this, Constants.MSG_LOGIN_COMPLETE);

                //login succeeded so try to register for notifications
                TryRegisterForMobileAppNotifications();
            }
            
        }

        public void NotificationRegComplete(AuctionService svc, bool result)
        {
            //indicate that we have successfully registered on the device platform
            deviceNotificationRegComplete = result;

            //if the device was registered with the provider for notifications
            //then register with the Mobile App as well
            if(result)
            {
                //keep listening for new tokens to be registered on android
                //as it could happen at times other than startup. 
                if (Device.RuntimePlatform != Device.Android)
                {
                    //stop listening for notification registration messages after success
                    MessagingCenter.Unsubscribe<AuctionService>(
                        this, Constants.MSG_DEVICE_NOTIFY_REG_COMPLETE);
                }

                TryRegisterForMobileAppNotifications();
            }
            else
            {
                //TODO: notify user notification reg failed on the device
            }
        }

        private async Task TryRegisterForMobileAppNotifications()
        {
            if(loginSucceeded)
            {
                await _service.RegisterForNotifications();
            }
        }

        public static AuctionService GetAuctionService(){
            return _service;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
