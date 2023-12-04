
namespace ConsumerService.API.DTOs
{
    public class ExamDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Time { get; set; }
        public string CreateBy { get; set; }
        public int QuestionCount { get; set; }
    }

    public class ExamDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<QuestionDto> QuestionDtos { get; set; }
    }
}