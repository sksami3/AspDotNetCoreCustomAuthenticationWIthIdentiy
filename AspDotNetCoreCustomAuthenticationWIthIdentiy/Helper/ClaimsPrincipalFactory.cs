using AspDotNetCoreCustomAuthenticationWIthIdentiy.Domain.AuthModel;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

public sealed class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    public ClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
    {

    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        #region Basic (sets cookies)
        try
        {
            var identity = await base.GenerateClaimsAsync(user).ConfigureAwait(false);

            if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Subject))
            {
                var sub = user.UserName;
                var email = user.Email;

                identity.AddClaim(new Claim(JwtClaimTypes.Subject, sub));
                identity.AddClaim(new Claim(JwtClaimTypes.Email, email));
            }

            return identity;
        }
        catch (Exception e)
        {
            return null;
        }
        #endregion

    }
}