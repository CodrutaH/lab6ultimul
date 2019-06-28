using Lab2.Models;
using Lab2.Servies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ExpenseTests
{
    class CommentsServiceTest
    {
        private IOptions<AppSettings> config;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "ASD"
            });
        }

        [Test]
        public void GetAllShouldReturnCorrectNumberOfPages()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPages))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {

                var commentsService = new CommentsService(context);
                var expenseService = new ExpenseService(context);
                var addedExpense = expenseService.Create(new Lab2.DTOs.PostExpenseDto
                {
                    Description = "fdsfsd",
                    Date = new DateTime(),
                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "asd",
                            Owner = null
                        }
                    },
                    Currency = "large",
                    Sum = 5,
                    Type = "food",
                    Location = "aaa"
                }, null);

                var allComments = commentsService.GetComments(1, string.Empty);
                Assert.AreEqual(1, allComments.NumberOfPages);
            }
        }
            public void ValidGetAllShouldDisplayAllComments()
            {
                var options = new DbContextOptionsBuilder<ExpensesDbContext>()
                  .UseInMemoryDatabase(databaseName: nameof(ValidGetAllShouldDisplayAllComments))
                  .Options;

                using (var context = new ExpensesDbContext(options))
                {
                    var commentsService = new CommentsService(context);

                    var added = new Lab2.DTOs.CommentPostDto

                    {
                        Text = "Write a Book",
                        Important = true,

                    };
                    var added2 = new Lab2.DTOs.CommentPostDto

                    {
                        Text = "Read a Book",
                        Important = false,

                    };

                    commentsService.Create(added);
                    commentsService.Create(added2);

                    var result = commentsService.GetAll(string.Empty);
                   // Assert.AreEqual(0, result.Count());

                }
            }

    }
}
