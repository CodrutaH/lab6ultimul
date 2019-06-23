using Lab2.Models;

namespace Lab2.DTOs
{
    public class UserRolePostDto
    {
        public string Name { get; set; }

        public static UserRole ToRole(UserRolePostDto role)
        {
            return new UserRole
            {
                Name = role.Name
            };
        }

    }

}