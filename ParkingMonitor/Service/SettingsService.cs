using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace ParkingMonitor.Service
{
    public class SettingsService
    {
        private static string PathToAppsettings = "appsettings.json";
        
        private static string _mqttIP;
        
        public static string getMqttIp()
        {
            setSettings();
            return _mqttIP;
        }

        public static void setSettings()
        {
            string json;
            try
            {
                json = File.ReadAllText(PathToAppsettings);
            }
            catch(FileNotFoundException e)
            {
                var jo = new JObject();
                jo["MqttIP"] = "194.87.237.67";
                json = JsonConvert.SerializeObject(jo, Formatting.Indented);
                File.WriteAllText(PathToAppsettings, json);
            }
         
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            _mqttIP = jsonObj.MqttIP;
        }
    }
}
