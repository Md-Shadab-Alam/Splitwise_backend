using Microsoft.EntityFrameworkCore;
using Splitwise.Data;
using Splitwise.Entities;

namespace Splitwise.Services
{
    public class GroupService : IGroupService
    {
        private readonly SplitwiseDbContext _context;
        public GroupService(SplitwiseDbContext context)
        {
            _context = context;
        }

        public async Task<Response> GetAllGroup()
        {
            Response res = new Response();
            res.Status = true;

            var groups = await _context.Groups
                            .Include(u => u.Users)  
                            .Include(e => e.GroupDetails)   
                            .ToListAsync();

            res.Data = groups;
            res.Message = "Group Fetched";
            return res;
        }
        public async Task<Response> GetGroupByIdAsync(int id)
        {
            Response res = new Response();
            res.Status = true;

            var group=  await _context.Groups
                            .Include(u => u.GroupDetails)
                            .Include(e => e.Users)
                            .FirstOrDefaultAsync(a => a.GroupId == id);
            if(group == null)
            {
                res.Status = false;
                res.Message = "Group Not Found";
                return res;
            }
            res.Data = group;
            res.Message = "Group Fetched";
            return res;

        }
        //public async Task<Response> CreateGroup(Group group)
        //{
        //    Response res = new Response();
        //    res.Status = true;

        //    _context.Groups.Add(group);
        //    await _context.SaveChangesAsync();
        //    //return CreatedAtAction(nameof(GetByIdAsync), new { id = group.GroupId }, group);
        //    res.Data = group;
        //    res.Message = "Group Created";
        //    return res;
        //}
        public async Task<Response> CreateGroup(Group group)
        {
            Response response = new Response();
            var existingGroup = await _context.Groups
             .Include(u => u.Users) // Include the Users related to the Group
             .Include(g => g.GroupDetails) // Include the GroupDetails related to the Group
             .FirstOrDefaultAsync(u => u.GroupDetails.Name == group.GroupDetails.Name);
            var usersToAdd = new List<Users>();
            var existingUsersInDb = await _context.Users.ToListAsync();
            if (existingGroup != null)
            {
                response.Status = false;
                response.Message = "Group already exists.";
                return response;
            }
            foreach (var user in group.Users)
            {
                var existingUser = existingUsersInDb.FirstOrDefault(u => u.Name.ToLower() == user.Name.ToLower() && u.Email.ToLower() == user.Email.ToLower());
                if (existingUser != null)
                {
                    usersToAdd.Add(existingUser);
                }
                else
                {
                    usersToAdd.Add(user);
                }
            }
            group.Users = usersToAdd;
            await _context.Groups.AddAsync(group);

            await _context.SaveChangesAsync();
            response.Status = true;
            response.Message = "Group created Successfully";
            response.Data = group;
            return response;

        }

        public async Task<Response> EditGroupDetail(GroupDetail groupDetail, int id)
        {
            Response res = new Response();
            res.Status = true;

            if (id != groupDetail.Id || id == null)
            {
                res.Status = false;
                res.Message = "Group Not Found";
                return res;
            }
            var groupdtl = await _context.GroupDetail.FindAsync(id);
            if (groupdtl == null)
            {
                res.Status = false;
                res.Message = "Group Details Not Found";
                return res;
            }
            groupdtl.Name = groupDetail.Name;
            groupdtl.Description = groupDetail.Description;
            groupdtl.Category = groupDetail.Category;
            await _context.SaveChangesAsync();
            res.Data = groupdtl;
            res.Message = "Group Updated";
            return res;
        }

        public async Task<Response> AddUsersInGroup(Users newuser, int groupID)
        {

            Response res = new Response();
            res.Status = true;

            Group group = await _context.Groups
                .Include(u => u.Users)
                .Include(e=>e.GroupDetails)
                .FirstOrDefaultAsync(e=>e.GroupId== groupID);
            if (group == null)
            {
                res.Status = false;
                res.Message = "Group Not Found";
                return res;
            }

            Users users = await _context.Users.FirstOrDefaultAsync(e=>e.Equals(newuser));
            if(users == null)
            {
                group.Users.Add(newuser);
                _context.Users.AddAsync(newuser);
            }
            else
            {
                if (users != group.Users)
                {
                    group.Users.Add(users);
                }
               
            }
            await _context.SaveChangesAsync();
            res.Data = users;
            res.Message = "User added in the group";
            return res;
        }
    }
}
