using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AspDotNetCoreCustomAuthenticationWIthIdentiy.Domain.AuthModel;
using AspDotNetCoreCustomAuthenticationWIthIdentiy.Helper;
using AspDotNetCoreCustomAuthenticationWIthIdentiy.ViewModels;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspDotNetCoreCustomAuthenticationWIthIdentiy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize("admin")]
    public class AdministrationController : ControllerBase
    {
        public RoleManager<IdentityRole> _roleManager { get; }
        public UserManager<ApplicationUser> _userManager { get; }

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        // GET: api/<AdministrationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AdministrationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AdministrationController>
        [HttpPost("createRole")]
        public async Task<IActionResult> CreateRole(string role)
        {
            if (string.IsNullOrEmpty(role))
                return StatusCode(404);
            var newRole = new IdentityRole { Name = role };
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
                return Ok(result);
            else
                return Content(String.Concat(String.Empty, result.Errors.ToArray()));
        }

        [HttpPost("addClaim")]
        public async Task<IActionResult> AddClaim(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return StatusCode(404);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return StatusCode(404);
            //decision according to front end (SPA); Hardcoded at this moment
            var result = await _userManager.AddClaimAsync(user, ClaimsStore.AllClaims
                .Where(x => x.Value == "Create Role").FirstOrDefault());
            if (result.Succeeded)
                return Ok(result);
            else
                return Content(String.Concat(String.Empty, result.Errors.ToArray()));
        }


        [HttpPost("addRoleToUser")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> AddRoleToUser(AspNetRolesVM aspNetRolesVM)
        {
            if (!string.IsNullOrEmpty(aspNetRolesVM.roleId) && !string.IsNullOrEmpty(aspNetRolesVM.userId))
            {
                var role = await _roleManager.FindByIdAsync(aspNetRolesVM.roleId);
                var user = await _userManager.FindByIdAsync(aspNetRolesVM.userId);
                if (role != null && !await _userManager.IsInRoleAsync(user, role.Name))
                {
                    var result = await _userManager.AddToRoleAsync(user, role.Name);
                    if (result.Succeeded)
                        return Ok("Role assigned to the user");
                    else
                        return Content(String.Concat(String.Empty, result.Errors.ToArray()));
                }
                
            }
            return StatusCode(404);
        }

        // PUT api/<AdministrationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AdministrationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
