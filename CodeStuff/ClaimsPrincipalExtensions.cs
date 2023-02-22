using System.Security.Claims;

namespace CodeStuff;

public static class ClaimsPrincipalExtensions
{
    public static string? FullName(this ClaimsPrincipal principal)
        => principal.HasClaim(c => c.Type == ClaimTypes.GivenName) &&
           principal.HasClaim(c => c.Type == ClaimTypes.Surname)
            ? $"{principal.FindFirstValue(ClaimTypes.GivenName)} {principal.FindFirstValue(ClaimTypes.Surname)}"
            : null;
}