﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
namespace CreateDevIdentity
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "HostName=IoTLeg.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=dmg/tdAmz6r13qhToNy6AptULWvbtXeO0aZY2D2qoC4=";

        private async static Task AddDeviceAsync()
        {
            string deviceId = "alarm";
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddDeviceAsync().Wait();
            Console.ReadLine();
        }
    }
}
