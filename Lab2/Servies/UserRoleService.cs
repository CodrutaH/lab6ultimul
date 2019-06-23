using System;
using System.Linq;
using Lab2.DTOs;
using Lab2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Lab2.Servies
{

    public class UserRoleService : IUserRoleService
    {
        private ExpensesDbContext dbcontext;

        private readonly AppSettings appSettings;

        public UserRoleService(ExpensesDbContext context, IOptions<AppSettings> appSettings)
        {
            this.dbcontext = context;
            this.appSettings = appSettings.Value;
        }

        public PaginatedList<UserRoleGetDto> GetAll(int page, DateTime? from = null, DateTime? to = null)
        {
            IQueryable<UserRole> result = dbcontext
                            .UserRole
                            .OrderBy(t => t.Id); ;
            PaginatedList<UserRoleGetDto> paginatedList = new PaginatedList<UserRoleGetDto>();
            paginatedList.CurrentPage = page;

            //if there are more includes use thenInclude


            paginatedList.NumberOfPages = (result.Count() - 1) / PaginatedList<GetExpenseDto>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<UserRoleGetDto>.EntriesPerPage)
                .Take(PaginatedList<UserRoleGetDto>.EntriesPerPage);
            paginatedList.Entries = result.Select(t => UserRoleGetDto.FromRole(t)).ToList();

            return paginatedList;
        }

        public UserRole GetById(int id)
        {
            return dbcontext.UserRoles
                .FirstOrDefault(u => u.Id == id);
        }

        public UserRole Create(UserRolePostDto role)
        {
            UserRole toAdd = UserRolePostDto.ToRole(role);

            dbcontext.UserRoles.Add(toAdd);
            dbcontext.SaveChanges();
            return toAdd;
        }

        public UserRole Upsert(int id, UserRolePostDto rolePostDto)
        {
            var existing = dbcontext.UserRoles.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                UserRole toAdd = UserRolePostDto.ToRole(rolePostDto);
                dbcontext.UserRoles.Add(toAdd);
                dbcontext.SaveChanges();
                return toAdd;
            }

            UserRole toUpdate = UserRolePostDto.ToRole(rolePostDto);
            toUpdate.Id = id;
            toUpdate.UserUserRoles = existing.UserUserRoles;
            dbcontext.UserRoles.Update(toUpdate);
            dbcontext.SaveChanges();
            return toUpdate;
        }


        public UserRole Delete(int id)
        {
            var existing = dbcontext.UserRoles.FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                return null;
            }
            dbcontext.UserRoles.Remove(existing);
            dbcontext.SaveChanges();
            return existing;
        }
    }
}