using HealthCareAppointments.Entities;
using HealthCareAppointments.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthCareAppointments.Controllers
{
    [Route("api/accounts/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly SignInManager<ApplicationUsers> _signInManager;
        private readonly IConfiguration _config;
        public AccountController(UserManager<ApplicationUsers> userManager, SignInManager<ApplicationUsers> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (registerModel != null)
            {
                var user = new ApplicationUsers { NameofUser = registerModel.NameofUser, UserName = registerModel.Email, Email = registerModel.Email };
                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("List", "Cake"); //Currently redirecting user to cake list page

                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<string> Login(LoginModel loginModel)
        {
            if (loginModel != null) 
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password,false,false);

                if (result.Succeeded)
                {
                    //Create claim object for JWT token
                    var claims = new[] 
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                        new Claim("Email",loginModel.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                            _config["Jwt:Issuer"],
                            _config["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn
                        );
                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return jwtToken;
                }
            }

            throw new Exception("User not found.");
        }
    }
}
