
namespace ConsumerService.API.Models.Events
{
    public class TestResultEvent : IntegrationEvent
    {
        public int UserId { get; set; } // who is doing
        public int ExamId { get; set; }

        public List<ResultUserChoose> ResultUserChooses { get; set; }
    }

    public class ResultUserChoose
    {
        public int QuestionId { get; set; }
        public int? Choose { get; set; } // answerId
    }
}