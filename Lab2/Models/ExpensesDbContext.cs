using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Models
{
    public class ExpensesDbContext : DbContext
    {
        public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}
