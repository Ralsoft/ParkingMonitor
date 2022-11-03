using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingMonitor.Models
{
    public class MQTTNode
    {
        public string Topic { get; set; }
        public string Body { get; set; }
        public List<MQTTNode> MQTTNodes { get; set; }
    }
}
