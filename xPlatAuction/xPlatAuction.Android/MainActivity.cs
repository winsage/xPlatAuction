using System;
using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace xPlatAuction.Droid
{
    [Activity(Label = "xPlatAuction", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                //log error
                Android.Util.Log.WriteLine(Android.Util.LogPriority.Debug, "GMS",resultCode.ToString());
            }
            else
            {
                Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
                global::Xamarin.Forms.Forms.Init(this, bundle);
                LoadApplication(new App());
            }

        }
    }
}

