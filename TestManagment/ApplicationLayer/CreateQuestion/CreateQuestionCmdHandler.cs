using TestManagment.ApplicationLayer.Interfaces.EventMediator;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Domain.Events;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Shared.Dtos;

namespace TestManagment.ApplicationLayer.CreateQuestion
{
    public class CreateQuestionCmdHandler : ICmdHandler<CreateQuestionCmd>
    {
        private TestDbContext dbContext { get; }
        private readonly IDomainEventHandler<OneQuestionCreatedEvent> eventHandler;

        public CreateQuestionCmdHandler(TestDbContext _dbContext, IDomainEventHandler<OneQuestionCreatedEvent> eventHandler)
        {
            this.dbContext = _dbContext;
            this.eventHandler = eventHandler;
        }
        public async Task Handle(CreateQuestionCmd request)
        {
            var question = ObjectMapper.QuestionRequestToQuestion(request);
            await dbContext.AddAsync(question);
            await dbContext.SaveChangesAsync();

            var questionInfo = ObjectMapper.QuestionToQuestionCreatedInfo(question);
            await eventHandler.Publish(new OneQuestionCreatedEvent(questionInfo));
        }
    }
}
