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
        //static string connectionString = "HostName=IoTLeg.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=dmg/tdAmz6r13qhToNy6AptULWvbtXeO0aZY2D2qoC4=";
        //static string iotHubD2cEndpoint = "messages/events";//"iothub-ehub-iotleg-988-e2490900c4";//"messages/events";
        //static EventHubClient eventHubClient;

        //static string EventHubEndpoint = "sb://ihsuproddbres001dednamespace.servicebus.windows.net/";

        //static DeviceClient devClient;

        //private async static Task ReceiveMessagesFromDeviceAsync(string partition)
        //{
        //    var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.Now);
        //    while (true)
        //    {
        //        EventData eventData = await eventHubReceiver.ReceiveAsync();
        //        if (eventData == null) continue;

        //        string data = Encoding.UTF8.GetString(eventData.GetBytes());
        //        Console.WriteLine(string.Format("Message received. Partition: {0} Data: '{1}'", partition, data));
        //    }
        //}

        //async static void ReceivedFromAzure()
        //{
        //    devClient.CreateFromConnectionString(connectionString, TransportType.Http1);
        //    Message msg;
        //    string triggerTime;
        //    msg = devClient.ReceiveAsync();
        //    if (msg != null)
        //    {
        //        triggerTime = Encoding.ASCII.GetString(msg.GetBytes());
        //        await devClient.CompleteAsync(msg);
        //        Console.WriteLine("Triggered at: {0}", triggerTime);
        //    }
        //}


        //static void ReadFromEventHub()
        //{
        //    string eventHubConnectionString = "{event hub connection string}";
        //    string eventHubName = "iothub-ehub-iotleg-988-e2490900c4";
        //    string storageAccountName = "{storage account name}";
        //    string storageAccountKey = "{storage account key}";
        //    string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
        //        storageAccountName, storageAccountKey);

        //    string eventProcessorHostName = Guid.NewGuid().ToString();
        //    EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, eventHubName, EventHubConsumerGroup.DefaultGroupName, eventHubConnectionString, storageConnectionString);
        //    Console.WriteLine("Registering EventProcessor...");
        //    eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>().Wait();

        //    Console.WriteLine("Receiving. Press enter key to stop worker.");
        //    Console.ReadLine();
        //    eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        //}
        static void Main(string[] args)
        {
            Console.WriteLine("Receive messages\n");

            //eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, iotHubD2cEndpoint);

            //var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

            //foreach (string partition in d2cPartitions)
            //{
            //    ReceiveMessagesFromDeviceAsync(partition);
            //}
            //while(true)
            //{
            //    ReceivedFromAzure();
            //}
            Console.ReadLine();
        }
    }
}
