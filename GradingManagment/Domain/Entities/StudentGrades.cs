namespace GradingManagment.Domain.Entities
{
    public class StudentGrade
    {
        public string StudentId { get; set; }
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public string StudentAnswer { get; set; }
        public int Grade { get; set; }
    }
}
