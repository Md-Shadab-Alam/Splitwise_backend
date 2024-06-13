using Microsoft.AspNetCore.Mvc;
using Splitwise.Entities;

namespace Splitwise.Services
{
    public interface IUsersService
    {
        public Task<Response> GetUsers();
        public Task<Response> GetUserByGroup(int groupId);
        public Task<Response> GetUserByNameAsync(string name);
        public Task<Response> CreateUserAsync(Users user);
        public Task<Response> EditUser(Users user, int id);

    }
}
