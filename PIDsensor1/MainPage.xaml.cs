using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PIDsensor1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        GpioController gpio = GpioController.GetDefault();
        GpioPin pirPin;
        GpioPin abortPin;
        GpioPin[] inputs = new GpioPin[4];
        GpioPin[] outputs = new GpioPin[4];

        GpioPin ledPin;

        DispatcherTimer timer = new DispatcherTimer();

        Dictionary<int, char> keypadData = new Dictionary<int, char>();

        string[] messages = new string[] {"I see you", "I know you are there", "Come out where ever you are", "I am ready now, come and play" };

        bool missonAborted = false;

        public MainPage()
        {
            this.InitializeComponent();
            InitPirsensorPin();
            InitKeypadPins();
            InitLedPin();
            InitAbortButton();
            if (gpio == null)
                PirStatus.Text = "GPIO is not supported";
        }

        private void InitAbortButton()
        {
            if (gpio == null)
                return;
            abortPin = gpio.OpenPin(13);
            if (abortPin.IsDriveModeSupported(GpioPinDriveMode.InputPullDown))
                abortPin.SetDriveMode(GpioPinDriveMode.InputPullDown);
            else
                abortPin.SetDriveMode(GpioPinDriveMode.Input);
            abortPin.DebounceTimeout = TimeSpan.FromMilliseconds(100);
            abortPin.ValueChanged += AbortPin_ValueChanged;
        }

        private void AbortPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                if (args.Edge == GpioPinEdge.RisingEdge)
                    missonAborted = missonAborted ? false : true;
                AbortText.Text = (missonAborted ? "ABORT MISSION!" : "");
            });
        }

        private void InitLedPin()
        {
            if (gpio == null)
                return;
            ledPin = gpio.OpenPin(6);
            ledPin.Write(GpioPinValue.High);
            ledPin.SetDriveMode(GpioPinDriveMode.Output);
        }

        private void InitPirsensorPin()
        {
            MediaElement mediaElement = new MediaElement();
            SpeechSynthesizer speech = new SpeechSynthesizer();
            if (gpio == null)
                return;
            pirPin = gpio.OpenPin(5);
            if (pirPin.IsDriveModeSupported(GpioPinDriveMode.InputPullDown))
                pirPin.SetDriveMode(GpioPinDriveMode.InputPullDown);
            else
                pirPin.SetDriveMode(GpioPinDriveMode.Input);
            pirPin.SetDriveMode(GpioPinDriveMode.Input);
            pirPin.ValueChanged += (GpioPin p, GpioPinValueChangedEventArgs args) =>
            {
                if (args.Edge == GpioPinEdge.RisingEdge)
                {
                    var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => {
                        IoTHelper.SendReading(DateTime.Now);
                        var msg = messages.OrderBy(x => Guid.NewGuid()).Take(1).First();
                        PirStatus.Text = msg;
                        SpeechSynthesisStream stream = await speech.SynthesizeTextToStreamAsync(msg);
                        mediaElement.SetSource(stream, stream.ContentType);
                        mediaElement.Play();
                    });
                }
            };
        }

        private void InitKeypadPins()
        {
            if (gpio == null)
                return;
            //4 inputs
            inputs[0] = gpio.OpenPin(27);
            inputs[1] = gpio.OpenPin(22);
            inputs[2] = gpio.OpenPin(18);
            inputs[3] = gpio.OpenPin(23);
            //4 outputs
            outputs[0] = gpio.OpenPin(24);
            outputs[1] = gpio.OpenPin(25);
            outputs[2] = gpio.OpenPin(12);
            outputs[3] = gpio.OpenPin(16);

            foreach (var pin in inputs)
            {
                if (pin.IsDriveModeSupported(GpioPinDriveMode.InputPullDown))
                {
                    pin.SetDriveMode(GpioPinDriveMode.InputPullDown);
                }
                else
                {
                    pin.SetDriveMode(GpioPinDriveMode.Input);
                }
            }

            foreach (var pin in outputs)
            {
                pin.SetDriveMode(GpioPinDriveMode.Output);
                pin.Write(GpioPinValue.Low);
            }

            keypadData.Add(0, 'D');
            keypadData.Add(1, '#');
            keypadData.Add(2, '0');
            keypadData.Add(3, '*');
            keypadData.Add(4, 'C');
            keypadData.Add(5, '9');
            keypadData.Add(6, '8');
            keypadData.Add(7, '7');
            keypadData.Add(8, 'B');
            keypadData.Add(9, '6');
            keypadData.Add(10, '5');
            keypadData.Add(11, '4');
            keypadData.Add(12, 'A');
            keypadData.Add(13, '3');
            keypadData.Add(14, '2');
            keypadData.Add(15, '1');

            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(200);
            this.timer.Tick += Timer_Tick;
            this.timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            var key = ReadKey();
            if (key != null)
                KeypadKey.Text += key.ToString();
            if (key == '*')
                ledPin.Write(GpioPinValue.High);
            if (key == '#')
                ledPin.Write(GpioPinValue.Low);
        }

        private char? ReadKey()
        {
            for (int i = 0; i < 4; i++)
            {
                foreach (var pin in outputs)
                {
                    pin.Write(GpioPinValue.Low);
                }
                outputs[i].Write(GpioPinValue.High);
                for(int j = 0; j < 4; j++)
                {
                    if (inputs[j].Read() == GpioPinValue.High)
                    {
                        return keypadData[i * 4 + j];
                    }
                }
            }
            return null;
        }
    }
}
