using Microsoft.AspNetCore.Mvc;
using TestManagment.ApplicationLayer.Interfaces.CmdMediator;
using TestManagment.ApplicationLayer.PublishTest;

namespace TestManagment.PresentationLayer
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishTestController : ControllerBase
    {
        private readonly ICmdHandler<PublishTestCmd> publishhandler;
        private readonly ICmdHandler<UnPublishTestCmd> unPublishhandler;

        public PublishTestController(ICmdHandler<PublishTestCmd> publishhandler, ICmdHandler<UnPublishTestCmd> unPublishhandler)
        {
            this.publishhandler = publishhandler;
            this.unPublishhandler = unPublishhandler;
        }

        [HttpPost("PublishTest")]
        public async Task<IActionResult> PublishTest(PublishTestCmd cmd)
        {
            try
            {
                await publishhandler.Handle(cmd);
                return Ok("Published sucssesfully");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("UnPublishTest")]
        public async Task<IActionResult> UnPublishTest(UnPublishTestCmd cmd)
        {
            try
            {
                await unPublishhandler.Handle(cmd);
                return Ok("UnPublished sucssesfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
