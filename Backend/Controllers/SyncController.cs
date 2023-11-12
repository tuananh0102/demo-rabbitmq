using Backend.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly ISyncBl _syncBl;
        public SyncController(ISyncBl syncBl)
        {
            _syncBl = syncBl;
        }

        [HttpPost]
        public async Task<IActionResult> Sync([FromQuery] int num, [FromBody] string message)
        {
            _syncBl.PublishMessage(num, message);

            return StatusCode(StatusCodes.Status202Accepted);
        }
    }
}
