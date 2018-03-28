using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTrayApp.Models
{
    public class ServerBackup
    {
        public int BackupID { get; set; }
        public string Name { get; set; }
        public DateTime ScheduledTime { get; set; }
        public string Frequency { get; set; }
        public string BackupType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ServerName { get; set; }
        public bool Active { get; set; }
        public int SQLBackupID { get; set; }
    }
}
