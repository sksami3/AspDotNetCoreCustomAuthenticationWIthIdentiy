using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspDotNetCoreCustomAuthenticationWIthIdentiy.Domain.AuthModel;
using AspDotNetCoreCustomAuthenticationWIthIdentiy.Helper;
using AspDotNetCoreCustomAuthenticationWIthIdentiy.ViewModels;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<UserController> logger)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _logger = logger;
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
                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    _logger.Log(LogLevel.Warning, token);
                    bool isSent = EmailSender.SendEmail(user, token);

                    //await SignInManager.SignInAsync(user, isPersistent: false);
                    return isSent;
                }
                else
                    //result.Errors;
                    return false;
            }
            else
                return false;
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if(string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(token))
            {
                var result = await UserManager.ConfirmEmailAsync(await UserManager.FindByIdAsync(userId), token);
                if (result.Succeeded)
                    return StatusCode(200);
                else
                    StatusCode(500);
            }
            return StatusCode(404);
        }


        // POST api/<UserController>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(UserVM registerVM)
        {
            if (registerVM != null)
            {
                try
                {
                    //#region Basic
                    ////var result = await SignInManager.PasswordSignInAsync(registerVM.UserName, registerVM.Password, false, true);
                    //#endregion
                    #region with token generation
                    var result = await SignInManager.PasswordSignInAsync(registerVM.UserName, registerVM.Password, true, false);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByNameAsync(registerVM.UserName);
                        var roles = await UserManager.GetRolesAsync(user);
                        var claims = await UserManager.GetClaimsAsync(user);
                        user = JwtTokenGeneration.GenerateToken(user, roles,claims);

                        return Ok(user);
                    }
                    #endregion
                    else
                        //result.Errors;
                        return Content("Error in login");
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            else
                return Content("Input error!!");
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
