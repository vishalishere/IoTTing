using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Threading;

namespace PIDsensor1
{
    public class IoTHelper
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "IoTLeg.azure-devices.net";
        static string deviceKey = "3tvfXCb0sWaH6uXle1evgTuDd7PRmg2culVxVzFdH/s=";


        public static async void SendReading(DateTime triggerTime)
        {
            deviceClient = DeviceClient.CreateFromConnectionString("HostName=IoTLeg.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=dmg/tdAmz6r13qhToNy6AptULWvbtXeO0aZY2D2qoC4=","alarm");
            var telemetryDataPoint = new
            {
                deviceId = "alarm",
                triggerTime = triggerTime
            };

            var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            await deviceClient.SendEventAsync(message);
        }
    }
}
