using System;
using System.Collections.Generic;
using System.Diagnostics;
using CIN_CITY.Features;
using Xamarin.Forms;

namespace CIN_CITY.ViewModels
{
    // view model defining alert preferences i.e. whether alerts are enabled and how many to generate
    class AboutNotiPage4ViewModel : VMWithObservableProperty
    {
        // Icon
        public ImageSource PageIcon { get; private set; }
            = ImageSource.FromFile((OnPlatform<string>)Application.Current.Resources["icon_alarm"]);

        // Notification ID -- only need one
        readonly int notificationId = 12345;

        // Enable or disable alerts switch

        private bool alarmSwitch = false;
        public bool AlarmSwitch
        {
            get
            {
                return alarmSwitch;
            }
            set
            {
                // Reset number of notifications to zero if alerts are disabled, default 5 if enabled
                alarmSwitch = value;
                if (alarmSwitch && SelectedNoOfNotifications == 0) SelectedNoOfNotifications = 5;
                else if (!alarmSwitch) SelectedNoOfNotifications = 0;
                OnPropertyChanged("AlarmSwitch");
            }
        }
        // Number of notifications a day picker 
        private bool noNotificationsPickerEnabled = true;
        public bool NoNotificationsPickerEnabled
        {
            get
            {
                return noNotificationsPickerEnabled;
            }
            set
            {
                noNotificationsPickerEnabled = value;
                OnPropertyChanged("NoNotificationsPickerEnabled");
            }
        }

        // Currently default to 7am/previously entered in on the page
        private TimeSpan startAlarmTime = new TimeSpan(7, 00, 0);
        public TimeSpan StartAlarmTime
        {
            get
            {
                return startAlarmTime;
            }
            set
            {
                startAlarmTime = value;
                OnPropertyChanged("StartAlarmTime");
            }
        }

        // Currently default to 11pm/previously entered in on the page
        private TimeSpan endAlarmTime = new TimeSpan(23, 00, 0);
        public TimeSpan EndAlarmTime
        {
            get
            {
                return endAlarmTime;
            }
            set
            {
                endAlarmTime = value;
                OnPropertyChanged("EndAlarmTime");
            }
        }

        // List of the number of notifications per day
        public List<int> NoOfNotifications { get; set; } = new List<int>();

        // Selected number of notifications
        private int selectedNoOfNotifications;
        public int SelectedNoOfNotifications
        {
            get
            {
                return selectedNoOfNotifications;
            }
            set
            {
                selectedNoOfNotifications = value;
                if (selectedNoOfNotifications > 0)
                {
                    AlarmSwitch = true;
                    OnPropertyChanged("AlarmSwitch");
                }
                OnPropertyChanged("SelectedNoOfNotifications");
            }
        }

        internal void UpdateAlarm()
        {
            // Cancel existing alarm 
            Debug.WriteLine("AboutNotiPage4ViewModel: ALARM CANCELLED!");
            DependencyService.Get<ILocalNotificationService>().Cancel(notificationId);

            // Set new alarm
            if (AlarmSwitch == false)
            {
                SelectedNoOfNotifications = 0;
            }
            else
            {
                Debug.WriteLine("AboutNotiPage4ViewModel: SETTING NEW ALERTs!");
                
                // Start/End times were original values displayed on the page
                // Now default 7am - 11pm N.B. left in for now as original method expects them

                // Generate start alarm time 
                DateTime timeNow = DateTime.Now;
                DateTime startTimeForAlarm = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, 7, 0, 0);

                 // Generate end alarm time 
                DateTime endTimeForAlarm = new DateTime(timeNow.Year, timeNow.Month, timeNow.Day, 23, 0, 0);

                // Call the native notification service and set up the alarm/alert times
                DependencyService.Get<ILocalNotificationService>()
                    .LocalNotification(
                    (string)Application.Current.Resources["text_notification_title"],
                    (string)Application.Current.Resources["text_notification_text"],
                    notificationId, SelectedNoOfNotifications, startTimeForAlarm, endTimeForAlarm);
            }
        }

        // Ctor
        public AboutNotiPage4ViewModel()
        {
            // Set up list of number of notifications 1 - 5
            for (int y = 1; y < 6; y++) NoOfNotifications.Add(y);
        }
    }
}

