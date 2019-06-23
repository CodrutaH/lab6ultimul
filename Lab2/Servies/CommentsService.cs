using Lab2.DTOs;
using Lab2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Servies
{
    public class CommentsService : ICommentService
    {

        private ExpensesDbContext context;

        public CommentsService(ExpensesDbContext context)
        {
            this.context = context;
        }

        public PaginatedList<GetCommentsDto> GetAll(int page, string filterString)
        {
            throw new NotImplementedException();
        }

        // public PaginatedList<GetCommentsDto> GetAll(int page, string filterString)
        // {
        //     throw new NotImplementedException();
        // }

        //public IEnumerable<GetCommentsDto> GetComments(string text)
        //{
        //    IQueryable<GetCommentsDto> result = context.Comments.Select(x => new Comment
        //    {
        //        Id = x.Id,
        //        Text = x.Text,
        //        Important = x.Important,
        //        ExpenseId = (from Expense in context.Expenses
        //                     where Expense.Id == x.ExpenseId
        //                     select Expense.Id).FirstOrDefault()
        //    });
        //    //var result = context.Comments.Select(x 

        //    if (text != null)
        //    {
        //        result = result.Where(comment => comment.Text.Contains(text));
        //    }

        //    return result.Select(comment => GetCommentsDto.DtoFromModel(comment));
        //}

        public PaginatedList<GetCommentsDto> GetComments(int page, string filterString)
        {
            //IQueryable<GetCommentsDto> result = context.Comments.Select(x => new GetCommentsDto
            //{
            //    Id = x.Id,
            //    Text = x.Text,
            //    Important = x.Important,
            //    ExpenseId = (from ex in context.Expenses
            //                 where ex.Comments.Contains(x)
            //                 select ex.Id).FirstOrDefault()
            //});

            //if (text != "")
            //{
            //    result = result.Where(c => c.Text.Contains(text));
            //}

            //return result;
            IQueryable<Comment> result = context
              .Comments
              .Where(c => string.IsNullOrEmpty(filterString) || c.Text.Contains(filterString))
              .OrderBy(c => c.Id)
              .Include(c => c.Expense);
            var paginatedResult = new PaginatedList<GetCommentsDto>();
            paginatedResult.CurrentPage = page;

            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<GetCommentsDto>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<GetCommentsDto>.EntriesPerPage)
                .Take(PaginatedList<GetCommentsDto>.EntriesPerPage);
            paginatedResult.Entries = result.Select(c => GetCommentsDto.DtoFromModel(c)).ToList();

            return paginatedResult;
        }
    }
}
