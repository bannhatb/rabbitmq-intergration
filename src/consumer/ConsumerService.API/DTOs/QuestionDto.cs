
namespace ConsumerService.API.DTOs
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public string Explaint { get; set; }
        public List<AnswerDto> AnswerDtos { get; set; }
    }

    public class AnswerDto
    {
        public int AnswerId { get; set; }
        public string AnswerContent { get; set; }
        public bool RightAnswer { get; set; }
    }

    public class QuestionExamDto
    {
        public int ExamId { get; set; }
        public List<int> QuestionIds { get; set; }
    }
}