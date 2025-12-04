using TestManagment.Shared.Result;

namespace TestManagment.Domain.SuccessNotes
{
    public static class QuestionNotes
    {
        public static SuccessNote QuestionAddedSuccessfully => new SuccessNote(SuccessType.Created, "Question added successfully");
    }
}
