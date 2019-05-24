using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.DTOs;
using Lab2.Servies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentService commentService;

        public CommentsController(ICommentService service)
        {
            this.commentService = service;
        }

        [HttpGet]
        public IEnumerable<GetCommentsDto> GetComments(string text = "")
        {
            return commentService.GetComments(text);
        }
    }
}