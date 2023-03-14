using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DataTransferObjects;
using WebAPI.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")] // POST - api/account/register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username))
                return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.Username;

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);
            
            return new UserDTO
            {
                Username = registerDTO.Username.ToLower(),
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }

        [HttpPost("login")] // POST - api/account/login
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.Users.Include(user => user.Photos).SingleOrDefaultAsync<AppUser>(user => user.UserName == loginDTO.Username);
            if (user == null)
                return Unauthorized("Invalid Username");

            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!result)
                return Unauthorized("Invalid Password");

            return new UserDTO
            {
                Username = loginDTO.Username.ToLower(),
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> UserExists(string username)
        {
            bool isExists = await _userManager.Users.AnyAsync(user => user.UserName.ToLower() == username.ToLower());

            return isExists;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
