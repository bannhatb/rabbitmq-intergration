
using Microsoft.AspNetCore.Mvc;
using ProducerService.API.DTOs;
using ProducerService.API.Repositories;
using ProducerService.API.Services;

namespace ProducerService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;

        public AuthController(IAuthService authService, IUserRepository userRepository, IHttpContextAccessor httpContext)
        {
            _authService = authService;
            _userRepository = userRepository;
            _httpContext = httpContext;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                return Ok(_authService.Register(registerUserDto));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthUserDto authUserDto)
        {
            try
            {
                var token = _authService.Login(authUserDto);
                return Ok(new { access_token = token, token_type = "bearer" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("get-list-user")]
        public IActionResult GetListUser()
        {
            try
            {
                return Ok(_userRepository.GetUsers());
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("get-id-username-current-user")]
        public IActionResult GetIdUsernameCurrentUser()
        {
            try
            {
                int userId = int.Parse(_httpContext.HttpContext.User.Identity.Name.ToString());
                return Ok(userId);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}