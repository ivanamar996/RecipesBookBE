using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RecepiesBook.Models;

namespace RecepiesBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;

        public AuthController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user) 
        {
            var userInDb = await userManager.FindByEmailAsync(user.Email);

            if (userInDb != null && await userManager.CheckPasswordAsync(userInDb, user.Password))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userInDb.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));

                var token = new JwtSecurityToken(
                        issuer : "http://oec.com",
                        audience: "http://oec.com",
                        expires: DateTime.UtcNow.AddHours(1),
                        claims: claims,
                        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized(); 
        }

        [HttpPost("register")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            var userInDb = await userManager.FindByEmailAsync(user.Email);

            if (userInDb == null )
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser appUser = new ApplicationUser
                    {
                        Email = user.Email,
                        UserName = user.Email.Split('@')[0],
                        SecurityStamp = Guid.NewGuid().ToString(),
                    };

                    IdentityResult result = await userManager.CreateAsync(appUser, user.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));

                        var token = new JwtSecurityToken(
                                issuer: "http://oec.com",
                                audience: "http://oec.com",
                                expires: DateTime.UtcNow.AddHours(1),
                                claims: claims,
                                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                            );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                    else
                    {
                        return BadRequest("Error occuried while creating a new user!");
                    }
                }
                else 
                {
                    return BadRequest(ModelState);
                }  
            }

            return BadRequest("This email already exists in database.");
        }
    }
}