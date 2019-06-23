using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Models
{
    public class RoleDbSeeder
    {
        public static void Initialize(ExpensesDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.UserRoles.Any())
            {
                return;   // DB has been seeded
            }

            context.UserRoles.AddRange(
                new UserRole
                {
                    Name = RoleConstants.REGULAR,
                },

                                new UserRole
                                {
                                    Name = RoleConstants.USER_MANAGER,
                                },

                                new UserRole
                                {
                                    Name = RoleConstants.ADMIN,
                                }

            );

            context.SaveChanges(); // commit transaction
        }
    }
}
