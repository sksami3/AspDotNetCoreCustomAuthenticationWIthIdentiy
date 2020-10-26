using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspDotNetCoreCustomAuthenticationWIthIdentiy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        public RoleManager<IdentityRole> _roleManager { get; }

        public AdministrationController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
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
        public async Task<IActionResult> Post(string role)
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
