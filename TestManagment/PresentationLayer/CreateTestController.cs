using Microsoft.AspNetCore.Mvc;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.Shared.Dtos;

namespace TestManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateTestController : ControllerBase
    {
        private readonly ICmdHandler<CreateTestCmd> createTestHandler;
        private readonly ICmdHandler<AddQuestionToTestCmd> addQuestionToTestHandler;
        private readonly ICmdHandler<RemoveQuestionFromTestCmd> removeQuestionFromTestHandler;

        public CreateTestController(ICmdHandler<CreateTestCmd> createTestHandler,
            ICmdHandler<AddQuestionToTestCmd> addQuestionToTestHandler,
            ICmdHandler<RemoveQuestionFromTestCmd> removeQuestionFromTestHandler)
        {
            this.createTestHandler = createTestHandler;
            this.addQuestionToTestHandler = addQuestionToTestHandler;
            this.removeQuestionFromTestHandler = removeQuestionFromTestHandler;
        }

        [HttpPost("CreateTest")]
        public async Task<IActionResult> CreateTest(CreateTestCmd createTestDto)
        {
            if (createTestDto == null)
            {
                return BadRequest("Test object must be not null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await createTestHandler.Handle(createTestDto);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddQuestionToTest")]
        public async Task<IActionResult> AddQuestionToTest(AddQuestionToTestCmd modifyTestRequest)
        {
            try
            {
                await addQuestionToTestHandler.Handle(modifyTestRequest);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok("Question was added successfully");
        }

        [HttpPost("RemoveQuestionToTest")]
        public async Task<IActionResult> RemoveQuestionToTest(RemoveQuestionFromTestCmd modifyTestRequest)
        {
            try
            {
                await removeQuestionFromTestHandler.Handle(modifyTestRequest);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok("Question was removed successfully successfully");
        }
    }
}
