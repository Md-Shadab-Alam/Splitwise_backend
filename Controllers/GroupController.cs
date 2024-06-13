using Microsoft.AspNetCore.Mvc;
using Splitwise.Entities;
using Splitwise.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Splitwise.Services;

namespace Splitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly SplitwiseDbContext _context;
        private readonly IGroupService _groupService;
        public GroupController(SplitwiseDbContext context, IGroupService groupService)
        {
            _context = context;
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroup()
        {
            return Ok(await _groupService.GetAllGroup());
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _groupService.GetGroupByIdAsync(id));
        }


        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] Group group)
        { 
            return Ok( await _groupService.CreateGroup(group));
        }

        [HttpPost("groupId")]
        public async Task<IActionResult> AddUsersInGroup(Users users, int groupID)
        {
            return Ok(await _groupService.AddUsersInGroup(users, groupID));
        }


        [HttpPut("id")]
        public async Task<IActionResult> EditGroupDetail([FromBody] GroupDetail groupDetail, int id)
        {
            return Ok(await _groupService.EditGroupDetail(groupDetail, id));
        }

    }
}


