using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Entities;
using Splitwise.Services;
using Splitwise.Utility;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Splitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly SplitwiseDbContext _context;
        private readonly IUsersService _usersServices;
        private readonly RabbitMQService _rabbitmqService;
        public UsersController(SplitwiseDbContext context, IUsersService usersServices, RabbitMQService rabbitmqService)
        {
            _context = context;
            _usersServices = usersServices;
            _rabbitmqService = rabbitmqService;
        }


        [HttpGet]
      //  [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _usersServices.GetUsers());
        }


        [HttpGet("GroupId")]
    //    [Authorize]
        public async Task<IActionResult>GetUserByGroup(int groupId)
        {
            var user = await _usersServices.GetUserByGroup(groupId);
           return Ok(user);
        }


        [HttpGet("UserId")]
        public async Task<Response> GetUserByIds([FromQuery] int[] userIds)
        {
            Response response = new Response();
            response.Status = true;
            var users = await _context.Users.Where(u => userIds.Contains(u.UsersId)).ToListAsync();
            if (users == null)
            {
                response.Status = false;
                response.Message = "No user found..";
                return response;
            }
            response.Status = true;
            response.Message = "User fetched Successfully..";
            response.Data = users;
            return response;
        }


        [HttpGet("Name")]
    //    [Authorize]
        public async Task<IActionResult> GetUserByNameAsync(string name)
        {
            var user = await _usersServices.GetUserByNameAsync(name);
            return Ok(user);
        }


        [HttpPost]
    //    [Authorize]
        public async Task<IActionResult> CreateUserAsync([FromBody] Users user)
        {
            _rabbitmqService.SendMessage(user);
           return Ok(await _usersServices.CreateUserAsync(user));
        }


        [HttpPut("id")]
     //   [Authorize]
        public async Task<IActionResult> EditUser([FromBody] Users user, int id)
        {
            return Ok(await _usersServices.EditUser(user, id));
        }

        
    

    }
}
