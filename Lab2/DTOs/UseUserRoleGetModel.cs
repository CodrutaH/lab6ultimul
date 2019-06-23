using Lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.DTOs
{
    public class UseUserRoleGetModel
    {
        public string Username { get; set; }
        public string RoleTitle { get; set; }
        public DateTime? AllocatedAt { get; set; }
        public DateTime? RemovedAt { get; set; }

        public static UseUserRoleGetModel ToUseUserRoleGetModel(UserUserRole userUserRole)
        {
            return new UseUserRoleGetModel
            {
                Username = userUserRole.User.Username,
                RoleTitle = userUserRole.UserRole.Name,
                AllocatedAt = userUserRole.StartTime,
                RemovedAt = userUserRole.EndTime
            };
        }
    }
}
