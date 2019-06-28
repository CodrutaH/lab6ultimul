using Lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.DTOs
{
    public class CommentPostDto
    {


        public int Id { get; set; }

        public string Text { get; set; }

        public bool Important { get; set; }





        public static Comment ToComment(CommentPostDto comment)
        {
            return new Comment
            {
                Id = comment.Id,
                Text = comment.Text,
                Important = comment.Important,
            };
        }
    }
}