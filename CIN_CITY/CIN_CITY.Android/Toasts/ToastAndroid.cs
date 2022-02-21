
using Android.App;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(CIN_CITY.Droid.Toasts.ToastAndroid))]
namespace CIN_CITY.Droid.Toasts
{
    // Android implementation of Toasts
    class ToastAndroid : Features.IToast
    {
        public void ToastLong(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ToastShort(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}