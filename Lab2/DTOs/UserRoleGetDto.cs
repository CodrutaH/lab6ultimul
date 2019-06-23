namespace Lab2.DTOs
{
    public class UserRoleGetDto
    {
        
            public int Id { get; set; }
            public string Name { get; set; }

            public static UserRoleGetDto FromRole(Models.UserRole role)
            {
                return new UserRoleGetDto
                {
                    Id = role.Id,
                    Name = role.Name
                };
            }
        }
}