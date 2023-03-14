using Microsoft.EntityFrameworkCore;
using WebAPI.DataTransferObjects;
using WebAPI.Entities;
using WebAPI.Extensions;
using WebAPI.Helpers;
using WebAPI.Interfaces;

namespace WebAPI.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataBaseContext _dataBaseContext;
        public LikesRepository(DataBaseContext context)
        {
            _dataBaseContext = context;
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _dataBaseContext.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PageList<LikeDTO>> GetUserLikes(LikesParams likesParams)
        {
            var users = _dataBaseContext.Users.OrderBy(user => user.UserName).AsQueryable();
            var likes = _dataBaseContext.Likes.AsQueryable();

            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.TargetUser);
            }

            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.TargetUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likedUsers = users.Select(user => new LikeDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Age = user.DateOfBirth.CalculateAge(),
                KnownAs = user.KnownAs,
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain).Url,
                City = user.City,

            });

            return await PageList<LikeDTO>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _dataBaseContext.Users
                .Include(user => user.LikedUsers).FirstOrDefaultAsync(user => user.Id == userId);
        }
    }
}