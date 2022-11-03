using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingMonitor.Models
{
    public class MQTTNode : INotifyPropertyChanged
    {

        private ObservableCollection<MQTTNode> _mqttNodes;
        
        public ObservableCollection<MQTTNode> MQTTNodes
        {
            get { return _mqttNodes; }
            set
            {
            _mqttNodes = value;
                OnPropertyChanged("MQTTNodes");
            }
        }

        private string _body;
        public string Body
        {
            get { return _body; }
            set
            {
                _body = value;
                OnPropertyChanged("Body");
            }
        }
        
        
        private string _topic;
        public string Topic
        {
            get { return _topic; }
            set
            {
                _topic = value;
                OnPropertyChanged("Topic");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
