using MediatR;
using TestManagment.Domain.Entities;
using TestManagment.Domain.Events;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Shared.Dtos;

namespace TestManagment.Services.CreateTest
{
    public class CreateQuestionService
    {
        private TestDbContext dbContext { get; }
        private readonly IMediator mediator;

        public CreateQuestionService(TestDbContext _dbContext, IMediator mediator)
        {
            dbContext = _dbContext;
            this.mediator = mediator;
        }

        public async Task CreateQuestion(QuestionDto questionDto)
        {
            var question = ObjectMapper.QuestionRequestToQuestion(questionDto);
            await dbContext.AddAsync(question);

            var questionInfo = ObjectMapper.QuestionToQuestionCreatedInfo(question);
            await mediator.Publish(new OneQuestionCreatedEvent(questionInfo));
            
            await dbContext.SaveChangesAsync();
        }

        public async Task CreateQuestions(List<QuestionDto> questionDtos)
        {
            //var questions = mapper.Map<List<Question>>(questionDtos);
            var questions = ObjectMapper.QuestionRequestListToQuestionList(questionDtos);
            await dbContext.AddRangeAsync(questions);

            var questionsInfo = ObjectMapper.QuestionListToQuestionCreatedInfoList(questions);
            await mediator.Publish(new ManyQuestionsCreatedEvent(questionsInfo));

            await dbContext.SaveChangesAsync();
        }


    }
}
