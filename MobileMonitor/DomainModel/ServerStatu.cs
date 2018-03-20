namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServerStatu
    {
        public int StatusID { get; set; }
        public string CPUUsage { get; set; }
        public string MemoryAvailble { get; set; }
        public string NetworkUsage { get; set; }
        public DateTime DateOfStatus { get; set; }
        public bool Active { get; set; }
        public int Server_ServerID { get; set; }
    
        public virtual Server Server { get; set; }
    }
}
