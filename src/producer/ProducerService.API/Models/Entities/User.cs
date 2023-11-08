using System.ComponentModel.DataAnnotations;

namespace ProducerService.API.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(32)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(32)]
        public string FirstName { get; set; }

        [MaxLength(32)]
        public string LastName { get; set; }
        public bool IsBlock { get; set; }

        public ICollection<TestUser> TestUsers { get; set; }
        public ICollection<TestQuestionUserChoose> TestQuestionUserChooses { get; set; }
    }
}
