namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class ServerBackup
    {   
        public int BackupID { get; set; }
        [Required()]
        [DisplayName("Database Name")]
        public string Name { get; set; }
        [Required()]
        public DateTime ScheduledTime { get; set; }
        [Required()]
        public string Frequency { get; set; }
        [Required()]
        public string BackupType { get; set; }
        [Required()]
        public DateTime ExpirationDate { get; set; }
        public bool Active { get; set; }
        [Required()]
        [DisplayName("SQL Server Name")]
        public int SQLBackupID { get; set; }
    
        public virtual ICollection<BackupResult> BackupResults { get; set; }
        public virtual SQLBackupServer SQLServer { get; set; }
    }
}
