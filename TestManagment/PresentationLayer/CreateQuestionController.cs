using Microsoft.AspNetCore.Mvc;
using Serilog;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Shared.Dtos;

namespace TestManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateQuestionController : ControllerBase
    {
        private readonly ICmdHandler<CreateQuestionCmd> createQuestionHandler;
        private readonly ICmdHandler<CreateQuestionListCmd> createQuestionListHandler;


        public CreateQuestionController(
            ICmdHandler<CreateQuestionCmd> createQuestionHandler,
            ICmdHandler<CreateQuestionListCmd> CreateQuestionListHandler)
        {
            this.createQuestionHandler = createQuestionHandler;
            createQuestionListHandler = CreateQuestionListHandler;
        }

        [HttpPost("CreateQuestion")]
        public async Task<IActionResult> CreateQuestion(CreateQuestionCmd cmd)
        {
            if(cmd == null)
            {
                return BadRequest("question must be not null");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await createQuestionHandler.Handle(cmd);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateQuestions")]
        public async Task<IActionResult> CreateQuestions(CreateQuestionListCmd cmd)
        {
            if (cmd == null)
            {
                return BadRequest("question must be not null");
            }
            if (cmd.List.Count == 0)
            {
                return BadRequest("At least send one question.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await createQuestionListHandler.Handle(cmd);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

    }
}
