using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CIN_CITY.Features;
using CIN_CITY.Services;
using CIN_CITY.ViewModels;
using CIN_CITY.Views;
using CIN_CITY.Views.Questions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CIN_CITY
{
    public partial class App : Application
    {
        // View models for onboarding
        internal static AboutDemoPage1ViewModel vm1;
        internal static AboutEthPostCodesPage2ViewModel vm2;
        internal static AboutNotiPage4ViewModel vm4;

        // Originally used with the alert handling intent
        public const string AppStartedByNotificationKey = "fromNotification";
        public static bool AppStartedFromNotification;

        // GPS Wrapper class which obtains current device latitude and longitude values
        public static GPSGetter gps = new GPSGetter();

        public App()
        {
            InitializeComponent();
            VersionTracking.Track();

            // Initialise the view models
            InitViewModels();

            // Assign the login page
            MainPage = new LoginPage();
        }

        // Executed when the App starts up
        protected override async void OnStart()
        {
            // Ask for permissions
            await GetPermissionAsync();

            // Check onboarding complete
            if (string.IsNullOrEmpty(Settings.Age))
            {
                Debug.WriteLine("Onboarding Incomplete!");
                return;
            }

            // Set the main page as we passed the checks for being able to skip login
            MainPage = new MainPage();

            // Go straight to questionnaire page
            if (AppStartedFromNotification)
            {
                (MainPage as FlyoutPage).Detail = new NavigationPage(new SpatialQuestionsPage(ReportType.FromNotfication));
                AppStartedFromNotification = false;
            }
        }

        internal static void UpdateSettings()
        {
            // Update gender and age
            if (vm1 != null)
            {
                Settings.Gender = vm1.SelectedGender;
                Settings.Age = vm1.SelectedAge;
            }
            // update ethnicity, home and school post code truncated details
            if (vm2 != null)
            {
                Settings.Ethnicity = vm2.SelectedEthnicity;
                Settings.HomePC = vm2.HomePC;
                Settings.SchoolPC = vm2.SchoolPC;
            }
            // update the alert preferences of the user
            if (vm4 != null)
            {
                Settings.NoOfNotifications = vm4.SelectedNoOfNotifications;
                Settings.Alert = vm4.AlarmSwitch;
                // current default is to 7am & 11pm
                Settings.StartAlertTime = vm4.StartAlarmTime;
                Settings.EndAlertTime = vm4.EndAlarmTime;
            }

            Debug.WriteLine("SETTINGS UPDATED");
        }

        /// <summary>
        /// Initialise the view models
        /// </summary>
        internal void InitViewModels()
        {
            // Create new view models
            if (vm1 == null)
            {
                vm1 = new AboutDemoPage1ViewModel()
                {
                    SelectedAge = Settings.Age ?? "",
                    SelectedGender = Settings.Gender ?? "",
                };
            }
            if (vm2 == null)
            {
                vm2 = new AboutEthPostCodesPage2ViewModel()
                {
                    SelectedEthnicity = Settings.Ethnicity,
                    HomePC = Settings.HomePC,
                    SchoolPC = Settings.SchoolPC
                };
            }
            if (vm4 == null)
            {
                vm4 = new AboutNotiPage4ViewModel()
                {
                    StartAlarmTime = Settings.StartAlertTime,
                    EndAlarmTime = Settings.EndAlertTime,
                    SelectedNoOfNotifications = Settings.NoOfNotifications,
                    AlarmSwitch = Settings.Alert
                };
            }
        }

        // Check that location permissions have been enabled
        internal async Task<bool> GetPermissionAsync()
        {
            try
            {
                PermissionStatus status = new PermissionStatus();
                status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                // If OK, can continue in App
                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                bool premissionsGranted = false;

                // Loop around until permissions are granted, can't continue unless location permissions are enabled
                while (!premissionsGranted)
                {
                    // If already denied, show an appropriate message
                    if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
                    {
                        await MainPage.DisplayAlert("Permission", (string)Application.Current.Resources["text_location_accessneeded"], "OK");
                    }

                    // Request the permissions again
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (status == PermissionStatus.Granted)
                    {
                        premissionsGranted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return true;
        }
    }
}
