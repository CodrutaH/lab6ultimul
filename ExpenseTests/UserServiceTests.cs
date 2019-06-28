using Lab2.DTOs;
using Lab2.Models;
using Lab2.Servies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    public class UserServiceTests
    {
        private IOptions<AppSettings> config;
        private User User;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "dsadhjcghduihdfhdifd8ih"
            });
        }

        /// <summary>
        /// TODO: AAA - Arrange, Act, Assert
        /// </summary>
        [Test]
        public void ValidRegisterShouldCreateNewUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
               .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateNewUser))// "ValidRegisterShouldCreateNewUser")
               .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var usersService = new UserService(context,null,config);
                var added = new RegisterUserPostDto
                {
                    FullName = "fdsfsdfs",
                    Username = "test_username",
                    Email = "a@a.b",
                    Password = "1234567",
                };
                var result = usersService.Register(added);

                Assert.IsNotNull(result);
                
            }
        }

        [Test]
        public void AuthenticateShouldLoginAUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(AuthenticateShouldLoginAUser))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var usersService = new UserService(context,null,config);
                var added = new RegisterUserPostDto
                //

                {
                    FullName = "codruta",
                    Username = "ioana",
                    Email = "maria@gmail.com",
                    Password = "1234567"
                };
                var result = usersService.Register(added);
                var authenticated = new LoginPostDto
                {
                    Username = "ioana",
                    Password = "1234567"
                };
                var authresult = usersService.Authenticate(added.Username, added.Password);

                Assert.IsNotNull(authresult);
               //Assert.AreEqual(5, authresult.Id);
                Assert.AreEqual(authenticated.Username, authresult.Username);
            }
        }
        [Test]
        public void GetAllShouldReturnAllUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnAllUser))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var usersService = new UserService(context, null, config);
                var added1 = new RegisterUserPostDto

                {
                    FullName = "codruta1",

                    Email = "maria@gmail.com",
                    Username = "ioana",
                    Password = "1234567"
                };
                var added2 = new RegisterUserPostDto

                {
                    FullName = "codruta3",

                    Email = "maria@gmail.com",
                    Username = "ioana",

                    Password = "1234567"
                };

                usersService.Register(added1);
                usersService.Register(added2);

                int numberOfElements = usersService.GetAll().Count();

                Assert.NotZero(numberOfElements);


            }
        }
            [Test]
        public void DeleteShouldDeleteUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
             .UseInMemoryDatabase(databaseName: nameof(DeleteShouldDeleteUser))
             .Options;

            using (var context = new ExpensesDbContext(options))
            {

                var userService = new UserService(context, null, config);

                var newUser = new RegisterUserPostDto
                {
                    Email = "alina3@yahoo.com",
                   
                    FullName = "dana",
                    Password = "1234567",
                    Username = "alina3"
                };

                userService.Register(newUser);

                User addedUser = context.Users.Last();

                context.Entry(addedUser).State = EntityState.Detached;

                //var addedUser = context.Users.Where(u => u.Username == "alina3").FirstOrDefault();

                userService.Delete(addedUser.Id,User);

                int users = userService.GetAll().Count();

                Assert.Zero(users);
            }
        }

    }
}