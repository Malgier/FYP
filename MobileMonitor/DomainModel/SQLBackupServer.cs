using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class SQLBackupServer
    {
        public int SQLBackupID { get; set; }
        [DisplayName("SQL Server Name")]
        public string DatabaseName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        [DisplayName("Assigned Server")]
        public int Server_ServerID { get; set; }

        public virtual ICollection<ServerBackup> ServerBackups { get; set; }
        public virtual Server Server { get; set; }
    }
}
