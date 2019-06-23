using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Lab2.DTOs;
using Lab2.Models;
using Lab2.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Lab2.Servies
{
    public class UserService : IUserService
    {
        private ExpensesDbContext context;
        private readonly AppSettings appSettings;
        private readonly IRegisterValidator registerValidator;
        //private ErrorsCollection ErrorsCollection { get; private set; }

        public UserService(ExpensesDbContext context, IRegisterValidator registerValidator, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.registerValidator = registerValidator;
        }



        public GetUserDto Authenticate(string username, string password)
        {

            var user = context.Users
                .Include(us => us.UserUserRoles)
                .ThenInclude(uur => uur.UserRole)
                .SingleOrDefault(x => x.Username == username &&
                                        x.Password == ComputeSha256Hash(password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                     //new Claim(ClaimTypes.Role, user.Role.ToString())
                     new Claim(ClaimTypes.Role, GetLatestUserUserRole(user.UserUserRoles).UserRole.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new GetUserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };

            return result;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            // TODO: also use salt
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        public GetUserDto Register(RegisterUserPostDto register)
        {
            User existing = context.Users.FirstOrDefault(u => u.Username == register.Username);
            if (existing != null)
            {
                return null;
            }
            User user = new User
            {
                Email = register.Email,
                FullName = register.FullName,
                Password = ComputeSha256Hash(register.Password),
                Username = register.Username,
                CreatedAt = DateTime.Now
            };

            context.Users.Add(user);
            context.SaveChanges();
            context.Users.Attach(user);

            UserRole role = new UserRole
            {
                Id = 1,
                Name = RoleConstants.REGULAR
            };

            UserUserRole history = new UserUserRole
            {
                UserRole = role,
                StartTime = DateTime.Now
            };

            List<UserUserRole> list = new List<UserUserRole>
            {
                history
            };

            context.UserRoles.Add(role);
            context.UserRoles.Attach(role);

            user.UserUserRoles = list;

            context.SaveChanges();

            return Authenticate(register.Username, register.Password);
        }

        public User GetCurrentUser(HttpContext httpContext)
        {

            string username = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            //string accountType = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.AuthenticationMethod).Value;
            //return _context.Users.FirstOrDefault(u => u.Username == username && u.AccountType.ToString() == accountType);
            return context
                .Users
                .Include(u => u.UserUserRoles)
                .ThenInclude(us => us.UserRole)
                .FirstOrDefault(u => u.Username == username);
        }


        public IEnumerable<GetUserDto> GetAll()
        {
            // return users without passwords
            return context.Users.Select(user => new GetUserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = null
            });
        }

        public IEnumerable<UseUserRoleGetModel> GetHistoryById(int id)
        {
            List<UserUserRole> histories = context.Users
                .Include(x => x.UserUserRoles)
                .ThenInclude(x => x.UserRole)
                .FirstOrDefault(u => u.Id == id).UserUserRoles.ToList();

            List<UseUserRoleGetModel> returnList = new List<UseUserRoleGetModel>();
            foreach (UserUserRole history in histories)
            {
                returnList.Add(UseUserRoleGetModel.ToUseUserRoleGetModel(history));
            }
            var list = returnList.OrderBy(x => x.AllocatedAt);
            return list;
        }

        public GetUserDto GetById(int id)
        {
            User user = context.Users
                .FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return null;
            }
            return GetUserDto.FromUser(user);
        }

        public User Create(RegisterUserPostDto userNew)
        {
            // TODO: how to store the user that added the expense as a field in Expense?
            User toAdd = RegisterUserPostDto.ToUser(userNew);

            context.Users.Add(toAdd);
            context.SaveChanges();
            context.Users.Attach(toAdd);

            UserRole role = new UserRole
            {
                Id = 1,
                Name = RoleConstants.REGULAR
            };

            UserUserRole history = new UserUserRole
            {
                UserRole = role,
                StartTime = DateTime.Now
            };

            List<UserUserRole> list = new List<UserUserRole>
            {
                history
            };

            context.UserRoles.Add(role);
            context.UserRoles.Attach(role);

            toAdd.UserUserRoles = list;

            context.SaveChanges();

            return toAdd;
        }

        public User Delete(int id, User addedBy)
        {
            var existing = context.Users
               .Include(ex => ex.UserUserRoles)
               .FirstOrDefault(user => user.Id == id);

            if (existing == null)
            {
                return null;
            }
            String existingCurrentRole = GetLatestUserUserRole(existing.UserUserRoles).UserRole.Name;
            String addedByCurrentRole = GetLatestUserUserRole(addedBy.UserUserRoles).UserRole.Name;

            if (existing == null)
            {
                return null;
            }

            if (existingCurrentRole.Equals(RoleConstants.ADMIN) && !addedByCurrentRole.Equals(RoleConstants.ADMIN))
            {
                return null;
            }
            if (((!existingCurrentRole.Equals(RoleConstants.ADMIN) || (!existingCurrentRole.Equals(RoleConstants.USER_MANAGER)) && addedByCurrentRole.Equals(RoleConstants.USER_MANAGER))) ||
                (existingCurrentRole.Equals(RoleConstants.USER_MANAGER) && addedByCurrentRole.Equals(RoleConstants.USER_MANAGER) && addedBy.CreatedAt.AddMonths(6) <= DateTime.Now))
            {
                context.Comments.RemoveRange(context.Comments.Where(u => u.Owner.Id == existing.Id));
                context.SaveChanges();
                context.Expenses.RemoveRange(context.Expenses.Where(u => u.Owner.Id == existing.Id));
                context.SaveChanges();
                context.UserUserRoles.RemoveRange(context.UserUserRoles.Where(u => u.User.Id == existing.Id));
                context.SaveChanges();
            
                context.Users.Remove(existing);
                context.SaveChanges();
                return existing;
            }
            else if (addedByCurrentRole.Equals(RoleConstants.ADMIN))
            {
                context.Comments.RemoveRange(context.Comments.Where(u => u.Owner.Id == existing.Id));
                context.SaveChanges();
                context.Expenses.RemoveRange(context.Expenses.Where(u => u.Owner.Id == existing.Id));
                context.SaveChanges();
                context.UserUserRoles.RemoveRange(context.UserUserRoles.Where(u => u.User.Id == existing.Id));
                context.SaveChanges();

                context.Users.Remove(existing);
                context.SaveChanges();
                return existing;
            }
            return null;
        }

        // public User Upsert(int id, User user)
        //{
        //   var existing = context.Users.AsNoTracking().FirstOrDefault(ex => ex.Id == id);

        //  if (existing == null)
        // {
        //      context.Users.Add(user);
        //     context.SaveChanges();
        //     return user;
        // }

        // user.Id = id;
        // context.Users.Update(user);
        // context.SaveChanges();
        // return user;
        // }
        public User Upsert(int id, PostUserDto UserPostDto, User addedBy)
        {
            var existing = context.Users.AsNoTracking()
                .Include(u => u.UserUserRoles)
                .ThenInclude(us => us.UserRole)
                .FirstOrDefault(u => u.Id == id);

            if (existing == null)
            {
                User toAdd = PostUserDto.ToUser(UserPostDto);
                context.Users.Add(toAdd);
                context.SaveChanges();
                return toAdd;
            }

            String existingCurrentRole = GetLatestUserUserRole(existing.UserUserRoles).UserRole.Name;
            String addedByCurrentRole = GetLatestUserUserRole(addedBy.UserUserRoles).UserRole.Name;

            UserUserRole currentUserUserRole = GetLatestUserUserRole(existing.UserUserRoles);

            User toUpdate = PostUserDto.ToUser(UserPostDto);
            toUpdate.Password = existing.Password;
            toUpdate.CreatedAt = existing.CreatedAt;
            toUpdate.Id = id;

            if (existingCurrentRole.Equals(RoleConstants.USER_MANAGER) && addedByCurrentRole.Equals(RoleConstants.USER_MANAGER) && addedBy.CreatedAt.AddMonths(6) >= DateTime.Now)
            {
                return null;
            }
            if (((!existingCurrentRole.Equals(RoleConstants.ADMIN) || 
                (!existingCurrentRole.Equals(RoleConstants.USER_MANAGER)) 
                && (addedByCurrentRole.Equals(RoleConstants.USER_MANAGER) || addedByCurrentRole.Equals(RoleConstants.ADMIN)))) ||
                (existingCurrentRole.Equals(RoleConstants.USER_MANAGER) 
                && addedByCurrentRole.Equals(RoleConstants.USER_MANAGER) 
                && addedBy.CreatedAt.AddMonths(6) <= DateTime.Now))
            {
                toUpdate.UserUserRoles = existing.UserUserRoles;
                context.Users.Update(toUpdate);
                context.SaveChanges();
                context.Users.Attach(toUpdate);

                if (existingCurrentRole != UserPostDto.UserRole)
                {
                    IEnumerable<UserRole> allRoles = context.UserRoles;
                    List<String> list = new List<string>();
                    foreach (UserRole role in allRoles)
                    {
                        list.Add(role.Name);
                    }
                    if (list.Contains(UserPostDto.UserRole))
                    {
                        UserRole role = SearchForRoleByTitle(UserPostDto.UserRole);
                        UserUserRole history = new UserUserRole
                        {
                            UserRole = role,
                            StartTime = DateTime.Now
                        };

                        currentUserUserRole.EndTime = DateTime.Now;

                        context.UserRoles.Attach(role);
                        toUpdate.UserUserRoles.Add(history);
                        context.SaveChanges();
                    }
                    else
                    {
                        return null;
                    }
                }

                return toUpdate;
            }
            return null;
        }

        public List<UseUserRoleGetModel> GetAllRoles(int id)
        {
            List<UseUserRoleGetModel> result = new List<UseUserRoleGetModel>();
            var list = context.UserUserRoles.Include(u => u.UserRole).Include(u => u.User)
                .Where(u => u.UserId == id).OrderBy(r => r.StartTime);
            foreach (UserUserRole us in list)
            {
                result.Add(UseUserRoleGetModel.ToUseUserRoleGetModel(us));
            }

            return result;
        }

        private UserUserRole GetLatestUserUserRole(IEnumerable<UserUserRole> allHistory)
        {
            var latestHistoryUserRole = allHistory.OrderByDescending(x => x.StartTime).
                FirstOrDefault();

            if (latestHistoryUserRole.EndTime == null)
            {
                return latestHistoryUserRole;
            }

            return null;
        }


        private UserRole SearchForRoleByTitle(String title)
        {
            IEnumerable<UserRole> roles = context.UserRoles;
            foreach (UserRole role in roles)
            {
                if (role.Name.Equals(title))
                {
                    return role;
                }
            }
            return null;
        }
    }

}



