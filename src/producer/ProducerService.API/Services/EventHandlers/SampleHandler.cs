using System;
using ProducerService.API.Models.Entities;

namespace ProducerService.API.Services.EventHandlers
{
	public class SampleHandler
	{
		public SampleHandler()
		{
		}

		public void Process(MessageModel message)
		{
			Console.WriteLine(message);
		}
	}
}

