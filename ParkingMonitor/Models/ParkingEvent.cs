using System;

namespace ParkingMonitor.Models
{
    public class ParkingEvent
    {
        public DateTime DateTime { get; set; }
        public string Place { get; set; }
        public string GRZ { get; set; }
        public string EventType { get; set; }
    }
}
