using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspDotNetCoreCustomAuthenticationWIthIdentiy.Domain.AuthModel;
using AspDotNetCoreCustomAuthenticationWIthIdentiy.ViewModels;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<IActionResult> Authenticate(UserVM registerVM)
        {
            if (registerVM != null)
            {
                try
                {
                    #region Basic
                    //var result = await SignInManager.PasswordSignInAsync(registerVM.UserName, registerVM.Password, false, true);
                    #endregion
                    #region with token generation
                    var result = GenerateToken(await UserManager.FindByNameAsync(registerVM.UserName));
                    #endregion
                    if (result != null)
                    {
                        //await SignInManager.PasswordSignInAsync(registerVM.UserName, registerVM.Password, false, true);
                        return Ok(result);
                    }
                    else
                        //result.Errors;
                        return Content("Error in login");
                }
                catch(Exception e)
                {
                    throw e;
                }
                 
            }
            else
               return Content("Input error!!");
        }
        private ApplicationUser GenerateToken(ApplicationUser user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Secret"];
            var key = Encoding.ASCII.GetBytes(secret);
            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    //new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.token = tokenHandler.WriteToken(token);
                return user;

            }
            catch (Exception e)
            {
                throw e;
            }

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
