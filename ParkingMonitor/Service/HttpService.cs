using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ParkingMonitor.Service
{
    public class HttpService
    {
        private const string _port = "8080";
        private const string _host = "192.168.8.104";

        public enum Method
        {
            POST,
            GET,
            DELETE,
            PUT
        }

        public static string GenerateURI()
        {
            return $"http://{_host}:{_port}";
        }


        public async static Task<string> sendRequest(
            string uri, string body, 
            string method, Dictionary<string, string> parameters)
        {
            await Task.Run(async () =>
            {
                try
                {
                    uri += "?";
                    foreach (var item in parameters)
                        uri += $"{item.Key}={item.Value}&";

                    var httpWebRequest = WebRequest.Create(uri);
                    httpWebRequest.ContentType = "application/json; charset=utf-8";
                    httpWebRequest.Method = method;

                    if (body.Length > 0)
                    {
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(body);
                            streamWriter.Flush();
                        }
                    }

                    var httpResponse = await httpWebRequest.GetResponseAsync();

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        return result;
                    }
                }
                catch (Exception ee)
                {
                    return ee.ToString();
                }
            });
            return null;
        }



        public async static Task<string> sendGRZ(string GRZ, string camNumber)
        {
            string url = $"{GenerateURI()}/send";
            return await sendRequest(url, "", "GET", new Dictionary<string, string>{
                    {"topic", "Parking/IntegratorCVS"},
                    {"payload", GRZ},
                    {"camNumber", camNumber}
                });
        }
    }
}
