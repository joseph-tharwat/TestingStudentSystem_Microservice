using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TestManagment.ApplicationLayer.Interfaces.QueryMediator;
using TestManagment.PresentaionLayer;
using TestManagment.Shared.Requests;

namespace TestManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetQuestionController : ControllerBase
    {
        private readonly IRqtHandler<GetNextQuestionRequest, NextQuestion> handler;
        private readonly IHubContext<TestObservationHub> testObservationHub;

        public GetQuestionController(IRqtHandler<GetNextQuestionRequest, NextQuestion> handler, IHubContext<TestObservationHub> testObservationHub)
        {
            this.handler = handler;
            this.testObservationHub = testObservationHub;
        }

        [HttpGet("GetNextQuestion")]
        public async Task<ActionResult<NextQuestion>> GetNextQuestion([FromQuery]GetNextQuestionRequest studentProgress)
        {
            try
            {
                var nextQuestion = await handler.Handle(studentProgress);
                await testObservationHub.Clients.All.SendAsync("StudentGotNextQuestion", Request.Headers["x-UserName"].ToString(), studentProgress);
                return Ok(nextQuestion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
