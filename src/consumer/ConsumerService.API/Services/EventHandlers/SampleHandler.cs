using System;
using ConsumerService.API.Models;

namespace ConsumerService.API.Services.EventHandlers
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

