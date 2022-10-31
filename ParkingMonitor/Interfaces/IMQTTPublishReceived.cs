using ParkingMonitor.Models;

namespace ParkingMonitor.Interfaces
{
    public interface IMQTTPublishReceived
    {
        public void AddEvent(ParkingEvent @event);
    }
}
