﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Models
{


    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<UserUserRole> UserUserRoles { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
