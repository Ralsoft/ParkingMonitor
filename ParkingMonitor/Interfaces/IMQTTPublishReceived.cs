using ParkingMonitor.Models;

namespace ParkingMonitor.Interfaces
{
    public interface IMqttPublishReceived
    {
        public void AddEvent(ParkingEvent @event);
        public void AddNodeToTree(MqttNode node);
    }
}
