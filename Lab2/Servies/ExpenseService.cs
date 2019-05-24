using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.DTOs;
using Lab2.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Servies
{
    public class ExpenseService : IExpenseService
    {
        private ExpensesDbContext context;

        public ExpenseService(ExpensesDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<GetExpenseDto> GetAll(DateTime? from = null, DateTime? to = null, TypeEnum? type = null)
        {
            IQueryable<Expense> result = context.Expenses.Include(expense => expense.Comments);

            if (from == null && to == null && type == null)
            {
                return result.Select(expense => GetExpenseDto.DtoFromModel(expense));
            }

            if (from != null)
            {
                result = result.Where(expense => expense.Date >= from);
            }

            if (to != null)
            {
                result = result.Where(expense => expense.Date <= to);
            }

            if (type != null)
            {
                result = result.Where(expense => expense.Type == type);
            }

            return result.Select(expense => GetExpenseDto.DtoFromModel(expense));
        }

        public Expense GetById(int id)
        {
            return context.Expenses.Include(ex => ex.Comments).FirstOrDefault(ex => ex.Id == id);
        }

        public Expense Create(PostExpenseDto expenseDto)
        {
            Expense expenseModel = PostExpenseDto.ModelFromDto(expenseDto);
            context.Expenses.Add(expenseModel);
            context.SaveChanges();
            return expenseModel;
        }

        public Expense Upsert(int id, Expense expense)
        {
            var existing = context.Expenses.AsNoTracking().FirstOrDefault(ex => ex.Id == id);

            if (existing == null)
            {
                context.Expenses.Add(expense);
                context.SaveChanges();
                return expense;
            }

            expense.Id = id;
            context.Expenses.Update(expense);
            context.SaveChanges();
            return expense;
        }

        public Expense Delete(int id)
        {
            var existing = context.Expenses.FirstOrDefault(expense => expense.Id == id);

            if (existing == null)
            {
                return null;
            }

            context.Expenses.Remove(existing);
            context.SaveChanges();
            return existing;
        }
        
    }
}
