using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ParkingMonitor.Interfaces;
using ParkingMonitor.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ParkingMonitor.Service
{
    public class MqttService
    {
        private MqttClient _client;

        public MqttService(IMQTTPublishReceived iMQTTPublishReceived)
        {
            _client = new MqttClient("194.87.237.67");
            _client.Connect("ParkingMonitor");
            _client.Subscribe(new string[] { "Parking/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            _client.MqttMsgPublishReceived += (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Message);
                var topic = e.Topic;
                var titleNodes = topic.Split('/');
                
                var stack = new Queue<string>();

                foreach(var item in titleNodes)
                {
                    stack.Enqueue(item);
                }

                var node = new MQTTNode()
                {
                    Topic = stack.Dequeue(),
                    MQTTNodes = new List<MQTTNode>
                    {
                        addNodeToNode(stack, message)
                    }
                };

                iMQTTPublishReceived.AddNodeToTree(node);
               
            };
        }

        public MQTTNode addNodeToNode(Queue<string> stack, string body)
        {
            var node = new MQTTNode()
            {
                Topic = stack.Dequeue(),
            };

            if (stack.Count > 0)
                node.MQTTNodes = new List<MQTTNode>
                {
                    addNodeToNode(stack, body),
                };
            else
                node.Body = body;
            
            
            return node;
        }



        public async Task SendMessage(string topic, byte[] message)
        {
            await Task.Run(() =>
            {
                _client.Publish(topic, message);
            });
        }

        public async Task SendTextOnMonitor(List<Message> messages, int camNumber)
        {
            await Task.Run(() =>
            {
                var monitor = new Monitor()
                {
                    CamNumber = camNumber,
                    Messages = messages
                };
                var json = JsonConvert.SerializeObject(monitor, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
                SendMessage("Parking/MonitorDoor/Monitor/View/", jsonBytes);
            });
        }

        public async Task OpenDoor(int camNumber, string doorNumber)
        {
            await Task.Run(() =>
            {
                var door = new Door()
                {
                    CameraNumber = camNumber
                };
                var json = JsonConvert.SerializeObject(door, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

                SendMessage($"Parking/MonitorDoor/Door/Open/{doorNumber}/", jsonBytes);
            });
        }

        public async Task WarningDoor(int camNumber)
        {
            await Task.Run(() =>
            {
                var door = new Door()
                {
                    CameraNumber = camNumber
                };
         
                var json = JsonConvert.SerializeObject(door, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
                SendMessage("Parking/MonitorDoor/Door/Warning/", jsonBytes);
            });
        }
    }
}
