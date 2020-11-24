using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usb.Net.Windows;
using Hid.Net.Windows;
using Device.Net;
using UtilsLibrary;

namespace UsbLibrary
{
    public class Enumerator
    {

        public List<Device> Devices { get; set; } = new List<Device> { };
        public async Task Run(Target target)
        {
            Devices = new List<Device> { };
            var logger = Logger.Instance();


            if (target == Target.stm32)
            {
                var deviceFactory = new FilterDeviceDefinition(0x0483, 0xDF11) // STM32 Bootloader
                .CreateWindowsUsbDeviceFactory(classGuid: WindowsDeviceConstants.GUID_DEVINTERFACE_USB_DEVICE);
                var devices = await deviceFactory.GetConnectedDeviceDefinitionsAsync();
                var device = devices.FirstOrDefault();

                if (device != null)
                {
                    Devices.Add(new Device(device));
                }
            }

            if (target == Target.teensy)
            {
                var deviceFactory = new FilterDeviceDefinition(0x16C0, 0x0478) // Teensy Bootloader
                .CreateWindowsHidDeviceFactory(classGuid: WindowsDeviceConstants.GUID_DEVINTERFACE_HID);
                var devices = await deviceFactory.GetConnectedDeviceDefinitionsAsync();
                var device = devices.FirstOrDefault();

                if (device != null)
                {
                    Devices.Add(new Device(device));
                }
            }

            if (target == Target.all)
            {
                var hidGuid = WindowsDeviceConstants.GUID_DEVINTERFACE_HID;
                var usbGuid = WindowsDeviceConstants.GUID_DEVINTERFACE_USB_DEVICE;
                var deviceFactories = new List<IDeviceFactory>
                {
                    new FilterDeviceDefinition(0x16C0, 0x0478)              // Teensy Bootloader
                        .CreateWindowsHidDeviceFactory(classGuid: hidGuid),
                    new FilterDeviceDefinition(0x0483, 0xDF11)              // STM32 Bootloader
                        .CreateWindowsUsbDeviceFactory(classGuid: usbGuid),
                    new FilterDeviceDefinition(0xFEED, 0x1307)              // Legacy Ids Ergodox EZ
                        .CreateWindowsHidDeviceFactory(classGuid: hidGuid),
                    new FilterDeviceDefinition(0xFEED, 0x6060)              // Legacy Ids Planck EZ
                        .CreateWindowsHidDeviceFactory(classGuid: hidGuid),
                    new FilterDeviceDefinition(0x3297, 0x1969)              // Moonlander MK1
                        .CreateWindowsHidDeviceFactory(classGuid: hidGuid),
                    new FilterDeviceDefinition(0x3297, 0x4974)              // Ergodox EZ Standard
                        .CreateWindowsHidDeviceFactory(classGuid: hidGuid),
                    new FilterDeviceDefinition(0x3297, 0x4975)              // Ergodox EZ Shine
                        .CreateWindowsHidDeviceFactory(classGuid: hidGuid),
                    new FilterDeviceDefinition(0x3297, 0x4976)              // Ergodox EZ Glow
                        .CreateWindowsHidDeviceFactory(classGuid: hidGuid),
                    new FilterDeviceDefinition(0x3297, 0xC6CE)              // Planck EZ Standard
                        .CreateWindowsHidDeviceFactory(classGuid: hidGuid),
                    new FilterDeviceDefinition(0x3297, 0xC6CF)              // Planck EZ Glow
                        .CreateWindowsHidDeviceFactory(classGuid: hidGuid),
                };

                var deviceManager = new DeviceManager(deviceFactories, null);
                var devices = await deviceManager.GetConnectedDeviceDefinitionsAsync();
                // The device manager returns an entry for each endpoints
                // as a result duplicates are removed by checking against
                // the pid / vid of previously enumerated devices.
                foreach (var device in devices)
                {
                    var match = Devices.FindIndex((_dev) => _dev.Info.VendorId == device.VendorId && _dev.Info.ProductId == device.ProductId);
                    if (match == -1)
                    {
                        Devices.Add(new Device(device));
                    }
                }
            }
        }
    }
}