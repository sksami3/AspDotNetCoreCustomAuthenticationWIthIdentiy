using Microsoft.AspNetCore.Identity;
using System;

namespace AspDotNetCoreCustomAuthenticationWIthIdentiy.Domain.AuthModel
{
    public class ApplicationUser : IdentityUser
    {
        public string token { get; set; }
    }
}
