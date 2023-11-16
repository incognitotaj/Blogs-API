using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Extensions;

public static class UserManagerExtension
{
    public static async Task<IdentityUser> FindByEmailFromClaimsPrincipalAsync(this UserManager<IdentityUser> input, ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email);

        return await input.Users.SingleOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
    }
}
