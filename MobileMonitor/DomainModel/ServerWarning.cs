namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServerWarning
    {
        public int WarningID { get; set; }
        public string WarningCause { get; set; }
        public DateTime TimeWarningStart { get; set; }
        public DateTime TimeWarningEnd { get; set; }
        public bool Active { get; set; }
        public int Server_ServerID { get; set; }
    
        public virtual Server Server { get; set; }
    }
}
