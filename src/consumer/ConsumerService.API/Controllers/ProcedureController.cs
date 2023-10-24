using ConsumerService.API.Models;
using Microsoft.AspNetCore.Mvc;
using ProducerService.API.Services;

namespace ConsumerService.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProcedureController : ControllerBase
    {
        private readonly ILogger<ProcedureController> _logger;
        private readonly IEventBusService _eventBus;

        public ProcedureController(ILogger<ProcedureController> logger, IEventBusService eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        [HttpPost("publish")]
        public IActionResult Publish([FromBody] MessageModel message)
        {
            if (ModelState.IsValid)
            {
                _eventBus.Publish(message);

                return Ok(new { msg = "message has been delivery" });
            }

            return BadRequest(new { msg = "invalid message" });
        }
    }
}

