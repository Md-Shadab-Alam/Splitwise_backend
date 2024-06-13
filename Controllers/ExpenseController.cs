using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Splitwise.Data;
using Splitwise.Entities;
using Splitwise.Services;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Splitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : Controller
    {
        private readonly SplitwiseDbContext _context;
        private readonly IExpenseService _expenseService;
        public ExpenseController(SplitwiseDbContext context,IExpenseService expenseService)
        {
            _context = context;
            _expenseService = expenseService;
        }


        [HttpGet]
        public async Task<IActionResult> GetExpenseAsync()
        {
           return Ok(await  _expenseService.GetExpenseAsync());
        }


        [HttpGet("GroupId")]
        public async Task<IActionResult> GetExpenseByGroup(int groupId)
        {
            var exp = await _expenseService.GetExpenseByGroup(groupId);
            return Ok(exp);
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] Expense expense, [FromQuery] int[] selectedUsersId, [FromQuery] int userPaidId)
        {
            return Ok(await _expenseService.AddExpenseAsync(expense, selectedUsersId, userPaidId));
        }

    }
}

