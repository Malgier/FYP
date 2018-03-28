namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class ServerWarning
    {
        public int WarningID { get; set; }
        public string WarningCause { get; set; }
        public DateTime TimeWarningStart { get; set; }
        public DateTime TimeWarningEnd { get; set; }
        public bool Active { get; set; }
        [DisplayName("Server Name")]
        public int Server_ServerID { get; set; }
    
        public virtual Server Server { get; set; }
    }
}
