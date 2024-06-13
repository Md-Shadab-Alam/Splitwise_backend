using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Entities;
using Splitwise.Services;

namespace Splitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly SplitwiseDbContext _context;
        private readonly IBalanceService _balanceService;

        public BalanceController(SplitwiseDbContext context,
            IBalanceService balanceService)
        {
            _context = context;
            _balanceService = balanceService;
            
        }


        [HttpGet("Balance")]
        public async Task<IActionResult> GetBalances()
        {           
            return Ok(await _balanceService.GetBalances());
        }

        [HttpGet("UserID")]
        public async Task<IActionResult> GetBalanceByUser(int userId)
        {
            return Ok( await _balanceService.GetBalanceByUser(userId));
        }
        [HttpGet("tran")]
        public async Task<IActionResult> OptimizeTransactions(int groupId)
        {
            return Ok(await _balanceService.OptimizeTransactions(groupId));
        }


        [HttpPost("AddBalanceForUserInvolved")]
        public async Task<IActionResult> AddBalanceForUserInvolved(int userId, decimal IndividualAmount)
        {
            return Ok(await _balanceService.AddBalanceForUserInvolved(userId, IndividualAmount));
        }


        [HttpPost("AddBalanceForUserPaid")]
        public async Task<IActionResult> AddBalanceForUserPaid(int userId, decimal amount)
        {
            return Ok(await _balanceService.AddBalanceForUserPaid(userId, amount));
        }

        [HttpPost("expCalculation")]
        public async Task<IActionResult> Calculation(int expId)
        {
            return Ok(await _balanceService.Calculation(expId));
        }
    }

}





