using System.ComponentModel.DataAnnotations;

namespace GradingManagment.Shared
{
    public class StudentAnswer
    {
        public string? StudentId { get; set; }
        [Required]
        public int TestId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string Answer { get; set; }
    }
}
