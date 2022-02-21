using System;
using System.Collections.Generic;
using System.Text;

namespace CIN_CITY.Features
{
    // Indicates which type of report this is
    public enum ReportType
    {
        // 0 - 'Report Now' on 'Home'  
        // 1 - 'Notification Tapped'
        // 2 - 'Report in response to an alert' on 'Home'
        // 3 – 'Previous Incident' on 'Home'

        ReportNow = 0,
        FromNotfication = 1,
        FromAlert = 2,
        PreviousIncident = 3
    }
}
