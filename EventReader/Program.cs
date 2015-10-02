using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Azure.Devices.Common;
using Microsoft.ServiceBus.Messaging;

namespace EventReader
{
    class Program
    {
        static string connectionString = "HostName=IoTLeg.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=dmg/tdAmz6r13qhToNy6AptULWvbtXeO0aZY2D2qoC4=";
        static string iotHubD2cEndpoint = "messages/events";//"iothub-ehub-iotleg-988-e2490900c4";//"messages/events";
        static EventHubClient eventHubClient;

        private async static Task ReceiveMessagesFromDeviceAsync(string partition)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.Now);
            while (true)
            {
                EventData eventData = await eventHubReceiver.ReceiveAsync();
                if (eventData == null) continue;

                string data = Encoding.UTF8.GetString(eventData.GetBytes());
                Console.WriteLine(string.Format("Message received. Partition: {0} Data: '{1}'", partition, data));
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Receive messages\n");
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, iotHubD2cEndpoint);

            var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

            foreach (string partition in d2cPartitions)
            {
                ReceiveMessagesFromDeviceAsync(partition);
            }
            Console.ReadLine();
        }
    }
}
