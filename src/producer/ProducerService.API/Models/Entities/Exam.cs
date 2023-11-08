using System.ComponentModel.DataAnnotations;

namespace ProducerService.API.Models.Entities
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Title { get; set; }

        [Required]
        public int Time { get; set; }

        [Required]
        public string CreateBy { get; set; }

        [Required]
        public int QuestionCount { get; set; }

        public IList<QuestionExam> QuestionExams { get; set; }
    }
}
