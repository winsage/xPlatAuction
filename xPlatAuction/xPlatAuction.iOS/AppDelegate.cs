using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Facebook.LoginKit;
using ObjCRuntime;
using UserNotifications;

namespace xPlatAuction.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            Facebook.CoreKit.ApplicationDelegate.SharedInstance.FinishedLaunching(app, options);

            // Register for push notifications with the Apple push service.
            //assuming ios 10+
            UNUserNotificationCenter.Current.RequestAuthorization(
                UNAuthorizationOptions.Alert,
              (granted, error) =>
              {
                if(granted)
                {
                    InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                }
              });

            /*var settings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert,
                new NSSet());

            //register the settings and then register for notifications
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
*/


            return base.FinishedLaunching(app, options);
        }

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
            if(url.AbsoluteString.StartsWith(Constants.LOGIN_RETURN_URI_SCHEME, StringComparison.InvariantCultureIgnoreCase))
            {
                LoginManager.ResumeLoginWithUrl(url);
                return true;
            }

            return false;

		}
		public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
            return Facebook.CoreKit.ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation);
		}


        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            
            NotificationManager.SetDeviceToken(deviceToken);
            Xamarin.Forms.MessagingCenter.Send<AuctionService, bool>(
                App.GetAuctionService(), Constants.MSG_DEVICE_NOTIFY_REG_COMPLETE, true);

        }
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
     
            Xamarin.Forms.MessagingCenter.Send<AuctionService, bool>(
                App.GetAuctionService(), Constants.MSG_DEVICE_NOTIFY_REG_COMPLETE, false);
        }


        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;
            if (aps.ContainsKey(new NSString("alert")))
            {
                NSDictionary alertPayload = aps.ObjectForKey(new NSString("alert")) as NSDictionary;


                string alertMessage = string.Empty;
                string title = string.Empty;

                if (alertPayload.ContainsKey(new NSString("title")))
                    title = (alertPayload[new NSString("title")] as NSString).ToString();

                if (alertPayload.ContainsKey(new NSString("body")))
                    alertMessage = (alertPayload[new NSString("body")] as NSString).ToString();


                //show alert
                var alert = UIAlertController.Create(title, alertMessage, UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, null));
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
            }
        }
    }
}
