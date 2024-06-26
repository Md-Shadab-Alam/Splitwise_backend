using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Splitwise.Data;
using Splitwise.Entities;
using Splitwise.Services;
using Splitwise.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Splitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly SplitwiseDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private string secretKey;

        public AuthController(SplitwiseDbContext context, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole>roleManager)
        {
            _context = context;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            ApplicationUser userFromDb = _context.ApplicationUsers
                                            .FirstOrDefault(u => u.UserName.ToLower() == model.Username.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password);

            if(isValid == false)
            {
                return BadRequest();
            }

            //we have to generate token
            var roles = await _userManager.GetRolesAsync(userFromDb);
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key= Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("fullName", userFromDb.Name),
                    new Claim("id", userFromDb.Id.ToString()),
                    new Claim(ClaimTypes.Email, userFromDb.Email.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponse loginResponse = new()
            {
                Email = userFromDb.Email,
                Token = tokenHandler.WriteToken(token)
            };
            if(loginResponse.Email == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                return BadRequest();
            }
            return Ok(loginResponse);

        }



            [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            ApplicationUser userFromDb = _context.ApplicationUsers
                .FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());

            //if (userFromDb == null)
            //{
            //    return BadRequest();
            //}

            ApplicationUser newUser = new()
            {
                UserName = model.UserName,
                Email = model.UserName,
                NormalizedUserName = model.UserName.ToUpper(),
                Name = model.Name
            };
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if(result.Succeeded)
            {
                if(!_roleManager.RoleExistsAsync(StorageData.Role_Admin).GetAwaiter().GetResult())
                {
                    //create roles in database
                    await _roleManager.CreateAsync(new IdentityRole(StorageData.Role_Admin));
                    await _roleManager.CreateAsync(new IdentityRole(StorageData.Role_Customer));

                }

                if (model.Role.ToLower() == StorageData.Role_Admin)
                {
                    await _userManager.AddToRoleAsync(newUser, StorageData.Role_Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, StorageData.Role_Customer);
                }

                return Ok(result);
            }
            return BadRequest();

        }

    }
}
