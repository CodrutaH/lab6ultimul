using Lab2.Models;

namespace Lab2.Servies
{
    public class PostUserDto
    {

        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }

        public static User ToUser(PostUserDto userModel)
        {
            return new User
            {
                FullName = userModel.FullName,
                Username = userModel.UserName,
                Email = userModel.Email,
            };
        }
    }
}