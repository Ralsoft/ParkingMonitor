using ParkingMonitor.Models;
using ParkingMonitor.Service;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ParkingMonitor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region ICommands

        #region Group1

        public ICommand ClickLeave { get; private set; }
        public ICommand ClickEntry { get; private set; }
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



        #endregion

        private MqttService _service;

        public MainWindowViewModel()
        {

            _service = new MqttService();
            var canClickEntryLeave = this
               .WhenAnyValue(
                   x => x.GRZ,
                   (grz) => grz?.Length == 8);


            ClickLeave = ReactiveCommand.Create(async () =>
            {
                string result = await HttpService.sendGRZ(GRZ, "2");
            }, canClickEntryLeave);

            ClickEntry = ReactiveCommand.Create( async () =>
            {
                string result = await HttpService.sendGRZ(GRZ, "1");
            }, canClickEntryLeave);

            ClickSendTextMonitor = ReactiveCommand.Create(async () =>
            {
                if(EditMonitorText?.Length > 0)
                {
                    
                    await _service.SendTextOnMonitor(new List<Message>()
                    {
                        new Message()
                        {
                            Color = 0x00,
                            X = 0x00,
                            Y = 0x00,
                            Text = EditMonitorText
                        }
                    });
                }
            });

            OpenStateDoor = ReactiveCommand.Create(async () =>
            {
                await _service.OpenDoor();
            });

            WarningStateDoor = ReactiveCommand.Create(async () =>
            {
                await _service.WarningDoor();
            });
        }
    }
}
