namespace Domain.Enums
{
    public enum Action
    {
        Pay = 0,
        Finish = 1,     // after payment and usage
        Cancel = 2,     // can never be paid
        Refound = 3,    // for already paid
        Reopen = 4,     // for already canceled
        Mantain = 5     // keeps current status
    }
}
