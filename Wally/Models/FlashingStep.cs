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
