using WebAPI.DataTransferObjects;
using WebAPI.Entities;
using WebAPI.Helpers;

namespace WebAPI.Interfaces
{
    public interface IUserRepository
    {
        public void Update(AppUser user);
        public Task<IEnumerable<AppUser>> GetUsersAsync();
        public Task<AppUser> GetUserByIdAsync(int id);
        public Task<AppUser> GetUserByUsernameAsync(string name);
        public Task<PageList<MemberDTO>> GetMembersAsync(UserParams userParams);
        public Task<MemberDTO> GetMemberByUserNameAsync(string username);
        public Task<MemberDTO> GetMemberByIdAsync(int id);
        public Task<string> GetUserGender(string username);
    }
}
