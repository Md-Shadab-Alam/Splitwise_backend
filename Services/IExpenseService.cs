using Microsoft.AspNetCore.Mvc;
using Splitwise.Entities;

namespace Splitwise.Services
{
    public interface IExpenseService
    {
        public  Task<Response> GetExpenseAsync();
        public Task<Response> GetExpenseByGroup(int groupid);
        public Task<Response> AddExpenseAsync(Expense expense, int[] selectedUserId, int userPaidId);

    }
}
