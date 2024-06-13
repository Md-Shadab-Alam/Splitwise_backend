using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Entities;

namespace Splitwise.Services
{
    public class UsersService : IUsersService
    {
        private readonly SplitwiseDbContext _context;
        public UsersService(SplitwiseDbContext context)
        {
            _context = context;
        }

        public async Task<Response> GetUsers()
        {
            Response res = new Response();
            res.Status = true;

            res.Data = await _context.Users.ToListAsync();
            res.Message = "User fetched successfully";
            return res;
        }


        public async Task<Response>GetUserByGroup(int groupId)
        {
            Response res = new Response();
            res.Status = true;

            var group =await _context.Groups
                .Include(g => g.Users)
                .FirstOrDefaultAsync (g => g.GroupId == groupId);
            if (group == null)
            {
                res.Status = false;
                res.Message = "Group Not Found";
                return res;
            }
            var user = group.Users.ToList();

            res.Data = user;
            res.Message = "User fetched successfully";

            return res;
        }

        public async Task<Response> GetUserByNameAsync(string name)
        {
            Response res = new Response();
            res.Status = true;

            var user = await _context.Users
                .FirstOrDefaultAsync(e => e.Name == name);
            if (user == null)
            {
                res.Status = false;
                res.Message = "User Not Found";
                return res;
            }
            res.Data = user;
            res.Message = "User fetch Successfully";
            return res;
        }

        public async Task<Response> CreateUserAsync(Users user)
        {
            Response res = new Response();
            res.Status = true;

            var existingUser = _context.Users.FirstOrDefault(e => e.Name == user.Name);
            if (existingUser != null)
            {
                res.Status = false;
                res.Message = "User Already Exist";
                return res;
            }
            _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            res.Data = user;
            res.Message = "User Added";
            return res;
        }

        public async Task<Response> EditUser(Users user, int id)
        {
            Response res = new Response();
            res.Status = true;

            Users userdlt = await _context.Users.FindAsync(id);
            if (userdlt == null)
            {
                res.Status = false;
                res.Message = "User Not Found";
                return res;
            }
            userdlt.Name = user.Name;
            userdlt.Email = user.Email;
//            _context.Users.Add(userdlt);
            _context.SaveChanges();
            res.Data = user;
            res.Message = "User Updated";
            return res;
        }
    }
    }

