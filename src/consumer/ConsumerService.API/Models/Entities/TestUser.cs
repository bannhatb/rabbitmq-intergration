using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsumerService.API.Models.Entities
{
    public class TestUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; } // who is doing

        [Required]
        public int ExamId { get; set; }

        [Required]
        public double Point { get; set; }
        public DateTime CreateDay { get; set; }
    }
}
