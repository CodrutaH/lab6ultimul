using Lab2.Models;
using Lab2.Servies;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTests
{
    class CommentsServiceTest
    {


        [Test]
        public void GetAllShouldReturnCorrectNumberOfPages()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPages))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {

                var commentService = new CommentsService(context);
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
                    Type ="food",
                    Location = "aaa"
                }, null);

                var allComments = commentService.GetAll(1, string.Empty);
                Assert.AreEqual(1, allComments.NumberOfPages);
            }
        }

    }
}
