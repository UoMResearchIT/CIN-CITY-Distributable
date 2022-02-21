using System;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;

namespace CIN_CITY.Features
{
    // Definitions of data stored locally
    // Data is initially stored from User input and then is re-used each time a report is submitted
    public class Settings
    {
        private static string GetName([CallerMemberName] string propertyName = "")
        {
            return propertyName;
        }

        // Age of the User
        public static string Age { get => Preferences.Get(GetName(), null); set => Preferences.Set(GetName(), value); }

        // Gender of the User
        public static string Gender { get => Preferences.Get(GetName(), null); set => Preferences.Set(GetName(), value); }

        // Ethnicity of the User
        public static string Ethnicity { get => Preferences.Get(GetName(), null); set => Preferences.Set(GetName(), value); }

        // Up to first 3 characters of the home post code for the User
        public static string HomePC { get => Preferences.Get(GetName(), null); set => Preferences.Set(GetName(), value); }

        // Up to first 3 characters of the home school code for the User
        public static string SchoolPC { get => Preferences.Get(GetName(), null); set => Preferences.Set(GetName(), value); }

        // Whether the User has been personally involved in a previous incident
        public static string PrevIncident { get => Preferences.Get(GetName(), null); set => Preferences.Set(GetName(), value); }

        // Number of notifications generated per day
        public static int NoOfNotifications { get => Preferences.Get(GetName(), 0); set => Preferences.Set(GetName(), value); }

        // Whether notifications are enabled or not
        public static bool Alert { get => Preferences.Get(GetName(), false); set => Preferences.Set(GetName(), value); }

        // Start time in the day for the notifications -- store in preferences as minutes
        public static TimeSpan StartAlertTime { get => TimeSpan.FromMinutes(Preferences.Get(GetName(), 0d)); set => Preferences.Set(GetName(), value.TotalMinutes); }

        // Finish time in the day for the notifications -- store in preferences as minutes
        public static TimeSpan EndAlertTime { get => TimeSpan.FromMinutes(Preferences.Get(GetName(), 0d)); set => Preferences.Set(GetName(), value.TotalMinutes); }
    }
}
