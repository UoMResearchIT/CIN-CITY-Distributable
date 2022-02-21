using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CIN_CITY.Features
{
    // Class to check if GPS is enabled and attempts to get the latitude and longitude values of the device
    public class GPSGetter
    {
        // Latitude of device location
        public double Latitude = 0.0;
        // Longitude of device location
        public double Longitude = 0.0;
        // Date and time the device location was obtained
        public string LastKnownLocationAt = "00:00:00";
        // Whether GPS handling in the App is working ok or not
        public bool statusGps = false;

        // Result value of a failed GPS call
        public int FailResult { get; set; } = 99;

        // Result message of a failed GPS call
        public string FailMessage { get; set; } = "Unknown Location related technical problem. Please check your settings.";

        // Obtains location details for the device
        // Coarse and Fine Location permissions must be set up in the App to access the GPS coordinates of the device
        public async Task<bool> GetGps()
        {
            Debug.WriteLine("GPS Task Started");
            FailResult = 0;
            // Full request for current location
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(6));
                var location = await Geolocation.GetLocationAsync(request);

                // If location access was sucessful store the latitude and longitude values
                if (location != null)
                {
                    Debug.WriteLine($"GPSGetter: Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    LastKnownLocationAt = DateTime.Now.ToString("dd/MM/yy HH:mm:ss");
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    statusGps = true;
                    return true;
                }
                else
                {
                    // Location access was UNsucessful 
                    Debug.WriteLine("GPSGetter: Unable to establish Latitude & Longitude");
                    statusGps = false;
                    FailResult = -1;
                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
                Debug.WriteLine((string)Application.Current.Resources["text_location_supported"]);
                FailResult = 1;
                FailMessage = (string)Application.Current.Resources["text_location_supported"];
                return false;
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
                Debug.WriteLine((string)Application.Current.Resources["text_location_enabled"]);
                FailResult = 2;
                FailMessage = (string)Application.Current.Resources["text_location_enabled"];
                return false;
            }
            catch (PermissionException)
            {
                // Handle permission exception
                Debug.WriteLine((string)Application.Current.Resources["text_location_permission"]);
                FailResult = 3;
                FailMessage = (string)Application.Current.Resources["text_location_permission"];
                return false;
            }
            catch (Exception e)
            {
                // Unable to get location
                Debug.WriteLine((string)Application.Current.Resources["text_location_problemunknown"] + " " + e.Message);
                FailResult = 4;
                FailMessage = (string)Application.Current.Resources["text_location_problemunknown"];
                return false;
            }
            return false;
        }

        // Initiates a timed check for GPS 
        public bool TimedGPSTask()
        //public static bool TimedTask(Action<bool> MethodToCall)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var myTask = GetGps();
            bool completed = true;
            try
            {
                // Set up the CancellationTokenSource e.g. to cancel after 2.5 seconds is 2500 milli                  
                cts.CancelAfter((int)Application.Current.Resources["checkGPSWaitTime"]);
                myTask.Wait(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"TimedIsConnected status 1 == {myTask.Status}");
                Debug.WriteLine("OperationCanceledException Task () CANCELLED. Task took too long");
                Debug.WriteLine($"Is status cancelled: {myTask.IsCanceled}");
                completed = false;
            }
            Debug.WriteLine($"TimedIsConnected status 2 == {myTask.Status}");
            return completed ? myTask.Result : false;
        }
    }
}
