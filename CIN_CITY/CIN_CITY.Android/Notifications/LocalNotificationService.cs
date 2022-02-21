using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.Media;
using AndroidX.Core.App;
using CIN_CITY.Features;
using Java.Lang;

[assembly: Xamarin.Forms.Dependency(typeof(CIN_CITY.Droid.Notifications.LocalNotificationService))]
namespace CIN_CITY.Droid.Notifications
{
    // Android implementation of notifications
    internal class LocalNotificationService : ILocalNotificationService
    {
        internal string randomNumber = "999999";

        // The repeating interval in our application is 1 day
        const long repeatInADayIntervalMillis = 24 * 60 * 60 * 1000;

        // Implementation of the notification creator

        // Create Alert for the specified time then repeat at intervals depending on no of alerts specified
        public void LocalNotification(string title, string body, int id, int noOfNotifications, DateTime startNotifyTime, DateTime endNotifyTime)
        {
            // Default to 7am - 11pm when alerts are sent

            // startNotifyTime & endNotifyTime left in as parameters for now but default values are currently used

            // Set up starttime = 7am and endtime = 11pm
            DateTime defaultStart = DateTime.Now;
            TimeSpan ts = new TimeSpan(7, 00, 0);
            defaultStart = defaultStart.Date + ts;

            DateTime defaultEnd = DateTime.Now;
            TimeSpan ts1 = new TimeSpan(23, 00, 0);
            defaultEnd = defaultEnd.Date + ts1;

            Debug.WriteLine("Alerts:  default start : " + defaultStart);
            Debug.WriteLine("Alerts:  default end : " + defaultEnd);

            // Convert notify to miliseconds -- the Android system is expecting it in Unix epoch time
            DateTimeOffset dto1 = new DateTimeOffset(defaultStart);
            long startTriggerAlarmAt = dto1.ToUnixTimeMilliseconds();

            DateTimeOffset dto2 = new DateTimeOffset(defaultEnd);
            long endTriggerAlarmAt = dto2.ToUnixTimeMilliseconds();

            Debug.WriteLine("LocalNotificationService ****   initial start dt01 " + dto1);
            Debug.WriteLine("LocalNotificationService ****   initial end dto2 " + dto2);
            Debug.WriteLine("LocalNotificationService ****   initial start notify " + defaultStart);
            Debug.WriteLine("LocalNotificationService ****   initial end notify " + defaultEnd);
            Debug.WriteLine("LocalNotificationService ****   startTriggerAlarmAt millis " + startTriggerAlarmAt);
            Debug.WriteLine("LocalNotificationService ****   endTriggerAlarmAt millis " + endTriggerAlarmAt);

            // If start time is behind current time then add a day to both values
            // Only happens if user set values, won't happen if using default start/end times as currently done
            /* ALSO COMMENT OUT WHILST TESTING..... */
            var currentMilli = JavaSystem.CurrentTimeMillis();
            if (startTriggerAlarmAt < currentMilli)
            {
                startTriggerAlarmAt = startTriggerAlarmAt + repeatInADayIntervalMillis;
                endTriggerAlarmAt = endTriggerAlarmAt + repeatInADayIntervalMillis;
            }

            // Always have at least one notification if this code is called (should only be called if alerts enabled)
            if (noOfNotifications == 0) noOfNotifications = 1;
            long repeatIntervalMillis = (endTriggerAlarmAt - startTriggerAlarmAt) / noOfNotifications;

            Debug.WriteLine("LocalNotificationService **** RepeatIntervalMillis " + repeatIntervalMillis);

            // The no of intents/alerts generated is 'noOfNotifications' per day 
            for (int i = 0; i < noOfNotifications; i++)
            {
                DateTime newstarttime;
                Debug.WriteLine("LocalNotificationService ****  noOfNotifications loop " + i);

                // Only on the first time use start notify time otherwise add on the repeat interval value each time
                if (i != 0)
                {
                    // add on repeatinterval to the start time
                    dto1 = dto1.AddMilliseconds(repeatIntervalMillis);
                    newstarttime = dto1.DateTime;
                    startTriggerAlarmAt += repeatIntervalMillis;
                }
                else
                {
                    newstarttime = defaultStart;
                }
                // Create the notification object Resource.Mipmap.icon
                var localNotification = new LocalNotificationModel()
                {
                    Title = title,
                    Body = body,
                    Id = id,
                    IconId = Resource.Drawable.ic_alert_cincity_icon,
                    NotifyTime = newstarttime
                };
                
                CreateIntentForAlarmManager(localNotification, startTriggerAlarmAt, repeatInADayIntervalMillis);
                Debug.WriteLine("LocalNotificationService **** New start time " + newstarttime);
                Debug.WriteLine("LocalNotificationService **** startTriggerAlarmAt " + startTriggerAlarmAt);
            }
        }
        // Create an intent and pending intent and set the alarm details with the AlarmManager
        private void CreateIntentForAlarmManager(LocalNotificationModel localNotification, long startTriggerAlarmAt, long repeatIntervalMillis)
        {
            // Create intent and attach the serialised notification using the "LocalNotificationKey" as an ID for the info
            var intent = CreateIntent(localNotification.Id);
            var serialisedNotification = SerialiseNotification(localNotification);
            intent.PutExtra(ScheduledAlarmHandler.LocalNotificationKey, serialisedNotification);

            // Generate a random ID for the intent
            Random generator = new Random();
            randomNumber = generator.Next(100000, 999999).ToString("D6");

            // Get pending intent (description of intent and activity to perform with it) from this application
            var pendingIntent = PendingIntent.GetBroadcast(
                Application.Context,
                Convert.ToInt32(randomNumber),
                intent, PendingIntentFlags.Immutable);

            // Give pending intent to the alarm manager including repeating time in milliseconds
            var alarmManager = GetAlarmManager();
            alarmManager.SetRepeating(AlarmType.RtcWakeup, startTriggerAlarmAt, repeatIntervalMillis, pendingIntent);
        }

        // Implementation of the cancellation of the alerts
        public void Cancel(int id)
        {
            // Get the existing intent and remove it from both the notification and alarm manager
            var intent = CreateIntent(id);
            var pendingIntent = PendingIntent.GetBroadcast(
                Application.Context,
                Convert.ToInt32(randomNumber),
                intent, PendingIntentFlags.Immutable);
            var alarmManager = GetAlarmManager();
            alarmManager.Cancel(pendingIntent);
            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.CancelAll();
            notificationManager.Cancel(id);
        }

        // Helper method to get an intent to launch this activity which will be used to launch the app when the notification is tapped
        public static Intent GetAppLaunchIntent()
        {
            var packageName = Application.Context.PackageName;
            return Application.Context.PackageManager.GetLaunchIntentForPackage(packageName);
        }

        // Helper method to generate an intent to be received by the broadcast receiver
        private Intent CreateIntent(int id)
        {
            return new Intent(Application.Context, typeof(ScheduledAlarmHandler))
                .SetAction("LocalNotifierIntent" + id);
        }

        // Getter for the Android alarm manager
        private AlarmManager GetAlarmManager()
        {
            var alarmManager = Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
            return alarmManager;
        }

        // Serialise the notification
        private string SerialiseNotification(LocalNotificationModel notification)
        {

            var xmlSerializer = new XmlSerializer(notification.GetType());

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, notification);
                return stringWriter.ToString();
            }
        }

        // Android O upwards needs a channel creating for notifications
        internal static void CreateNotificationChannel()
        {
            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = "General";
            var channelDescription = "Enable all reminders";
            var channel = new NotificationChannel("default_channel", channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        // Static method which can be called from anywhere to fire a notification
        internal static void FireNotification(LocalNotificationModel notification)
        {
            // Generate notification
            var notificationBuilder = new NotificationCompat.Builder(Application.Context, "default_channel")
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Body)
                .SetSmallIcon(notification.IconId)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetAutoCancel(true);

            // Create launch intent and add to a stack builder
            var appLaunchIntent = GetAppLaunchIntent();
            appLaunchIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            var stackBuilder = AndroidX.Core.App.TaskStackBuilder.Create(Application.Context);
            stackBuilder.AddNextIntent(appLaunchIntent);

            // Generate new intent ID
            Random random = new Random();
            int randomNumber = random.Next(9999 - 1000) + 1000;

            // Add the task stack intent to the notification
            var appLaunchPendingIntent =
                stackBuilder.GetPendingIntent(randomNumber, (int)PendingIntentFlags.Immutable);
            notificationBuilder.SetContentIntent(appLaunchPendingIntent);

            // Post notification    
            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.Notify(randomNumber, notificationBuilder.Build());
        }
    }

    // Implement our broadcast receiver to intercept LocalNotifications
    // This attribute should make it statically registered in the manifest and so still run even when the app is closed.
    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    public class ScheduledAlarmHandler : BroadcastReceiver
    {
        // Key to identify a local notification intent
        public const string LocalNotificationKey = "LocalNotification";

        // What to do when intent is received
        // this is an intent to load notification not launch the app
        public override void OnReceive(Context context, Intent intent)
        {
            Console.WriteLine("ALARM RECEIVED");
            // Get the serialised notification from the intent
            var extra = intent.GetStringExtra(LocalNotificationKey);
            var notification = DeserialiseNotification(extra);

            // Generate notification
            var notificationBuilder = new NotificationCompat.Builder(Application.Context, "default_channel")
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Body)
                .SetSmallIcon(notification.IconId)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetAutoCancel(true);

            // Create launch intent and add to a stack builder
            var appLaunchIntent = LocalNotificationService.GetAppLaunchIntent();
            appLaunchIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            appLaunchIntent.PutExtra(App.AppStartedByNotificationKey, true);

            System.Diagnostics.Debug.WriteLine("LocalNotificationService: Values added to intent: " + appLaunchIntent.GetBooleanExtra(App.AppStartedByNotificationKey, false));
            var stackBuilder = AndroidX.Core.App.TaskStackBuilder.Create(Application.Context);
            stackBuilder.AddNextIntent(appLaunchIntent);

            // Generate new intent ID
            System.Random random = new System.Random();
            int randomNumber = random.Next(9999 - 1000) + 1000;

            // Add the task stack intent to the notification
            var appLaunchPendingIntent =
                stackBuilder.GetPendingIntent(randomNumber, (int)PendingIntentFlags.Immutable);
            notificationBuilder.SetContentIntent(appLaunchPendingIntent);

            // Post notification    
            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.Notify(randomNumber, notificationBuilder.Build());
        }
        // De-serialise the notification
        private LocalNotificationModel DeserialiseNotification(string notificationString)
        {
            var xmlSerializer = new XmlSerializer(typeof(LocalNotificationModel));
            using (var stringReader = new StringReader(notificationString))
            {
                var notification = (LocalNotificationModel)xmlSerializer.Deserialize(stringReader);
                return notification;
            }
        }

        // Helper method to get an intent to launch this activity which will be used to launch the app when the notification is tapped
        public static Intent GetAppLaunchIntent()
        {
            var packageName = Application.Context.PackageName;
            return Application.Context.PackageManager.GetLaunchIntentForPackage(packageName);
        }
    }

        // Broadcast receiver to fire a notification when a boot complete intent is intercepted
        // Will ensure notifications are started even if the phone has been re-booted
        [BroadcastReceiver(Enabled = true, Label = "Boot Broadcast Receiver")]
        [IntentFilter(new[] { Intent.ActionBootCompleted })]
        public class BootNotificationReceiver : BroadcastReceiver
        {
            // What to do when intent is received
            public override void OnReceive(Context context, Intent intent)
            {
                Debug.WriteLine("BOOT RECEIVED");

                // Create notification using Android resources
                var notification = new LocalNotificationModel
                {
                    Title = Application.Context.Resources.GetString(Resource.String.boot_notification_title),
                    Body = Application.Context.Resources.GetString(Resource.String.boot_notification_text),
                    Id = 0,
                    IconId = Resource.Drawable.ic_alert_cincity_icon
                };

                // Fire notification
                LocalNotificationService.FireNotification(notification);
            }
        }
}