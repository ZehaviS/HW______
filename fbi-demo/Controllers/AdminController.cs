using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FBI.Models;
using FBI.Services;

namespace FBI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        public AdminController() { }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User User)
        {
            var dt = DateTime.Now;
            //var query = $"select * from users where idnumber = @idnumber";
            if (User.Username != "Wray"
            || User.Password != $"W{dt.Year}#{dt.Day}!")
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("username", User.Username),
                new Claim("type", "Admin"),
            };

            var token = FbiTokenService.GetToken(claims);

            return new OkObjectResult(FbiTokenService.WriteToken(token));
        }


        [HttpPost]
        [Route("[action]")]
        [Authorize(Policy = "Admin")]
        public IActionResult GenerateBadge([FromBody] Agent agent)
        {
            var claims = new List<Claim>
            {
                new Claim("username", agent.Name),
                new Claim("type", "Agent"),
                new Claim("ClearanceLevel", agent.ClearanceLevel.ToString()),
            };

            var token = FbiTokenService.GetToken(claims);

            return new OkObjectResult(FbiTokenService.WriteToken(token));
        }
    }



}
