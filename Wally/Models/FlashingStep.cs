using System;
using System.Collections.Generic;
using System.Text;

namespace Wally.Models
{
    public enum FlashingStep
    {
        SearchKeyboard,
        SelectKeyboard,
        SelectFirmware,
        SearchBootloader,
        Flash,
        Complete,
        DisplayLogs,
        Error
    }
}
