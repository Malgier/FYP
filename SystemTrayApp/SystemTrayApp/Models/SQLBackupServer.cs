using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTrayApp.Models
{
    public class SQLBackupServer
    {
        public int SQLBackupID { get; set; }
        public string DatabaseName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public int Server_ServerID { get; set; }
    }
}
