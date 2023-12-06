using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProducerService.API.Models.Events
{
    public class ScoreUserTestEvent : IntegrationEvent
    {
        public int UserId { get; set; } // who is doing
        public int ExamId { get; set; }
        public double Point { get; set; }
    }
}