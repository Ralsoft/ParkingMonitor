using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingMonitor.Models
{
    public class Message
    {
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte Color { get; set; }
        public string Text { get; set; }
    }
}
