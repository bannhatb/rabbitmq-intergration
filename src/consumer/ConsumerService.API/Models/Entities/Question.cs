using System.ComponentModel.DataAnnotations;

namespace ConsumerService.API.Models.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(2048)]
        public string QuestionContent { get; set; }

        [MaxLength(2048)]
        public string Explaint { get; set; }

        public IList<QuestionExam> QuestionExams { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
