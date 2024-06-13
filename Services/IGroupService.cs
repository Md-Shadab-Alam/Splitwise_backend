using Splitwise.Entities;

namespace Splitwise.Services
{
    public interface IGroupService
    {
        public Task<Response> GetAllGroup();
        public Task<Response> GetGroupByIdAsync(int id);
        public Task<Response> CreateGroup(Group group);
        public Task<Response> AddUsersInGroup(Users users, int groupId);
        public Task<Response> EditGroupDetail(GroupDetail groupDetail, int id);
    }
}
