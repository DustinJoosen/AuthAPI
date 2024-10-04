using Auth.API.Abstractions.Interfaces;
using Auth.API.Presentation.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Auth.API.Presentation.Controllers
{
    [Route("health")]
    [ApiController]
    public class HealthController : ControllerBase
    {

        [LogWhenRequestMade]
        [HttpGet]
        [Route("")]
        public IActionResult Health()
        {
            return Ok("Auth API is running");
        }
    }
}
