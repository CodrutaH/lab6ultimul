using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Models
{
    public class ExpensesDbSeeder
    {
        public static void Initialize(ExpensesDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Expenses.Any())
            {
                return;
            }

            context.Expenses.AddRange(
                new Expense
                {
                    Description = "shopping",
                    Sum = 100,
                    Location = "Cluj",
                    Date = new DateTime(2018, 05, 10, 23, 0, 0),
                    Currency = "RON",
                    Type = TypeEnum.Clothes
                },
                new Expense
                {
                    Description = "food",
                    Sum = 50,
                    Location = "Cluj",
                    Date = new DateTime(2018, 05, 11, 09, 0, 0),
                    Currency = "RON",
                    Type = TypeEnum.Food
                }
            );
            context.SaveChanges();
        }
    }
}
