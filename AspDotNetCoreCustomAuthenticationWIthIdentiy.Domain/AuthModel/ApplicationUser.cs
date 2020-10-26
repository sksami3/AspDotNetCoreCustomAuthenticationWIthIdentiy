using Microsoft.AspNetCore.Identity;

namespace AspDotNetCoreCustomAuthenticationWIthIdentiy.Domain.AuthModel
{
    public class ApplicationUser : IdentityUser
    {
        public string token { get; set; }
    }
}
