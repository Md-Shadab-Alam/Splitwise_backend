using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Entities;
using Splitwise.Services;
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
        public UsersController(SplitwiseDbContext context, IUsersService usersServices)
        {
            _context = context;
            _usersServices = usersServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _usersServices.GetUsers());
        }


        [HttpGet("GroupId")]
        public async Task<IActionResult>GetUserByGroup(int groupId)
        {
            var user = await _usersServices.GetUserByGroup(groupId);
           return Ok(user);
        }


        [HttpGet("Name")]
        public async Task<IActionResult> GetUserByNameAsync(string name)
        {
            var user = await _usersServices.GetUserByNameAsync(name);
            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] Users user)
        {
           return Ok(await _usersServices.CreateUserAsync(user));
        }


        [HttpPut("id")]
        public async Task<IActionResult> EditUser([FromBody] Users user, int id)
        {
            return Ok(await _usersServices.EditUser(user, id));
        }

        
    

    }
}
