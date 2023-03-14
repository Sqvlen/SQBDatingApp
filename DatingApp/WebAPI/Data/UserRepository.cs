using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Helpers;
using WebAPI.DataTransferObjects;
using WebAPI.Interfaces;

namespace WebAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly IMapper _mapper;

        public UserRepository(DataBaseContext dataBaseContext, IMapper mapper)
        {
            this._dataBaseContext = dataBaseContext;
            this._mapper = mapper;
        }

        public void Update(AppUser user)
        {
            _dataBaseContext.Entry(user).State = EntityState.Modified;
        }
        
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _dataBaseContext.Users.Include(user => user.Photos).ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _dataBaseContext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _dataBaseContext.Users.Include(user => user.Photos).SingleOrDefaultAsync(user => user.UserName == username);
        }

        public async Task<PageList<MemberDTO>> GetMembersAsync(UserParams userParams)
        {
            var query = _dataBaseContext.Users.AsQueryable();

            query = query.Where(user => user.UserName != userParams.CurrentUsername);
            query = query.Where(user => user.Gender != userParams.Gender);

            var minDateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query = query.Where(user => user.DateOfBirth >= minDateOfBirth && user.DateOfBirth <= maxDateOfBirth);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderBy(user => user.Created),
                _ => query.OrderByDescending(user => user.LastActive)
            };

            return await PageList<MemberDTO>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDTO>(_mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<MemberDTO> GetMemberByUserNameAsync(string username)
        {
            return await _dataBaseContext.Users.Where(user => user.UserName == username)
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }

        public async Task<MemberDTO> GetMemberByIdAsync(int id)
        {
            return await _dataBaseContext.Users.Where(user => user.Id == id)
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }

        public async Task<string> GetUserGender(string username)
        {
            return await _dataBaseContext.Users
                .Where(user => user.UserName == username)
                .Select(user => user.Gender).FirstOrDefaultAsync();
        }
    }
}
