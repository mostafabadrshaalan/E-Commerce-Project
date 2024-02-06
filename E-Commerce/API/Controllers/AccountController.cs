using API.DTOs;
using API.ResponseModule;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(
                                 UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 ITokenService tokenService,
                                 IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return NotFound(new ApiResponse(404));

            return new UserDto()
            {
                Email = user.Email,
                DesplayName = user.DisplayName,
                Token = tokenService.CreateToken(user)
            };
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            return new UserDto()
            {
                Email = email,
                DesplayName = user.DisplayName,
                Token = tokenService.CreateToken(user)
            };

        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await userManager.FindByEmailAsync(registerDto.Email);

            if (user is not null)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new[] { "Email is already exists" }
                });

            user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                Address = mapper.Map<Address>(registerDto.Address)
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest("Password is not valid");

            return new UserDto()
            {
                DesplayName = user.DisplayName,
                Email = user.Email
            };
        }

        [Authorize]
        [HttpGet("GetUserAddress")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Include(us => us.Address).FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var mappedAddress = mapper.Map<AddressDto>(user.Address);
            return Ok(mappedAddress);
        }

        [Authorize]
        [HttpPost("UpdateAddress")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Include(us => us.Address).FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            user.Address = mapper.Map<Address>(addressDto);

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(401, "Invalid Updating"));

            var mappedAddress = mapper.Map<AddressDto>(user.Address);

            return Ok(mappedAddress);
        }
    }
}
