using System.Collections.Generic;

namespace ParkingMonitor.Models
{
    public class Monitor
    {
        public int CamNumber { get; set; }
        public List<Message> Messages { get; set; }
    }
}
