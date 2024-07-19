using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebCoffee.Service.Models;

namespace WebCoffee.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _singInManager; 

        public AuthenticateController(
            UserManager<IdentityUser> userManager,
            //RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;

            //_roleManager = roleManager;
            _configuration = configuration;
            _singInManager = signInManager;
        }



        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] CoffeeLogin model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                //var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("City", GetCityByPhoneNumber(user.PhoneNumber))
                };

                var loginResult = await  _singInManager.PasswordSignInAsync(user , model.Password, true, false);

                //foreach (var userRole in userRoles)
                //{
                //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                //}

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var user = this.User.Identity?.Name;

            await _singInManager.SignOutAsync();

            return Ok();
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private string GetCityByPhoneNumber(string phoneNumber)
        {
            var cityCode = int.Parse(phoneNumber.Substring(1,3));

            switch (cityCode)
            {
                case 44:    return "Kiev";
                case 45:    return "Lviv";
                case 46:    return "new_york";
            }

            return "London";
        }

    }
}
