using Application.Contracts.Services;
using Application.Dtos;
using AutoMapper;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/Accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountsController(
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
    /// Get user details
    /// </summary>
    /// <returns></returns>
    [HttpGet("user-info")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManager.FindByEmailFromClaimsPrincipalAsync(User);

        return Ok(new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            Username = user.UserName
        });
    }
}
