using Lab2.DTOs;
using Lab2.Models;
using System;

namespace Lab2.Servies

{     
        public interface IUserRoleService
        {
            PaginatedList<UserRoleGetDto> GetAll(int page, DateTime? from = null, DateTime? to = null);

            UserRole Create(UserRolePostDto role);

            UserRole Upsert(int id, UserRolePostDto rolePostModel);

            UserRole  Delete(int id);

            UserRole GetById(int id);
        }
    }

