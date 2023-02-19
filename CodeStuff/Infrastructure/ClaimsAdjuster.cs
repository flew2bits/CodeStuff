using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace CodeStuff.Infrastructure;

public class ClaimsAdjuster : IClaimsTransformation
{
    private readonly ILogger<ClaimsAdjuster> _logger;

    public ClaimsAdjuster(ILogger<ClaimsAdjuster> logger)
    {
        _logger = logger;
    }

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        _logger.LogDebug("Checking for user");
        if (principal.Identity?.IsAuthenticated ?? false) return Task.FromResult(principal);
        _logger.LogDebug("Creating new principal/identity");
        var identity = new ClaimsIdentity();
        identity.AddClaim(new Claim(ClaimTypes.Name, "Titus Anderson"));
        identity.AddClaim(new Claim(ClaimTypes.Upn, "flew2bits@gmail.com"));

        var newPrincipal = new ClaimsPrincipal(identity);

        return Task.FromResult(newPrincipal);
    }
}