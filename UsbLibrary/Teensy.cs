using System.Threading.Tasks;
using UtilsLibrary;
using System.Threading;
using Polly;
using System;
using Device.Net.Exceptions;
using System.IO;

namespace UsbLibrary
{
    public class Teensy
    {
        private Device device { get; set; }
        private Firmware firmware { get; set; }

        private static readonly byte HidReportId = 0x02;
        private static readonly int ErgodoxMemSize = 32256;
        private static readonly int ErgodoxBlockSize = 128;

        #region Constructor
        public Teensy(Device _device, Firmware _firmware)
        {
            device = _device;
            firmware = _firmware;
        }
        #endregion

        public delegate void ProgressCallback(int percentage, string message);

        private async Task WriteReportWithRetries(byte[] block, byte reportId)
        {
            var retryPolicy = Policy.Handle<ValidationException>()
                                    .Or<NotInitializedException>()
                                    .Or<IOException>()
                                    .WaitAndRetryAsync(
                                        5,
                                        i => TimeSpan.FromMilliseconds(i * 500)
                                    );
            await retryPolicy.ExecuteAsync(async () => await device.HidHandle.WriteReportAsync(block, reportId));
        }
        private async Task WriteFlash(ProgressCallback progressCallback)
        {
            for(long addr = 0; addr < ErgodoxMemSize; addr += ErgodoxBlockSize)
            {
                byte[] block = new byte[ErgodoxBlockSize + 2];
                block[0] = (byte)(addr & 0XFF);
                block[1] = (byte)((addr >> 8) & 0XFF);
                System.Array.Copy(firmware.Bytes, addr, block, 2, ErgodoxBlockSize);
                await WriteReportWithRetries(block, HidReportId);
                if(addr == 0)
                {
                    progressCallback(0, "Erasing flash");
                    Thread.Sleep(3000);
                }
                else
                {
                    device.TransferredBytes += ErgodoxBlockSize;
                    progressCallback(device.FlashProgress, "Flashing");
                    Thread.Sleep(100);
                }
            }
        }

        private async Task Reboot()
        {
            byte[] buffer = new byte[130];
            buffer[0] = 0xFF;
            buffer[1] = 0xFF;
            Thread.Sleep(1000);
            await WriteReportWithRetries(buffer, HidReportId);
        }
        public async Task Run(ProgressCallback progressCallback)
        {
            device.TotalBytes = ErgodoxMemSize;
            await WriteFlash(progressCallback);
            progressCallback(0, "Restarting keyboard");
            await Reboot();
        }
    }
}
