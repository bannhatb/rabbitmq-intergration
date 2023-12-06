using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProducerService.API.DTOs;

namespace ProducerService.API.Services
{
    public interface IAuthService
    {
        string Login(AuthUserDto authUserDto);
        string Register(RegisterUserDto registerUserDto);
        string GenerateToken(int userId, string username);
    }
}