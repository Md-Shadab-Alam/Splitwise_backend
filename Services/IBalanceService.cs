using Splitwise.Entities;

namespace Splitwise.Services
{
    public interface IBalanceService
    {
        public Task<Response> GetBalances();
        public Task<Response> GetBalanceByUser(int userId);
        public Task<Response> AddBalanceForUserInvolved(int userId, decimal IndividualAmount);
        public Task<Response> AddBalanceForUserPaid(int userId, decimal amount);
        public Task<Response> Calculation(int expId);
        public Task<Response> OptimizeTransactions(int groupId);
    }
}
