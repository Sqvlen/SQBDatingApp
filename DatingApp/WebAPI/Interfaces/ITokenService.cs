using WebAPI.Entities;

namespace WebAPI.Interfaces
{
    public interface ITokenService
    {
        public Task<string> CreateToken(AppUser user);
    }
}
