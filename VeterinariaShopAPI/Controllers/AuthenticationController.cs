
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using VeterinariaShopAPI.Entities;

namespace VeterinariaShopAPI.Controllers
{

    [Route("/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly UserManager<Client> _userManagerClient;
        private readonly UserManager<Admin> _userManagerAdmin;
        private readonly DataContext _context;

        public class AuthenticationRequestBody
        {
            [DefaultValue("administrador@email.com")]
            public string? Email { get; set; }
            [DefaultValue("123qwe")]
            public string? Password { get; set; }
        }
        public AuthenticationController(
            IConfiguration config,
            UserManager<User> userManager,
            UserManager<Client> userManagerClient,
            UserManager<Admin> userManagerAdmin,
            DataContext context
            )
        {
            _config = config;
            _userManager = userManager;
            _userManagerClient = userManagerClient;
            _userManagerAdmin = userManagerAdmin;
            _context = context;

        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> AuthenticateAsync(AuthenticationRequestBody authenticationRequestBody)
        {
            var user = await _userManager.FindByEmailAsync(authenticationRequestBody.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, authenticationRequestBody.Password))
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim("name", user.UserName),
                new Claim("email", $"{user.Email}")
            };

            if (roles.Any("Administrador".Contains))
            {
                var admin = await _userManagerAdmin.FindByIdAsync(user.Id.ToString());
                claims.Add(new Claim("name", $"{admin.Name}"));

            }
            else if (roles.Any("Cliente".Contains))
            {
                var client = await _userManagerClient.FindByIdAsync(user.Id.ToString());
                claims.Add(new Claim("name", $"{client.Name}"));
            }


            foreach (var role in roles)
            {
                claims.Add(new Claim("assigned_role", role));
            }
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Authentication:SecretForKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _config["Authentication:Issuer"],
                audience: _config["Authentication:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return Ok(jwt);

        }


    }
}
