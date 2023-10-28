using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Smapple.Extensions;

public static class ClaimsPrincipalExtension
{
    public static int Id(this ClaimsPrincipal user)
    {
        return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}