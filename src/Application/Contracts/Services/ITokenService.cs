using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.Services
{
    public interface ITokenService
    {
        string CreateToken(IdentityUser user);
    }
}
