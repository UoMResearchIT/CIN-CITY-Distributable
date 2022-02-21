using System;

namespace CIN_CITY.Features
{
    // Base class to help implement notications in native code 
    public class LocalNotificationModel
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public int Id { get; set; }

        public int IconId { get; set; }

        public DateTime NotifyTime { get; set; }
    }
}
