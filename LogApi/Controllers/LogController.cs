using Log.Models;
using LogApi.Models;
using LogApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController(ILogDataService logDataService) : ControllerBase
    {
        [HttpPost("setlog", Name = "LogAppMessage")]
        public async Task<IActionResult> LogAppMessage([FromBody] AppLogParameters parameters)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);


            var lgType = await logDataService.ValidateMessageType(parameters);
            if (lgType == 0)
            {
                ModelState.AddModelError("MessageType", "Message type is invalid");
            }


            if (!ModelState.IsValid) return BadRequest(ModelState);


            var msgAdded = await logDataService.LogMessage(parameters, lgType);
            return msgAdded ? Ok(new { Message = "Log Successfully added" }) : StatusCode(500, "Server Error");
        }

        [HttpPost("email", Name = "SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailFields parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var emailSent = await logDataService.SendEmail(parameters);
            return emailSent ? Ok(true) : StatusCode(500, "Server Error");

        }

        [HttpPost("setevtlog/email", Name = "SendEvtEmail")]
        public async Task<IActionResult> SendEvtEmail([FromBody] EmailFields parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var emailSent = await logDataService.SendEmail(parameters);
            return emailSent ? Ok(true) : StatusCode(500, "Server Error");

        }

        [HttpPost("setevtlog", Name = "LogEventMessage")]
        public async Task<IActionResult> LogEventMessage([FromBody] EventViewParameters parameters)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var msgAdded = await logDataService.LogEventMessage(parameters);
            return msgAdded ? Ok(new { Message = "Log Successfully added" }) : StatusCode(500, "Server Error");
        }
    }
}
