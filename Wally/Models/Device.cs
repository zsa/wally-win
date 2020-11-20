using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsLibrary;

namespace Wally.Models
{
    public class Device
    {
        public int Pid { get; set; }
        public Target Target { get; set; }
        public string FriendlyName { get; set; }
        public Device(int pid, string friendlyName, Target target)
        {
            Pid = pid;
            FriendlyName = friendlyName;
            Target = target;
        }
    }
}
