﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyChanged;
using System.ComponentModel;
using UsbLibrary;
using UtilsLibrary;
using System.Linq;

namespace Wally.Models
{
    [AddINotifyPropertyChangedInterface]
    public class StateViewModel : INotifyPropertyChanged
    {

        private Firmware _firmware;
        private Device _selectedDevice;
        private void Flash()
        {
            Logger.Log(LogSeverity.Info, $"Starting flash process, targeting {Target} device(s).");
            Task.Run(async () =>
            {
                try
                {
                    var enumerator = new Enumerator();

                    while (enumerator.Devices.Count != 1)
                    {
                        await Task.Delay(10);
                        await enumerator.Run(_firmware.Target);
                    }
                    Logger.Log(LogSeverity.Info, $"Target {Target} found, the keyboard is in reset mode.");
                    var device = enumerator.Devices.First();
                    Step = FlashingStep.Flash;
                    await device.Init();
                    await device.Flash(_firmware, (int percentage, string message) =>
                    {
                        FlashPercentage = percentage;
                        FlashMessage = message;
                        if (percentage == 0)
                        {
                            Logger.Log(LogSeverity.Info, $"Flashing: {message}.");
                        }
                        else
                        {
                            Logger.Log(LogSeverity.Info, $"Flashing: {percentage}% complete.");
                        }
                    });
                    Step = FlashingStep.Complete;
                }
                catch (Exception e)
                {
                    Logger.Log(LogSeverity.Error, e.Message);
                    Step = FlashingStep.Error;
                }
            });
        }
        private async Task Enumerate()
        {
            try
            {

                var enumerator = new Enumerator();
                Logger.Log(LogSeverity.Info, $"Starting enumeration, targeting any compatible devices.");
                while (enumerator.Devices.Count == 0)
                {
                    await enumerator.Run(Target);
                    await Task.Delay(10);
                }

                Logger.Log(LogSeverity.Info, $"Enumeration complete for target, found {enumerator.Devices.Count} devices.");
                var i = 1;
                enumerator.Devices.ForEach(_dev =>
                {
                    ConnectedDevices.Add(new Device((int)_dev.Info.ProductId, _dev.FriendlyName, _dev.Target));
                    Logger.Log(LogSeverity.Info, $"{i} - {_dev.FriendlyName} | {_dev.Info.DeviceId} | {_dev.Target}.");
                    i++;
                });
            }
            catch (Exception e)
            {
                Logger.Log(LogSeverity.Error, e.Message);
                Step = FlashingStep.Error;
            }
        }
        private FlashingStep _previousStep;

        public Logger Logger { get; set; }

        public string AppVersion { get; set; }
        public string StatusBarVersion
        {
            get
            {
                return $"v{AppVersion}";
            }
        }

        public bool CopiedToClipboard { get; set; } = false;
        public FlashingStep Step { get; set; }

        public IList<Device> ConnectedDevices { get; set; }

        public int FlashPercentage { get; set; }
        public string FlashMessage { get; set; }

        public void CopyToClipboard()
        {
            LogsToClipboard.Run();
            CopiedToClipboard = true;
            Task.Run(async () =>
           {
               await Task.Delay(3000);
               CopiedToClipboard = false;
           });
        }
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
                {
                    Step = FlashingStep.SelectFirmware;
                    Logger.Log(LogSeverity.Info, $"Device selected as a flashing target: {value.FriendlyName} | {value.Target}");
                }
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

        public void ToggleLog()
        {
            if (Step != FlashingStep.DisplayLogs)
            {
                _previousStep = Step;
                Step = FlashingStep.DisplayLogs;
            }
            else
            {
                Step = _previousStep;
            }
        }
        public async Task Start()
        {
            // The reset button was clicked
            if (Step != FlashingStep.SearchKeyboard)
            {
                Logger.Log(LogSeverity.Info, $"Restarting the flash process.");
            }
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
        public StateViewModel(string appVersion, string filePath)
        {
            AppVersion = appVersion;
            Logger = Logger.Instance();
            Logger.Log(LogSeverity.Info, "Application started");
            if (filePath == String.Empty)
            {
                Task.Run(async () =>
                {
                    await Start();
                });
            }
            else
            {
                SelectFirmare(filePath);
            }
        }
        public void SelectKeyboard()
        {
            Step = FlashingStep.SelectFirmware;
        }

        public void SelectFirmare(string filePath)
        {
            try
            {
                Logger.Log(LogSeverity.Info, $"Selected firmware, path: {filePath}");
                _firmware = new Firmware(filePath);

                // If the file was provided as an argument, the keyboard step was skipped, as
                // a result we need to create one that has the same target as the firmware file
                if (SelectedDevice == null)
                {
                    SelectedDevice = new Device(0xDF11, "Keyboard in reset mode", _firmware.Target);
                }

                // A firmware could mismatch the target device
                if (SelectedDevice.Target != _firmware.Target)
                {
                    Logger.Log(LogSeverity.Error, "The firmware you supplied is not compatible with the keyboard you selected.");
                    Step = FlashingStep.Error;
                }
                else
                {
                    Step = FlashingStep.SearchBootloader;
                    Logger.Log(LogSeverity.Info, $"Firmware file is valid for {_firmware.Target}.");
                    Flash();
                }
            }
            catch (Exception e)
            {
                Logger.Log(LogSeverity.Error, e.Message);
                Step = FlashingStep.Error;
            }
        }

    }
}
