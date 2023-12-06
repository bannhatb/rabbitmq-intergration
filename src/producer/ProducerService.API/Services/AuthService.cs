using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProducerService.API.DTOs;
using ProducerService.API.Models.Entities;
using ProducerService.API.Repositories;

namespace ProducerService.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly Audience _audience;

        public AuthService(IUserRepository userRepository, IOptions<Audience> options)
        {
            _userRepository = userRepository;
            _audience = options.Value ?? throw new ArgumentException(nameof(options.Value));
        }

        public string GenerateToken(int userId, string username)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, userId.ToString())
            };

            var signningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_audience.Secret));

            var jwt = new JwtSecurityToken(
                issuer: _audience.Issuer,
                audience: _audience.Name,
                claims: claims,
                notBefore: now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(signningKey,
                SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string Login(AuthUserDto authUserDto)
        {
            authUserDto.Username = authUserDto.Username.ToLower();

            var currentUser = _userRepository.GetUserByUsername(authUserDto.Username);

            if (currentUser == null)
            {
                throw new UnauthorizedAccessException("Username is invalid!");
            }

            var checkPassword = BCrypt.Net.BCrypt.Verify(authUserDto.Password, currentUser.Password);

            return checkPassword ? this.GenerateToken(currentUser.Id, currentUser.Username) : "Password is invalid";
        }

        public string Register(RegisterUserDto registerUserDto)
        {
            registerUserDto.Username = registerUserDto.Username.ToLower();
            var currentUser = _userRepository.GetUserByUsername(registerUserDto.Username);
            if (currentUser != null)
            {
                throw new BadHttpRequestException("Username is already registered");
            }

            registerUserDto.Password = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);

            var newUser = new User()
            {
                Username = registerUserDto.Username,
                Password = registerUserDto.Password,
                Email = registerUserDto.Email,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                IsBlock = false
            };

            _userRepository.Add(newUser);
            _userRepository.IsSaveChanges();

            return "Register Success";
        }
    }
}