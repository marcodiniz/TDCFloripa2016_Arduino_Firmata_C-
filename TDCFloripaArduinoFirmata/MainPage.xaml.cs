using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Maker.Firmata;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TDCFloripaArduinoFirmata
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public RemoteDevice Arduino { get; set; }
        public UsbSerial USB { get; set; }

        private void btnConectar_Click(object sender, RoutedEventArgs e)
        {
            USB = new UsbSerial("1A86", "7523");
            USB.ConnectionEstablished += () => EscreveStatus("Conexão OK!");

            Arduino = new RemoteDevice(USB);
            Arduino.DeviceReady += () => EscreveStatus("PRONTO!");

            USB.begin(57600, SerialConfig.SERIAL_8N1);
        }

        private async void btnPiscaLed_Click(object sender, RoutedEventArgs e)
        {
            Arduino.pinMode(6, PinMode.OUTPUT);
            Arduino.digitalWrite(6, PinState.HIGH);
            await Task.Delay(2000);
            Arduino.digitalWrite(6, PinState.LOW);
        }

        private async void btnLigarSensor_Click(object sender, RoutedEventArgs e)
        {
            Arduino.pinMode("A3", PinMode.ANALOG);
            Arduino.pinMode(3, PinMode.SERVO);

            while (true)
            {
                var valor = Arduino.analogRead("A3");
                var angulo = ((decimal)valor / 1024) * 180;

                Arduino.analogWrite(3, (ushort)angulo);
                await Task.Delay(100);
            }
        }

        private async void EscreveStatus(string msg)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => status.Text = msg);
        }
        
    }
}
