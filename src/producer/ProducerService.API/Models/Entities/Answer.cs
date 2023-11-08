using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProducerService.API.Models.Entities
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string AnswerContent { get; set; }

        [Required]
        public bool RightAnswer { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
