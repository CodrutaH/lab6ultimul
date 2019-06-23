using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2.DTOs;
using Lab2.Models;
using Lab2.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers
{
    [Authorize]
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginPostDto login)
        {
            var user = userService.Authenticate(login.Username, login.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        //[HttpPost]
        public IActionResult Register([FromBody]RegisterUserPostDto registerModel)
        {
            var user = userService.Register(registerModel);
            if (user == null)
            {
                return BadRequest(user);
            }
            return Ok();
        }


        [AllowAnonymous]
        [HttpGet("roles/{id}", Name = "GetAllRoles")]
        public List<UseUserRoleGetModel> GetAllRoles(int id)
        {
            List<UseUserRoleGetModel> roles = userService.GetAllRoles(id);

            return roles;
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "User_Manager, Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = userService.GetAll();
            return Ok(users);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Regular, User_Manager")]
        // GET: api/Users/5
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUser(int id)
        {
            var existing = userService.GetById(id);

            if (existing == null)
            {
                return NotFound();
            }

            return Ok(existing);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userNew"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,User_Manager")]
        [HttpPost]
        public IActionResult Post([FromBody] RegisterUserPostDto userNew)
        {
         
            var user = userService.Create(userNew);
            if (user == null)
            {
                return BadRequest(user);
            }
            return Ok();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userNew"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,User_Manager")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PostUserDto userNew)
        {
            User addedBy = userService.GetCurrentUser(HttpContext);
            var result = userService.Upsert(id, userNew, addedBy);
            return Ok(result);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,User_Manager")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
           User addedBy = userService.GetCurrentUser(HttpContext);
            var result = userService.Delete(id,addedBy);
           // var result = userService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        

        
    }
}