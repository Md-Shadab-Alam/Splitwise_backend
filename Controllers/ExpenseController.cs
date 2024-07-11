using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Splitwise.Data;
using Splitwise.Entities;
using Splitwise.Services;
using Splitwise.Utility;
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
        private readonly RabbitMQService _rabbitMQService;
        public ExpenseController(SplitwiseDbContext context,IExpenseService expenseService, RabbitMQService rabbitMQService)
        {
            _context = context;
            _expenseService = expenseService;
            _rabbitMQService = rabbitMQService;
        }


        [HttpGet]
    //    [Authorize]
        public async Task<IActionResult> GetExpenseAsync()
        {
           return Ok(await  _expenseService.GetExpenseAsync());
        }


        [HttpGet("GroupId")]
      //  [Authorize]
        public async Task<IActionResult> GetExpenseByGroup(int groupId)
        {
            var exp = await _expenseService.GetExpenseByGroup(groupId);
            return Ok(exp);
        }

        [HttpPost]
      //  [Authorize(Roles = StorageData.Role_Admin)]
        public async Task<IActionResult> AddExpense([FromBody] Expense expense, [FromQuery] int[] selectedUsersId, [FromQuery] int userPaidId)
        {
            var expenseJson = JsonConvert.SerializeObject(expense);
            _rabbitMQService.SendMessage(expenseJson);
            return Ok(await _expenseService.AddExpenseAsync(expense, selectedUsersId, userPaidId));
     
        }

    }
}
public class AdminExpenseApprovalService
{
    private readonly RabbitMQService _rabbitMQService;
    public AdminExpenseApprovalService()
    {
        _rabbitMQService = new RabbitMQService();
    }

    public void StartListening()
    { 
        _rabbitMQService.ConsumeExpenses(ProcessExpense); 
    }
    
    private void ProcessExpense (string expenseMessage)
    {
        // Example: Deserialize and process expense approval logic
        var expense = JsonConvert.DeserializeObject<Expense>(expenseMessage);
        // Example: Check if user is admin (use JWT authentication)// Example: Approve or reject expense// Example: Publish approval status to admin-approval exchange
    }
}

