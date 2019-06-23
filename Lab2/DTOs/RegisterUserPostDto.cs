using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lab2.Models;

namespace Lab2.DTOs
{
    public class RegisterUserPostDto
    {
       
        public string FullName { get; set; }

        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        internal static User ToUser(RegisterUserPostDto userRole)
        {
            return new User
            {
                FullName = userRole.FullName,
                Username = userRole.Username,
                Email = userRole.Email,
                Password = userRole.Password
            };
        }
    }
}
