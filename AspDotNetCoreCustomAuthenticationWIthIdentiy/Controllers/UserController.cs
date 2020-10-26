using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspDotNetCoreCustomAuthenticationWIthIdentiy.Domain.AuthModel;
using AspDotNetCoreCustomAuthenticationWIthIdentiy.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspDotNetCoreCustomAuthenticationWIthIdentiy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        // GET: api/<UserController>
        [HttpGet]
        public IQueryable<ApplicationUser> Get()
        {
            return UserManager.Users;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost("register")]
        public async Task<bool> Register(UserVM registerVM)
        {
            if (registerVM != null)
            {
                var user = new ApplicationUser { Email = registerVM.Email, UserName = registerVM.UserName };
                var result = await UserManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    return true;
                }
                else
                    //result.Errors;
                    return false;
            }
            else
                return false;
        }

        // POST api/<UserController>
        [HttpPost("authenticate")]
        public async Task<bool> Authenticate(UserVM registerVM)
        {
            if (registerVM != null)
            {
                var result = await SignInManager.PasswordSignInAsync(registerVM.UserName, registerVM.Password, false, true);
                if (result.Succeeded)
                    return true;
                else
                    //result.Errors;
                    return false;
            }
            else
                return false;
        }
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
