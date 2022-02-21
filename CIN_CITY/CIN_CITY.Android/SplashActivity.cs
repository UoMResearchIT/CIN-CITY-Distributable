using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using AndroidX.AppCompat.App;

namespace CIN_CITY.Droid
{
    // NoHistory means that the activity is removed from the back stack
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/SplashTheme",
        MainLauncher = true,
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    // Android implementation of the Splash Screen which is the first screen visible to the user when the app is launched
    // A CinCity icon is displayed whilst the initial screen is established and presented
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        // Usual OnCreate
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();

            // Kick off a background task which creates the intent to start the main activity
            Task startupWork = new Task(() =>
            {
                App.AppStartedFromNotification = Intent.GetBooleanExtra(App.AppStartedByNotificationKey, false);
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            });
            startupWork.Start();
        }

        public override void OnBackPressed() { }
    }
}