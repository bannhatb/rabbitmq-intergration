﻿using System;
using ConsumerService.API.Models.Entities;

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

