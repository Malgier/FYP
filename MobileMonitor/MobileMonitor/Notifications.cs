using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.NotificationHubs;

namespace MobileMonitor
{
    public class Notifications
    {
        public static Notifications Instance = new Notifications();

        public NotificationHubClient Hub { get; set; }

        private Notifications()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://mobilemonitornamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=9E5ZQFEJM/r2KPDr9jDjzZPCUNFSaYMp3JLNo7MmgEE=",
                                                                         "MobileMonitorWarning");
        }
    }
}