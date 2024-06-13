using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Entities;

namespace Splitwise.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly SplitwiseDbContext _context;
        private readonly IBalanceService _balanceService;
        public ExpenseService(SplitwiseDbContext context, IBalanceService balanceService)
        {
            _context = context;
            _balanceService = balanceService;
        }


        public async Task<Response> GetExpenseAsync()
        {
            Response res = new Response();
            res.Status = true;

            var exp = await _context.Expenses
                .Include(e => e.UsersPaid)
                .Include(e => e.UsersInvolved)
                .Include(e => e.ExpenseDetails)
                .ToListAsync();

            res.Data = exp;
            res.Message = "Expense fetched successfully";
            return res;
        }

        public async Task<Response> GetExpenseByGroup(int groupId)
        {
            Response res = new Response();
            res.Status = true;

            var exp = await _context.Expenses
                .Include(e => e.UsersPaid)
                .Include(e => e.UsersInvolved)
                .Include(e => e.ExpenseDetails)
            .Where(e => e.GroupId == groupId).ToListAsync();

            if (exp == null || exp.Count == 0)
            {
                res.Message = "Expense not found";
                return res;
            }
            res.Data = exp;
            res.Message = "Expense fetched successfully";
            return res;
        }


        public async Task<Response> AddExpenseAsync([FromBody]Expense expense, [FromQuery] int[] selectedUserId, [FromQuery] int userPaidId)
        {
            Response res = new Response();
            res.Status = true;

            var groupId = expense.GroupId;
            Group group = await _context.Groups
                .Include(u => u.Users)
                .Include(g => g.GroupDetails)
                .FirstOrDefaultAsync(e => e.GroupId == groupId);

            if (group != null && expense.GroupId != 0)
            {
                //for any group

                var usersFromGroup = group.Users.ToList();
                if (usersFromGroup == null)
                {
                    res.Message = "No user found in the group. Please add individual users";
                    return res;
                }


                expense.UsersInvolved = usersFromGroup
                        .Where(u => selectedUserId.Contains(u.UsersId)).ToList();
                expense.UsersPaid = usersFromGroup
                        .Where(u => u.UsersId == userPaidId).ToList();
            }
            else
            {
                // for individual users
                var usersSelected = _context.Users
                    .Where(u => selectedUserId.Contains(u.UsersId)).ToList();

                var userPaid = _context.Users
                    .Where(u => u.UsersId == userPaidId).ToList();

                expense.UsersInvolved = usersSelected;
                expense.UsersPaid = userPaid;

            }
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();

            res.Data = expense;

            var bal = await _balanceService.Calculation(expense.ExpenseId);           

            return res;
        }
    }
}