using TestManagment.Shared.Result;

namespace TestManagment.ApplicationLayer.ErrorsNotes
{
    public static class DatabaseErrors
    {
        public static ErrorNote FailedDuringSaveChanges => new ErrorNote(ErrorType.Unexpected, "Can not save changes in the database");
    }
}
