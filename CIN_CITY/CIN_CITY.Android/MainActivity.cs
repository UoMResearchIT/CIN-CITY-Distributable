using System;
using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using CIN_CITY.Droid.Notifications;

namespace CIN_CITY.Droid
{
    // Place values here as an alternative to in AndroidManifest.xml
    [Activity(
        Label = "string/app_name",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // Initialise plugins
            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);

            // Init the xamarin maps
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            LoadApplication(new App());

            // Create the notifications channel
            LocalNotificationService.CreateNotificationChannel();

            // Check Google service is installed and ok before proceeding
            var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                System.Diagnostics.Debug.WriteLine($"MainActivity {(string)App.Current.Resources["text_google_ok"]}");
            }
            else
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
                {
                    // Attempt to make it avaialble
                    try
                    {
                        GoogleApiAvailability.Instance.MakeGooglePlayServicesAvailable(this);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine($"Could not enable Play Services: {e.Message}");
                    }
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}