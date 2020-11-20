using System;
using System.Threading.Tasks;
using UtilsLibrary;
using Usb.Net;
using System.Threading;

namespace UsbLibrary
{
    struct DFUStatus
    {
        public byte bStatus;
        public int bwPollTimeout;
        public byte bState;
    }
    public class DFU

    {
        private Device device { get; set; }
        private Firmware firmware { get; set; }
        private DFUStatus dfuStatus;

        private readonly long START_ADDRESS = 0x08000000;
        private readonly int DFU_SUFFIX_LENGTH = 16;
        private readonly int BLOCK_SIZE = 2048;

        //DFU Commands
        private readonly byte DFU_CMD_SETADDRESS = 0x21;
        private readonly byte DFU_CMD_MASSERASE = 0x41;

        //DFU States
        private readonly byte DFU_STA_IDLE = 0x02;
        private readonly byte DFU_STA_DLBUSY = 0x04;
        private readonly byte DFU_STA_ERROR = 0x0A;

        //DFU Requests
        private readonly byte DFU_REQ_DOWNLOAD = 0x01;
        private readonly byte DFU_REQ_GETSTATUS = 0x03;
        private readonly byte DFU_REQ_CLEARSTATUS = 0x04;

        public delegate void ProgressCallback(int percentage, string message);
        public DFU(Device _device, Firmware _firmware)
        {
            device = _device;
            firmware = _firmware;
            dfuStatus = new DFUStatus();
        }
        private async Task ClearStatus()
        {
            var setupPacket = new SetupPacket(
                requestType: new UsbDeviceRequestType(
                    RequestDirection.In,
                    RequestType.Class,
                    RequestRecipient.Interface),
                request: DFU_REQ_CLEARSTATUS
            );

            await this.device.Handle.SendControlTransferAsync(setupPacket, null);
        }
        private async Task GetStatus()
        {
            byte[] buffer = new byte[6];

            var setupPacket = new SetupPacket(
                requestType: new UsbDeviceRequestType(
                    RequestDirection.In,
                    RequestType.Class,
                    RequestRecipient.Interface),
                request: DFU_REQ_GETSTATUS,
                length: 6
            );

            var res = await this.device.Handle.SendControlTransferAsync(setupPacket, buffer);

            if (res.BytesTransferred != buffer.Length)
            {
                throw new Exception("Error while getting the device's status");
            }

            dfuStatus.bStatus = res.Data[0];
            dfuStatus.bwPollTimeout = (res.Data[3] & 0xFF) << 16;
            dfuStatus.bwPollTimeout |= (res.Data[2] & 0xFF) << 8;
            dfuStatus.bwPollTimeout |= (res.Data[1] & 0xFF);
            dfuStatus.bState = res.Data[4];
        }
        private async Task WaitForDevice()
        {
            int waitTime = dfuStatus.bwPollTimeout;
            do
            {
                Thread.Sleep(waitTime);
                await ClearStatus();
                await GetStatus();
            } while (dfuStatus.bState != DFU_STA_IDLE);
        }
        private async Task EraseFlash()
        {
            await WaitForDevice();
            byte[] buffer = new byte[1];
            buffer[0] = DFU_CMD_MASSERASE;
            await Command(buffer);
            await GetStatus();
            Thread.Sleep(dfuStatus.bwPollTimeout);
            await GetStatus();
            if (dfuStatus.bState == DFU_STA_ERROR)
            {
                throw new Exception("Error while erasing the Flash");
            }
            //await WaitForDevice();
        }
        private async Task SetAddress(long addr)
        {
            byte[] buffer = new byte[5];
            buffer[0] = DFU_CMD_SETADDRESS;
            buffer[1] = (byte)(addr & 0xFF);
            buffer[2] = (byte)((addr >> 8) & 0xFF);
            buffer[3] = (byte)((addr >> 16) & 0xFF);
            buffer[4] = (byte)((addr >> 24) & 0xFF);
            await Command(buffer);
        }

        private async Task WriteFlash(ProgressCallback progressCallback)
        {
            await WaitForDevice();

            await SetAddress(START_ADDRESS);
            await GetStatus();
            await GetStatus();

            if (dfuStatus.bState == DFU_STA_ERROR)
            {
                throw new Exception("Failed setting flashing start address");
            }

            byte[] block = new byte[BLOCK_SIZE];
            int blocksCount = (int)(device.TotalBytes / BLOCK_SIZE);

            int blockNumber;

            for (blockNumber = 0; blockNumber < blocksCount; blockNumber++)
            {
                System.Array.Copy(firmware.Bytes, (blockNumber * BLOCK_SIZE), block, 0, BLOCK_SIZE);
                await WriteBlock(blockNumber, block);
                device.TransferredBytes += BLOCK_SIZE;
                progressCallback(device.FlashProgress, "Flashing");
            }

            int remainingBytes = (int)(device.TotalBytes - (blockNumber * BLOCK_SIZE));
            if (remainingBytes > 0)
            {
                System.Array.Copy(firmware.Bytes, (blockNumber * BLOCK_SIZE), block, 0, remainingBytes);
                while (remainingBytes < block.Length)
                {
                    block[remainingBytes++] = (byte)0xFF;
                }
                await WriteBlock(blockNumber, block);
                progressCallback(device.FlashProgress, "Flashing");
            }
        }
        private async Task WriteBlock(int blockNumber, byte[] block)
        {
            await Download(blockNumber + 2, block);
            await GetStatus();
            if (dfuStatus.bState != DFU_STA_DLBUSY)
            {
                throw new Exception("Error while sending block, device did not acknowledge");
            }
            Thread.Sleep(dfuStatus.bwPollTimeout);
            await GetStatus();
            if (dfuStatus.bState == DFU_STA_ERROR)
            {
                throw new Exception("Error while sending block");
            }
        }
        private async Task Download(int blockNumber, byte[] block)
        {
            var setupPacket = new SetupPacket(
                requestType: new UsbDeviceRequestType(
                    RequestDirection.Out,
                    RequestType.Class,
                    RequestRecipient.Interface),
                request: DFU_REQ_DOWNLOAD,
                length: (ushort)block.Length,
                value: (ushort)blockNumber
            );

            var res = await this.device.Handle.SendControlTransferAsync(setupPacket, block);

            if (res.BytesTransferred != block.Length)
            {
                throw new Exception("Error while sending a block to the device");
            }
        }
        private async Task EraseBlock(long addr)
        {
            byte[] buffer = new byte[5];
            buffer[0] = 0x21;
            buffer[1] = (byte)(addr & 0xFF);
            buffer[2] = (byte)((addr >> 8) & 0xFF);
            buffer[3] = (byte)((addr >> 16) & 0xFF);
            buffer[4] = (byte)((addr >> 24) & 0xFF);
            await Command(buffer);
        }
        private async Task Command(byte[] buffer)
        {
            var setupPacket = new SetupPacket(
                requestType: new UsbDeviceRequestType(
                    RequestDirection.Out,
                    RequestType.Class,
                    RequestRecipient.Interface),
                request: DFU_REQ_DOWNLOAD,
                length: (ushort)buffer.Length
            );

            var res = await this.device.Handle.SendControlTransferAsync(setupPacket, buffer);

            if (res.BytesTransferred != buffer.Length)
            {
                throw new Exception("Error while sending a command to the device");
            }
        }
        private async Task Reboot()
        {

            var setupPacket = new SetupPacket(
                requestType: new UsbDeviceRequestType(
                    RequestDirection.In,
                    RequestType.Class,
                    RequestRecipient.Interface),
                request: DFU_REQ_DOWNLOAD
            );

            await this.device.Handle.SendControlTransferAsync(setupPacket, null);
        }
        public async Task Run(ProgressCallback progressCallback)
        {
            device.TotalBytes = firmware.Bytes.Length - DFU_SUFFIX_LENGTH;

            progressCallback(0, "Erasing flash");
            await EraseFlash();

            await WriteFlash(progressCallback);

            progressCallback(0, "Completing Flash");
            await WaitForDevice();

            progressCallback(0, "Restarting keyboard");
            await Reboot();
            await GetStatus();

            device.Handle.Dispose();
            device.Status = "Complete";
        }
    }
}
