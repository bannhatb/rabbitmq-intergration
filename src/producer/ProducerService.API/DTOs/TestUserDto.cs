
using System.ComponentModel.DataAnnotations;

namespace ProducerService.API.DTOs
{
    public class TestUserDto
    {

        public int? Id { get; set; }
        [Required]
        public int UserId { get; set; } // who is doing

        [Required]
        public int ExamId { get; set; }
        public double Point { get; set; }
    }

    public class RankUserDto
    {
        public int UserId { get; set; } // who is doing
        public string Username { get; set; }
        public int ExamId { get; set; }
        public double Point { get; set; }
    }
}