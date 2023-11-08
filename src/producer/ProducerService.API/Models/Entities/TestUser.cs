using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProducerService.API.Models.Entities
{
    public class TestUser
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; } // who is doing
        public User User { get; set; }

        [Required]
        public int ExamId { get; set; }

        [Required]
        public double Point { get; set; }
    }
}
