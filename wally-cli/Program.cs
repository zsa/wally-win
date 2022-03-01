using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Usb.Net;
using Usb.Net.Windows;
using Device.Net;
using System.Security.Claims;
using UtilsLibrary;
using UsbLibrary;
using Kurukuru;

namespace CLI
{
    class Program
    {
        static public UsbDevice Handle { get; set; }
        static async Task Main(string[] args)
        {

            if (args.Length != 1)
            {
                PrintUsage();
            }
            var firmware = new Firmware(args[0]);

            if (firmware.Opened == false)
            {
                Environment.Exit(-1);
            }

            if (firmware.Target == Target.unknown) 
            {
                PrintWrongFileType();
            }

            var enumerator = new Enumerator();

            await Spinner.StartAsync("Press the reset button of your keyboard.", async spinner => {
                while (enumerator.Devices.Count != 1)
                {
                    await Task.Delay(500);
                    await enumerator.Run(firmware.Target);
                }
                var device = enumerator.Devices.First();
                await device.Init();
                await device.Flash(firmware, (int percentage, string message) => {
                    if (percentage == 0)
                    {
                        spinner.Text = message;
                    }
                    else
                    {
                        spinner.Text = $"{message} - {percentage}% Complete";
                    }
                });
                spinner.Text = "Keyboard flashed, enjoy your new firmware.";
            });
        }
        static void PrintUsage()
        {
            Console.WriteLine("Usage: wally-cli <firmware-file>");
            Environment.Exit(-1);
        }
        static void PrintWrongFileType()
        {
            Console.WriteLine("Error: specify a .bin file (Moonlander/Planck EZ) or a .hex file (Ergodox EZ)");
            Environment.Exit(-1);
        }
    }
}
