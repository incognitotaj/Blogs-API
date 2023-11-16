using API.Requests;
using Application.Contracts.Services;
using Application.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/Auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthController(
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager, 
        ITokenService tokenService, 
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    /// <summary>
    /// Login an existing user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
            return Unauthorized();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        return Ok(new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            Username = user.UserName
        });
    }

    /// <summary>
    /// Register / create a new user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUserRequest request)
    {
        var user = new IdentityUser
        {
            Email = request.Email,
            UserName = request.Username,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest();
        }

        return Ok(new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            Username = user.UserName
        });
    }
}
