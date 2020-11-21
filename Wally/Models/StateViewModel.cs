using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using PropertyChanged;
using System.ComponentModel;
using UsbLibrary;
using UtilsLibrary;
using System.Windows.Input;
using System.Windows;
using System.Linq;

namespace Wally.Models
{
    [AddINotifyPropertyChangedInterface]
    public class StateViewModel : INotifyPropertyChanged
    {

        private Firmware _firmware;
        public FlashingStep Step { get; set; }

        public IList<Device> ConnectedDevices { get; set; }

        public int FlashPercentage { get; set; }
        public string FlashMessage { get; set; }

        public Device SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                _selectedDevice = value;
                if (value != null)
                    Step = FlashingStep.SelectFirmware;
            }
        }

        public Target Target
        {
            get
            {
                if (SelectedDevice == null) return Target.all;
                return SelectedDevice.Target;
            }
        }

        public string FileExtension
        {
            get
            {
                if (Target == Target.stm32) return ".bin";
                if (Target == Target.teensy) return ".hex";
                return String.Empty;
            }

        }

        private Device _selectedDevice;

        public string StatusLabel
        {
            get
            {
                if (Step == FlashingStep.SearchKeyboard)
                {
                    return "LOOKING...";
                }
                if (Step == FlashingStep.SelectKeyboard)
                {
                    return "-SELECT-";
                }
                if (SelectedDevice != null)
                {
                    return SelectedDevice.FriendlyName;
                }
                return String.Empty;
            }
        }
        public int ActivePillIndex
        {
            get
            {
                return Step switch
                {
                    FlashingStep.SearchKeyboard or FlashingStep.SelectKeyboard => 0,
                    FlashingStep.SelectFirmware or FlashingStep.SearchBootloader => 1,
                    FlashingStep.Flash => 2,
                    FlashingStep.Complete => 3,
                    FlashingStep.Error => 3,
                    _ => 3
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task Start()
        {
                ConnectedDevices = new List<Device>();
                FlashMessage = String.Empty;
                FlashPercentage = 0;
                SelectedDevice = null;
            Step = FlashingStep.SearchKeyboard;
            await Enumerate();
            // if there's only one device, skip the Keyboard selection step.
            if (ConnectedDevices.Count > 1)
            {
                Step = FlashingStep.SelectKeyboard;
            }
            else
            {
                SelectedDevice = ConnectedDevices[0];
                Step = FlashingStep.SelectFirmware;
            }
        }
        public StateViewModel()
        {
            Task.Run(async () =>
            {
                await Start();
            });
        }
        public void SelectKeyboard()
        {
            Step = FlashingStep.SelectFirmware;
        }

        public void SelectFirmare(string filePath)
        {
            _firmware = new Firmware(filePath);
            Step = FlashingStep.SearchBootloader;
            Flash();
        }

        private void Flash()
        {
            Task.Run(async () =>
            {
                var enumerator = new Enumerator();

                while (enumerator.Devices.Count != 1)
                {
                    await Task.Delay(10);
                    await enumerator.Run(_firmware.Target);
                }
                var device = enumerator.Devices.First();
                Step = FlashingStep.Flash;
                await device.Init();
                await device.Flash(_firmware, (int percentage, string message) =>
                {
                    FlashPercentage = percentage;
                    FlashMessage = message;
                });
                Step = FlashingStep.Complete;
            });
        }
        private async Task Enumerate()
        {
            var enumerator = new Enumerator();
            while (enumerator.Devices.Count == 0)
            {
                await enumerator.Run(Target);
                await Task.Delay(10);
            }

            enumerator.Devices.ForEach(_dev => ConnectedDevices.Add(new Device((int)_dev.Info.ProductId, _dev.FriendlyName, _dev.Target)));
        }
    }
}
