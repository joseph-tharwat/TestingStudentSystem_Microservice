using TestManagment.Shared.Result;

namespace TestManagment.Domain.SuccessNotes
{
    public static class TestNotes
    {
        public static SuccessNote CreatedSuccessfully => new SuccessNote(SuccessType.Created, "Test created successfully");
    }
}
