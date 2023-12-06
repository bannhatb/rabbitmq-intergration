using System;
namespace ProducerService.API.Models.Events
{
	public class DemoEvent : IntegrationEvent
	{
		public string? Data { get; set; }
	}
}

