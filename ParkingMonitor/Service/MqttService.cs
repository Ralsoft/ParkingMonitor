using ParkingMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ParkingMonitor.Service
{
    public class MqttService
    {
        private MqttClient _client;

        public MqttService()
        {
            _client = new MqttClient("194.87.237.67");
            _client.Connect("ParkingMonitor");
        }

        public async Task SendMessage(string topic, byte[] message)
        {
            await Task.Run(() =>
            {
                _client.Publish(topic, message);
            });
        }

        public async Task SendTextOnMonitor(List<Message> messages)
        {
            await Task.Run(() =>
            {
                var monitor = new Monitor()
                {
                    IP = "192.168.8.110",
                    Port = 1985,
                    CumNumber = "2",
                    Messages = messages
                };
                var json = JsonSerializer.Serialize(monitor);

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var WIN1251 = Encoding.GetEncoding("windows-1251");
                byte[] jsonBytes = WIN1251.GetBytes(json);
                SendMessage("Parking/MonitorDoor/Monitor/View", jsonBytes);
            });
        }

        public async Task OpenDoor()
        {
            await Task.Run(() =>
            {
                var door = new Door()
                {
                    IP = "192.168.8.110",
                    Port = 1985
                };
                var json = JsonSerializer.Serialize(door);

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var WIN1251 = Encoding.GetEncoding("windows-1251");
                byte[] jsonBytes = WIN1251.GetBytes(json);
                SendMessage("Parking/MonitorDoor/Door/Open", jsonBytes);
            });
        }

        public async Task WarningDoor()
        {
            await Task.Run(() =>
            {
                var door = new Door()
                {
                    IP = "localhost",
                    Port = 1234
                };
                var json = JsonSerializer.Serialize(door);

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var WIN1251 = Encoding.GetEncoding("windows-1251");
                byte[] jsonBytes = WIN1251.GetBytes(json);
                SendMessage("Parking/MonitorDoor/Door/Warning", jsonBytes);
            });
        }
    }
}
