using WebAPI.DataTransferObjects;
using WebAPI.Entities;
using WebAPI.Helpers;

namespace WebAPI.Interfaces
{
    public interface ILikesRepository
    {
        public Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);

        public Task<AppUser> GetUserWithLikes(int userId);

        public Task<PageList<LikeDTO>> GetUserLikes(LikesParams likesParams);
    }
}
