using Lab2.DTOs;
using Lab2.Models;
using Lab2.Servies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTests
{
    class UserRoleServiceTests

    {
        private IOptions<AppSettings> config;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "My armor is contempt, my shield is disgust, my sword is hatred, in the Emperor's name let none survive"
            });
        }

        [Test]
        public void Create()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(Create))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var roleService = new UserRoleService(context, config);

                var toAdd = new UserRolePostDto
                {
                    Name = "GOD"
                };

                UserRole role = roleService.Create(toAdd);

                Assert.AreEqual(toAdd.Name, role.Name);
                Assert.IsNotNull(roleService.GetById(role.Id));
            }
        }

        [Test]
        public void Upsert()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(Upsert))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var roleService = new UserRoleService(context, config);
                var toAdd = new UserRolePostDto
                {
                    Name = "GOD"
                };

                var toUpdate = new UserRolePostDto
                {
                    Name = "Sclave Master"
                };

                UserRole role = roleService.Create(toAdd);
                context.Entry(role).State = EntityState.Detached;
                UserRole updated = roleService.Upsert(role.Id, toUpdate);

                Assert.AreNotEqual(role.Name, updated.Name);
            }
        }

        [Test]
        public void Delete()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(Delete))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var roleService = new UserRoleService(context, config);
                var toAdd = new UserRolePostDto
                {
                    Name = "GOD"
                };
                UserRole role = roleService.Create(toAdd);
                roleService.Delete(role.Id);

                Assert.IsNull(context.UserRoles.Find(role.Id));
            }
        }

        [Test]
        public void GetById()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetById))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var roleService = new UserRoleService(context, config);
                var toAdd = new UserRolePostDto
                {
                    Name = "GOD"
                };
                UserRole expected = roleService.Create(toAdd);
                UserRole actual = roleService.GetById(expected.Id);

                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void GetAll()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAll))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var roleService = new UserRoleService(context, config);
                var first = new UserRolePostDto
                {
                    Name = "God"
                };
                var second = new UserRolePostDto
                {
                    Name = "Slave Master"
                };
                var third = new UserRolePostDto
                {
                    Name = "Matroana"
                };
                var fourth = new UserRolePostDto
                {
                    Name = "Shobolan"
                };
                var fifth = new UserRolePostDto
                {
                    Name = "Tod Howard"
                };
                roleService.Create(first);
                roleService.Create(second);
                roleService.Create(third);
                roleService.Create(fourth);
                roleService.Create(fifth);
                context.SaveChanges();

                PaginatedList<UserRoleGetDto> populated = roleService.GetAll(1);
                PaginatedList<UserRoleGetDto> empty = roleService.GetAll(2);

                Assert.AreEqual(5, populated.Entries.Count);
                Assert.Zero(empty.Entries.Count);

            }
        }
    }
}