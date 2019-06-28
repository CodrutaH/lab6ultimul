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
    class ExpenseServiceTest
    {
        private IOptions<AppSettings> config;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "asdfg"
            });
        }

        [Test]
        public void GetAll()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
             .UseInMemoryDatabase(databaseName: nameof(CreateANewExpense))// "CreateANewTask")
             .Options;
            using (var context = new ExpensesDbContext(options))
            {
                var expenseService = new ExpenseService(context);

                User user = new User
                {
                    Username = "ALDLSLADLAJDSA"
                };

               PostExpenseDto expense = new PostExpenseDto
                {
                    Description = "tdssgafgadfla"
                };
                PostExpenseDto expense2 = new PostExpenseDto
                {
                    Description = "Alallalalalaal"
                };
                PostExpenseDto expense3 = new PostExpenseDto
                {
                    Description = "fdsjgaoidsfhgasidl"
                };
                expenseService.Create(expense, user);
                expenseService.Create(expense2, user);
                expenseService.Create(expense3, user);
                var populated = expenseService.GetAll(1);
                var empty = expenseService.GetAll(2);
                Assert.AreEqual(4, populated.Entries.Count);
                Assert.Zero(empty.Entries.Count);

            }
        }

        [Test]
        public void CreateANewExpense()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateANewExpense))// 
              .Options;
            using (var context = new ExpensesDbContext(options))
            {
                var expenseService = new ExpenseService(context);
                Expense expense = new Expense();
                User user = new User
                {
                    Username = "ALDLSLADLAJDSA"
                };
                var create = new Lab2.DTOs.PostExpenseDto
                {
                   
                    Description = "Read1",
                    Sum = 3,
                    Location  = "Read1",
                    Date = DateTime.Now.AddDays(15),
                    Currency = "ADF",
                    Type = "FOOD",
                   // ClosedAt = null,
                   // Importance = "Medium",
                   // State = "InProgress",
                    Comments = expense.Comments

                     
                   
                    
                    

                   
                    
                };
                var result = expenseService.Create(create, user);
                Assert.NotNull(result);
                Assert.AreEqual(create.Description, result.Description);
            }

        }

        [Test]
        public void DeleteExistingExpense()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteExistingExpense))
              .Options;
            using (var context = new ExpensesDbContext(options))
            {
                var expenseService = new ExpenseService(context);
                User user = new User
                {
                    Username = "ALDLSLADLAJDSA"
                };
                var result = new Lab2.DTOs.PostExpenseDto
               
                {
                 
                  
                    //Added = DateTime.Now,
                   // Deadline = DateTime.Now.AddDays(15),
                   // ClosedAt = null,
                   
                     Description = "Read1",
                    Sum = 3,
                    Location = "Read1",
                    Date = DateTime.Now.AddDays(15),
                    Currency = "ADF",
                    Type = "FOOD",
                    // ClosedAt = null,
                    // Importance = "Medium",
                    // State = "InProgress",
                    Comments = null

                };
                Expense saveexpense = expenseService.Create(result, user);
                expenseService.Delete(saveexpense.Id);

                Assert.IsNull(expenseService.GetById(saveexpense.Id));
            }
        }

        [Test]
        public void UpdateExistingExpense()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(UpdateExistingExpense))
              .Options;
            using (var context = new ExpensesDbContext(options))
            {
                var expenseService = new ExpenseService(context);
                var resultPostExpenseDto = new Lab2.DTOs.PostExpenseDto
               
                {
                    
                    Description = "Read3",
                    Sum = 3,
                    Location = "Read3",
                    Date = DateTime.Now,
                    Currency ="ADF",
                    Type ="FOOD",
                    Comments = null
                 
                                          
                    
                };

                var user = new User
                {
                   
                    FullName = "Baggings",
                    Email = "frodo@yahoo.com",
                    Username = "frodo",
                    Password = "lalala",
                    CreatedAt = DateTime.Now
                };
                var lalala = expenseService.Create(resultPostExpenseDto, user);

                List<Comment> comments = new List<Comment>();
                Comment comment = new Comment
                {
                    Id = 1,
                    Text = "One Ring to Rule them All",
                    Important = true
                };
                comments.Add(comment);

                var dateDeadline = DateTime.Now.AddDays(20);

                var resultExpense = new Expense
                {
                    Description = "Read4",
                    Sum = 3,
                    Location = "Read4",
                    Date = DateTime.Now,
                    Currency = "ADF",
                    Comments = null
                };
                context.Entry(lalala).State = EntityState.Detached;
                
                            
                Assert.AreEqual(resultExpense.Location, "Read4");
                Assert.AreEqual(resultExpense.Description, "Read4");
                Assert.AreEqual(resultExpense.Currency, "ADF");
                Assert.AreEqual(resultExpense.Comments, null);
            }
        }
    }
}
