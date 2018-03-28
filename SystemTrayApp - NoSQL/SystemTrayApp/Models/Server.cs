using System;

namespace SystemTrayApp.Models
{

    public partial class Server
    {
        public int ServerID { get; set; }
        public string ServerName { get; set; }
        public DateTime DateAdded { get; set; }
        public string ServerUniqueCode { get; set; }
        public int CPUWarningPoint { get; set; }
        public int NetworkWarningPoint { get; set; }
        public int RAMWarningPoint { get; set; }
        public int TimeWarning { get; set; }
        public bool Active { get; set; }
    }

}
