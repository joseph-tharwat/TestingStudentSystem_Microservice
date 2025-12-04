using TestManagment.ApplicationLayer.Interfaces.EventMediator;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Domain.Events;
using TestManagment.Infrastructure.DataBase;
using TestManagment.Shared.Dtos;
using TestManagment.Shared.Result;

namespace TestManagment.ApplicationLayer.CreateQuestion
{
    public class CreateQuestionListCmdHandler : ICmdHandler<CreateQuestionListCmd>
    {
        private TestDbContext dbContext { get; }
        private readonly IDomainEventDispatcher eventDispatcher;

        public CreateQuestionListCmdHandler(TestDbContext _dbContext, IDomainEventDispatcher eventDispatcher)
        {
            this.dbContext = _dbContext;
            this.eventDispatcher= eventDispatcher;
        }
        public async Task<Result> Handle(CreateQuestionListCmd cmd)
        {
            var questions = ObjectMapper.QuestionRequestListToQuestionList(cmd.List);
            await dbContext.AddRangeAsync(questions);
            await dbContext.SaveChangesAsync();

            var questionsInfo = ObjectMapper.QuestionListToQuestionCreatedInfoList(questions);
            await eventDispatcher.DispatchAsync(new ManyQuestionsCreatedEvent(questionsInfo));
            return null;
        }
    }
}
