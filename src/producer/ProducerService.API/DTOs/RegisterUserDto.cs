using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProducerService.API.DTOs
{
    public class RegisterUserDto
    {
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
    }
}