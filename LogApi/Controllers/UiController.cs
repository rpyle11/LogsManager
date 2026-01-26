using Log.Models;
using LogApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UiController(ILogDataService logDataService) : ControllerBase
    {
        [HttpPost("logs", Name = "GetLogs")]
        public async Task<IActionResult> GetLogs([FromBody] GetLogsParameters parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var data = await logDataService.GetLogs(parameters);

            return data == null ? StatusCode(500, "Internal Error") : Ok(data);
        }

        [HttpGet("applications", Name = "AllApplications")]
        public async Task<IActionResult> AllApplications()
        {
            var data = await logDataService.GetAllApplications();

            return data == null ? StatusCode(500, "Internal Error") : Ok(data);
        }


        [HttpPost("addupdateapplication", Name = "AddUpdateApplication")]
        public async Task<IActionResult> AddUpdateApplication([FromBody] AddUpdateApplicationParameters parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var data = await logDataService.AddUpdateApplication(parameters);
            if (data != null)
            {
                return Ok(data);
            }

            return NotFound(new { Message = "Posted application was not found" });
        }

        [HttpGet("minmaxdates", Name = "GetMinMaxDates")]
        public async Task<IActionResult> GetStartEndDates()
        {
            var data = await logDataService.GetMinMaxDates();

            return data == null ? StatusCode(500, "Internal Error") : Ok(data);
        }
    }
}
