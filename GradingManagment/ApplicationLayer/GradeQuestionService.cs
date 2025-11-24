using AutoMapper;
using GradingManagment.Domain.Entities;
using GradingManagment.Infrastructure.Database;
using GradingManagment.Shared;
using Microsoft.EntityFrameworkCore;

namespace GradingManagment.ApplicationLayer
{
    public class GradeQuestionService
    {
        private readonly GradingDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GradeQuestionService(GradingDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task GradeStudentAnswerAsync(StudentAnswer studentAnswer)
        {
            string studentId = httpContextAccessor.HttpContext.Request.Headers["X-studentId"];
            studentAnswer.StudentId = studentId;

            var studnetGrade = mapper.Map<StudentGrade>(studentAnswer);

            var existedAnswer = await dbContext.StudentGrades
                .Where(g => g.StudentId == studnetGrade.StudentId
                && g.TestId == studnetGrade.TestId
                && g.QuestionId == studnetGrade.QuestionId).FirstOrDefaultAsync();

            if (existedAnswer != null)
            {
                throw new Exception("This question is already submitted before");
            }

            var correctAnswer = await dbContext.QuestionInformations
                .Where(q => q.QuestionId == studnetGrade.QuestionId)
                .Select(q => q.Answer)
                .FirstOrDefaultAsync();

            if (correctAnswer == null)
            {
                throw new Exception("The Question id is not exist");
            }

            if (correctAnswer != studnetGrade.StudentAnswer)
            {
                studnetGrade.Grade = 0;
            }
            await dbContext.AddAsync(studnetGrade);
            await dbContext.SaveChangesAsync();
        }
    }
}
