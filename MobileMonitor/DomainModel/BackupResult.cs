using System;
using System.ComponentModel;

namespace DomainModel
{
    
    public partial class BackupResult
    {
        public int ResultID { get; set; }
        public DateTime DatePerformed { get; set; }
        public string Result { get; set; }
        public string Error { get; set; }
        public bool Active { get; set; }
        [DisplayName("Server Backup")]
        public int ServerBackup_BackupID { get; set; }
    
        public virtual ServerBackup ServerBackup { get; set; }
    }
}
