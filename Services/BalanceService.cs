using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Entities;

namespace Splitwise.Services
{
    public class BalanceService : IBalanceService
    {

        private readonly SplitwiseDbContext _context;
        public BalanceService(SplitwiseDbContext context)
        {
            _context = context;
        }

        public async Task<Response> GetBalances()
        {
            Response res = new Response();
            res.Status = true;

            res.Data = await _context.Balances.ToListAsync();
            res.Message = "Balance fetched successfully";
            return res;
        }
        public async Task<Response> GetBalanceByUser(int userId)
        {
            Response res = new Response();
            res.Status = true;
            res.Data = await _context.Balances.FirstOrDefaultAsync(e => e.UsersId == userId);
            res.Message = "Balance fetched successfully";
            return res;
        }

        public async Task<Response> AddBalanceForUserInvolved(int userId, decimal IndividualAmount)
        {
            Response res = new Response();
            res.Status = true;

            var balances = await _context.Balances
                .FirstOrDefaultAsync(b => b.UsersId == userId);

            if (balances == null)
            {
                balances = new Balance()
                {
                    UsersId = userId,
                    Amount = -IndividualAmount

                };

                _context.Balances.AddAsync(balances);
            }
            else
            {
                balances.Amount -= IndividualAmount;

            }

            await _context.SaveChangesAsync();
            res.Data = balances;
            res.Message = "Balance added Successfully";
            return res;
        }

        public async Task<Response> AddBalanceForUserPaid(int userId, decimal amount)
        {
            Response res = new Response();
            res.Status = true;

            var balances = await _context.Balances.FirstOrDefaultAsync(b => b.UsersId == userId);
            if (balances == null)
            {
                balances = new Balance()
                {
                    UsersId = userId,
                    Amount = amount
                };
                _context.Balances.AddAsync(balances);
            }
            else
            {
                balances.Amount += amount;
            }
            await _context.SaveChangesAsync();

            res.Data = balances;
            res.Message = "Balance added Successfully";
            return res;
        }

        public async Task<Response> Calculation(int expid)
        {
            Response res = new Response();
            res.Status = true;

            

            Expense expense = await _context.Expenses
                                .Include(u => u.ExpenseDetails)
                                .Include(u => u.UsersInvolved)
                                .Include(u => u.UsersPaid)
                                .FirstOrDefaultAsync(e => e.ExpenseId == expid);

            if (expense == null)
            {
                res.Message = "Expense doesn't exist. Please enter valid expense.";
                res.Data = expense;
                return res;
            }
            var numberofusers = expense.UsersInvolved.Count();
            var amount = expense.ExpenseDetails.Amount;

            var individualAmount = amount / numberofusers;


            //add balance of userPaid
            foreach (var user in expense.UsersPaid)
            {
                var addBalanceResponseForUserPaid = await AddBalanceForUserPaid(user.UsersId, amount);
                if (addBalanceResponseForUserPaid.Status)
                {
                    var balance = addBalanceResponseForUserPaid.Data;
                    res.Message = "Balance added successfully for user" + user.UsersId;
                }
                else
                {
                    res.Status = false;
                    res.Message = "Error in adding Balance";
                }
            }

            //add balance of userInvolved
            foreach (var user in expense.UsersInvolved)
            {
                var addBalanceResponseForUserInvolved = await AddBalanceForUserInvolved(user.UsersId, individualAmount);
                if (addBalanceResponseForUserInvolved.Status)
                {
                    var balance = addBalanceResponseForUserInvolved.Data;
                    res.Message = "Balance added successfully for user" + user.UsersId;
                }
                else
                {
                    res.Status = false;
                    res.Message = "Error in adding Balance";
                }
            }
            var trans = OptimizeTransactions(expense.GroupId);
          //  res.Data = expense;
          res.Data = trans;
            return res;

        }

        public async Task<Response> OptimizeTransactions(int groupId)
        {
            Response res =  new Response();

            Group group =await _context.Groups.Include(e=>e.Users).Include(e=>e.GroupDetails)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);
            var users = group.Users;

            var bal = _context.Balances.ToList();

            List<Transaction> transactions1 = new List<Transaction>();

            Dictionary<int, decimal> Pos = new Dictionary<int, decimal>();
            Dictionary<int, decimal> Neg = new Dictionary<int, decimal>();

            foreach (var user in users)
            {
                var userBal = bal.FirstOrDefault(b => b.UsersId == user.UsersId);
                if (userBal != null)
                {
                    if (userBal.Amount > 0)
                    {
                        Pos[user.UsersId] = userBal.Amount;
                    }
                    else
                    {
                        Neg[user.UsersId] = userBal.Amount;
                    }
                }
            }
            foreach (var Payer in Pos)
            {
                foreach (var Receiver in Neg)
                {
                    decimal amount = Math.Min(Math.Abs(Payer.Value), Math.Abs(Receiver.Value));
                  
                    transactions1.Add(new Transaction { Payer = Payer.Key, Receiver = Receiver.Key, Amount = amount });

                    // Update balance
                    Pos[Payer.Key] -= amount;
                    Neg[Receiver.Key] = amount;

                    //Remove or update entries if fullly settle

                    //if (Pos[Payer.Key] == 0)
                    //{
                    //    Pos.Remove(Payer.Key);
                    //}
                    //if (Neg[Receiver.Key] == 0) 
                    //{ 
                    //    Neg.Remove(Receiver.Key);
                    //}
                }
            }
            res.Data = transactions1;
            return res;
            //return transactions1;


        }
    }
}