using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository 
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUSerByIdAsync(int id);
        Task<AppUser> GetUserByUsername(string username);
        Task<IEnumerable<MemberDto>> GetMemberAsync();
        Task<MemberDto> GetMemberAsync(string user);

    }
}