using Device.Net;
using System;
using System.Threading.Tasks;
using Usb.Net;
using Usb.Net.Windows;
using Hid.Net.Windows;
using UtilsLibrary;

namespace UsbLibrary
{
    public class Device
    {
        #region Public props
        public Target Target { 
            get {
                return Info.ProductId switch
                {
                    0x0478 or
                    0x1307 or
                    0x4974 or
                    0x4975 or
                    0x4976 => Target.teensy,
                    _ => Target.stm32,
                };
            }
        }
        public string FriendlyName { 
            get {
                    return Info.ProductId switch
                    {
                        0x1307 => "Ergodox EZ",
                        0x4974 => "Ergodox EZ Original",
                        0x4975 => "Ergodox EZ Shine",
                        0x4976 => "Ergodox EZ Glow",
                        0x6060 => "Planck EZ",
                        0xC6CE => "Planck EZ Standard",
                        0xC6CF => "Planck EZ Glow",
                        0x1969 => "Moonlander MK1",
                        0x0478 => "Ergodox in Reset Mode",
                        0xDF11 => "Keyboard in Reset Mode",
                        _ => "",
                    };
                }
        }
        public string Status { get; set; } = "Idle";
        public ConnectedDeviceDefinition Info { get; set; }
        public delegate void ProgressCallback(int percentage, string message);
        public long TotalBytes = 1;
        public long TransferredBytes = 0;
        public UsbDevice Handle;
        public WindowsHidDevice HidHandle;
        public int FlashProgress
        {
            get
            {
                return (int)Math.Ceiling((double)TransferredBytes * 100 / TotalBytes);
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor needs a chipset target <see cref="Target"/>
        /// and a usb device definition <see cref="ConnectedDeviceDefinition"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="deviceDefinition"></param>
        public Device(ConnectedDeviceDefinition deviceDefinition)
        {
            Info = deviceDefinition;
        }
        #endregion
        #region Public methods
        /// <summary>
        /// Inits and connect to a usb device
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            if(Info.DeviceType == DeviceType.Hid)
            {
                HidHandle = new WindowsHidDevice(Info.DeviceId, 131, 131);
                await HidHandle.InitializeAsync();
            }
            else
            {
                Handle = new UsbDevice(Info.DeviceId, new WindowsUsbInterfaceManager(Info.DeviceId));
                await Handle.InitializeAsync();
            }
        }
        /// <summary>
        /// Flash a firmware on the connected device,
        /// progress is passed as a callback.
        /// </summary>
        /// <param name="firmware"></param>
        /// <param name="progressCallback"></param>
        /// <returns></returns>
        public async Task Flash(Firmware firmware, ProgressCallback progressCallback)
        {
            progressCallback(0, "Initializing");
            if (Target == Target.stm32)
            {
                var flasher = new DFU(this, firmware);
                await flasher.Run((int percentage, string message) => progressCallback(percentage, message));
            }
            if (Target == Target.teensy)
            {
                var flasher = new Teensy(this, firmware);
                await flasher.Run((int percentage, string message) => progressCallback(percentage, message));
            }
        }
        #endregion
    }
}
