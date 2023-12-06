using System;
namespace ConsumerService.API.Models.Events
{
	public class DemoEvent : IntegrationEvent
	{
		public string? Data { get; set; }
	}
}

