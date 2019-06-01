using Lab2.DTOs;
using Lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Servies
{
    public interface IUserService
    {
        GetUserDto Authenticate(string username, string password);

        GetUserDto Register(RegisterUserPostDto registerInfo);

        IEnumerable<GetUserDto> GetAll();
    }

}
