using Microsoft.AspNetCore.Mvc;
using ProducerService.API.Models.Entities;
using ProducerService.API.Services;

namespace ProducerService.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProducerController : ControllerBase
    {
        private readonly ILogger<ProducerController> _logger;
        private readonly IEventBusService _eventBus;

        public ProducerController(ILogger<ProducerController> logger, IEventBusService eventBus)
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

