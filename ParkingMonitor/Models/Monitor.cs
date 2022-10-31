using System.Collections.Generic;

namespace ParkingMonitor.Models
{
    public class Monitor
    {
        public int Port { get; set; }
        public string CumNumber { get; set; }
        public string IP { get; set; }
        public List<Message> Messages { get; set; }
    }
}
