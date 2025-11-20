using TestManagment.ApplicationLayer.Interfaces.EventMediator;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Domain.Events;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Shared.Dtos;

namespace TestManagment.ApplicationLayer.CreateQuestion
{
    public class CreateQuestionListCmdHandler : ICmdHandler<CreateQuestionListCmd>
    {
        private TestDbContext dbContext { get; }
        private readonly IDomainEventHandler<ManyQuestionsCreatedEvent> eventHandler;

        public CreateQuestionListCmdHandler(TestDbContext _dbContext, IDomainEventHandler<ManyQuestionsCreatedEvent> eventHandler)
        {
            this.dbContext = _dbContext;
            this.eventHandler = eventHandler;
        }
        public async Task Handle(CreateQuestionListCmd cmd)
        {
            var questions = ObjectMapper.QuestionRequestListToQuestionList(cmd.List);
            await dbContext.AddRangeAsync(questions);
            await dbContext.SaveChangesAsync();

            var questionsInfo = ObjectMapper.QuestionListToQuestionCreatedInfoList(questions);
            await eventHandler.Publish(new ManyQuestionsCreatedEvent(questionsInfo));
        }
    }
}
