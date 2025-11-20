using Microsoft.EntityFrameworkCore;
using TestManagment.Domain.Entities;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Shared.Dtos;

namespace TestManagment.Services.CreateTest
{
    public class CreateTestService
    {
        private TestDbContext dbContext { get; }

        public CreateTestService(TestDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task CreateTest(CreateTestDto createTestDto)
        {
            var validQuestionIds = await dbContext.Questions
                .Where(q => createTestDto.questionsIds.Contains(q.Id))
                .Select(q => q.Id)
                .ToListAsync();

            var invalidIds = createTestDto.questionsIds.Except(validQuestionIds).ToList();
            if(invalidIds.Count!=0)
            {
                throw new ArgumentException($"Invalid question ids {string.Join(",",invalidIds)}");
            }

            var test = ObjectMapper.CreateTestRequestToTest(createTestDto);
            await dbContext.AddAsync(test);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddQuestionToTest(ModifyTestRequest modifyTestRequest)
        {
            if(modifyTestRequest.testId == 0)
            {
                throw new ArgumentException("Invalid test id");
            }
            if (modifyTestRequest.questionId == 0)
            {
                throw new ArgumentException("Invalid question id");
            }

            var test = await dbContext.Tests.Where(t => t.Id == modifyTestRequest.testId).Include(t=>t.TestQuestions).FirstOrDefaultAsync();
            if(test == null)
            {
                throw new ArgumentException("This test is not created");
            }

            test.AddQuestion(modifyTestRequest.questionId);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveQuestionToTest(ModifyTestRequest modifyTestRequest)
        {
            if (modifyTestRequest.testId == 0)
            {
                throw new ArgumentException("Invalid test id");
            }
            if (modifyTestRequest.questionId == 0)
            {
                throw new ArgumentException("Invalid question id");
            }

            var test = await dbContext.Tests.Where(t => t.Id == modifyTestRequest.testId).Include(t => t.TestQuestions).FirstOrDefaultAsync();
            if (test == null)
            {
                throw new ArgumentException("This test is not created");
            }

            test.RemoveQuestion(modifyTestRequest.questionId);
            await dbContext.SaveChangesAsync();
        }

    }
}
