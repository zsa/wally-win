using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using IntelHexFormatReader;
using IntelHexFormatReader.Model;

namespace UtilsLibrary
{
    public class Firmware
    {
        public string FilePath { get; set; }
        public bool Opened { get; set; } = false;
        public byte[] Bytes { get; set; }
        public Firmware(string filePath)
        {
            this.FilePath = filePath;
            OpenFile();
        }

        private static readonly int ErgodoxMemSize = 32256;

        private void ProcessDFUFile()
        {
            if (Encoding.UTF8.GetString(Bytes, Bytes.Length - 8, 3) != "UFD")
            {
                throw new FormatException("DFU File is missing a prefix");
            }
        }
        private void OpenFile()
        {
            try
            {
                // TODO Add CRC Check
                if (Target == Target.stm32)
                {
                Bytes = File.ReadAllBytes(FilePath);
                    ProcessDFUFile();
                    Opened = true;
                }

                if (Target == Target.teensy)
                {
                    HexFileReader reader = new HexFileReader(FilePath, ErgodoxMemSize);
                    MemoryBlock memory = reader.Parse();

                    Bytes = new byte[ErgodoxMemSize];
                    var i = 0;
                    foreach(MemoryCell cell in memory.Cells)
                    {
                        Bytes[i] = cell.Value;
                        i++;
                    }
                    Opened = true;
                }
            }
            catch (Exception exception)
            {
                throw new FormatException("Error while opening firmware file", exception);
            }
        }

        private String FileExt
        {
            get
            {
                return Path.GetExtension(FilePath);
            }
        }
        public Target Target
        {
            get
            {
                if (FileExt == ".hex")
                {
                    return Target.teensy;
                }
                if (FileExt == ".bin")
                {
                    return Target.stm32;
                }
                return Target.unknown;
            }
        }
    }
}
