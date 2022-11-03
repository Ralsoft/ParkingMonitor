using Avalonia.Threading;
using ParkingMonitor.Interfaces;
using ParkingMonitor.Models;
using ParkingMonitor.Service;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ParkingMonitor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMQTTPublishReceived
    {

        #region ICommands

        #region Group1

        public ICommand ClickSend { get; private set; }
        #endregion

        #region Group2

        public ICommand ClickSendTextMonitor { get; private set; }

        #endregion 
        
        #region Group4

        public ICommand OpenStateDoor { get; private set; }
        public ICommand WarningStateDoor { get; private set; }

        #endregion

        #endregion

        #region Fields

        private string _grz;
        public string GRZ
        {
            get => _grz;
            set => this.RaiseAndSetIfChanged(ref _grz, value);
        }

        private string _grzCameraNumber;
        public string GRZCameraNumber
        {
            get => _grzCameraNumber;
            set => this.RaiseAndSetIfChanged(ref _grzCameraNumber, value);
        }

        private string _monitorCameraNumber;
        public string MonitorCameraNumber
        {
            get => _monitorCameraNumber;
            set => this.RaiseAndSetIfChanged(ref _monitorCameraNumber, value);
        }

        private string _doorCameraNumber;
        public string DoorCameraNumber
        {
            get => _doorCameraNumber;
            set => this.RaiseAndSetIfChanged(ref _doorCameraNumber, value);
        }

        private string _doorNumber;
        public string DoorNumber
        {
            get => _doorNumber;
            set => this.RaiseAndSetIfChanged(ref _doorNumber, value);
        }

        private string _monitorText;
        public string MonitorText
        {
            get => _monitorText;
            set => this.RaiseAndSetIfChanged(ref _monitorText, value);
        }
        
        private string _editMonitorText;
        public string EditMonitorText
        {
            get => _editMonitorText;
            set => this.RaiseAndSetIfChanged(ref _editMonitorText, value);
        }

        private ObservableCollection<ParkingEvent> _parkingEvents = new ObservableCollection<ParkingEvent>();
        public ObservableCollection<ParkingEvent> ParkingEvents
        {
            get => _parkingEvents;
            set => this.RaiseAndSetIfChanged(ref _parkingEvents, value);
        }

        private ObservableCollection<MQTTNode> _mqttNodes = new ObservableCollection<MQTTNode>();
        public ObservableCollection<MQTTNode> MQTTNodes
        {
            get => _mqttNodes;
            set => this.RaiseAndSetIfChanged(ref _mqttNodes, value);
        }


        #endregion

        private MqttService _service;

        public event Action<DispatcherPriority?> Signaled;

        public MainWindowViewModel()
        {
            _service = new MqttService(this);
           
            var canClickSend = this
               .WhenAnyValue(
                   x => x.GRZ,
                   y => y.GRZCameraNumber,
                   (grz, cm) => grz?.Length == 8 && cm?.Length > 0);

            ClickSend = ReactiveCommand.Create(async () =>
            {
                string result = await HttpService.sendGRZ(GRZ, GRZCameraNumber);
            }, canClickSend);

            ClickSendTextMonitor = ReactiveCommand.Create(async () =>
            {
                if(EditMonitorText?.Length > 0)
                {
                    
                    await _service.SendTextOnMonitor(new List<Message>()
                    {
                        new Message()
                        {
                            Color = 0x01,
                            X = 0x00,
                            Y = 0x00,
                            Text = EditMonitorText
                        }
                    }, Convert.ToInt32(MonitorCameraNumber));
                }
            });

            OpenStateDoor = ReactiveCommand.Create(async () =>
            {
                await _service.OpenDoor(Convert.ToInt32(DoorCameraNumber), DoorNumber);
            });

            WarningStateDoor = ReactiveCommand.Create(async () =>
            {
                await _service.WarningDoor(Convert.ToInt32(DoorCameraNumber));
            });
        }

        public void AddEvent(ParkingEvent @event)
        {
            Dispatcher.UIThread.InvokeAsync(async () => {
                ParkingEvents.Add(@event);
            });
        }

        public void AddNodeToTree(MQTTNode node)
        {
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                Merge(node, MQTTNodes);
            });
        }

        public void Merge(MQTTNode node, ObservableCollection<MQTTNode> nodes)
        {
            var item = new List<MQTTNode>(nodes)
                .FirstOrDefault(x => x.Topic == node.Topic);

            if(node.Topic == item?.Topic)
            {
                if(node.MQTTNodes.Count == 0)
                    item.Body = node.Body;
                else
                {
                    foreach (var i in node.MQTTNodes)
                        Merge(i, item.MQTTNodes);
                }
            }
            else
                nodes.Add(node);
        }
    }
}
