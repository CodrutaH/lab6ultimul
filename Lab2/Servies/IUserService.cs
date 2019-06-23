using Lab2.DTOs;
using Lab2.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Servies
{
    public interface IUserService
    {
        GetUserDto Authenticate(string username, string password);

        GetUserDto Register(RegisterUserPostDto registerUserPostDto);

        User GetCurrentUser(HttpContext httpContext);

        IEnumerable<GetUserDto> GetAll();

        User Delete(int id, User addedBy);
        //User Delete(int id);

        IEnumerable<UseUserRoleGetModel> GetHistoryById(int id);
        List<UseUserRoleGetModel> GetAllRoles(int id);

        User Create( RegisterUserPostDto userNew);
        GetUserDto GetById(int id);
        User Upsert(int id, PostUserDto userNew, User addedBy);
        //User GetCurentUser(HttpContext httpContext);
    }

}




