
namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Server
    {   
        public int ServerID { get; set; }
        public string ServerName { get; set; }
        public DateTime DateAdded { get; set; }
        public string ServerUniqueCode { get; set; }
        [DisplayName("Warning Percentage Usage for CPU")]
        public int CPUWarningPoint { get; set; }
        [DisplayName("Warning Percentage Usage for Network")]
        public int NetworkWarningPoint { get; set; }
        [DisplayName("Warning Avialable Usage for RAM")]
        public int RAMWarningPoint { get; set; }
        [DisplayName("Time to warning in seconds")]
        public int TimeWarning { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<SQLBackupServer> SQLBackupServers { get; set; }
        public virtual ICollection<ServerStatu> ServerStatus { get; set; }
        public virtual ICollection<ServerWarning> ServerWarnings { get; set; }
        public virtual ICollection<UserServer> Users { get; set; }
    }
}
