﻿using ConsumerService.API.Models;
using Microsoft.AspNetCore.Mvc;
using ProducerService.API.Services;

namespace ConsumerService.API.Controllers;

[ApiController]
[Route("api")]
public class ConsumerController : ControllerBase
{

    private readonly ILogger<ConsumerController> _logger;
    private readonly IEventBusService _eventBus;

    public ConsumerController(ILogger<ConsumerController> logger, IEventBusService eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }

    [HttpPost("subscribe")]
    public IActionResult Publish([FromBody] MessageModel message)
    {
        if (ModelState.IsValid)
        {
            _eventBus.Publish(message);

            return Ok(new { msg = "message has been receive" });
        }

        return BadRequest(new { msg = "invalid message" });
    }

}
