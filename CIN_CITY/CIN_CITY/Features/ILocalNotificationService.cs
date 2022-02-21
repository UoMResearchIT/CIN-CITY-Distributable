using System;

namespace CIN_CITY.Features
{
    // Interface to allow alerts to be implemented in native code 
    public interface ILocalNotificationService
    {
        void LocalNotification(string title, string body, int id, int noOfNotifications, DateTime startNotifyTime, DateTime endNotifyTime);
        void Cancel(int id);
    }
}
