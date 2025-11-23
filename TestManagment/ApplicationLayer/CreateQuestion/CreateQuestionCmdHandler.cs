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
        private readonly IDomainEventDispatcher eventDispatcher;

        public CreateQuestionCmdHandler(TestDbContext _dbContext, IDomainEventDispatcher eventDispatcher)
        {
            this.dbContext = _dbContext;
            this.eventDispatcher = eventDispatcher;
        }
        public async Task Handle(CreateQuestionCmd request)
        {
            var question = ObjectMapper.QuestionRequestToQuestion(request);
            await dbContext.AddAsync(question);
            await dbContext.SaveChangesAsync();

            var questionInfo = ObjectMapper.QuestionToQuestionCreatedInfo(question);
            await eventDispatcher.DispatchAsync(new OneQuestionCreatedEvent(questionInfo));
        }
    }
}
