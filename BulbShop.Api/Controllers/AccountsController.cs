using BulbShop.Common.DTOs.Authentication;
using BulbShop.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BulbShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AccountsController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(LoginResponseModel))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseModel))]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimValueTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:ValidIssuer"],
                    audience: _configuration["JwtSettings:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                var responseBody = new LoginResponseModel
                {
                    IsSuccessful = true,
                    Message = "User signed in successfully.",
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                };

                return Ok(responseBody);
            }
            return Unauthorized(new LoginResponseModel
            {
                IsSuccessful = false,
                Message = "Login failed due to invalid username or password. Please try again."
            });
        }


        [HttpPost]
        [Route("register")] // This endpoint handles customer registration
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RegisterResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(RegisterResponseModel))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(RegisterResponseModel))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LoginResponseModel))]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExistsWithSameName = await userManager.FindByNameAsync(model.Username);
            if (userExistsWithSameName != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new RegisterResponseModel 
                { 
                    IsSuccessful = false, 
                    Message = "A user with this username already exists!" 
                });
            }

            var userExistsWithSameEmail = await userManager.FindByNameAsync(model.Email);
            if (userExistsWithSameEmail != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new RegisterResponseModel
                {
                    IsSuccessful = false,
                    Message = "A user with this email address already exists!"
                });
            }

            if (!string.Equals(model.Password, model.ConfirmPassword, StringComparison.Ordinal))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new RegisterResponseModel
                {
                    IsSuccessful = false,
                    Message = "The two passwords do not match. Please try again."
                });
            }
            
            // Validation passed. Create new user record.
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);

            // Create the "Customer" role, if it does not exist already in the DB.
            if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));
            }

            // Assign the "Customer" role to this user.
            if (await roleManager.RoleExistsAsync(UserRoles.Customer))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Customer);
            }

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new RegisterResponseModel()
                {
                    IsSuccessful = false,
                    Message = "User registration failed unexpectedly. Please try again later."
                });
            }

            return StatusCode(StatusCodes.Status201Created, new RegisterResponseModel()
            {
                IsSuccessful = true,
                Message = "User registered successfully!"
            });
        }


        [HttpPost]
        [Route("register-staff")] // This endpoint handles staff registration
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RegisterResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(RegisterResponseModel))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(RegisterResponseModel))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LoginResponseModel))]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterStaffModel model)
        {
            var userExistsWithSameName = await userManager.FindByNameAsync(model.Username);
            if (userExistsWithSameName != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new RegisterResponseModel
                {
                    IsSuccessful = false,
                    Message = "A user with this username already exists!"
                });
            }

            var userExistsWithSameEmail = await userManager.FindByNameAsync(model.Email);
            if (userExistsWithSameEmail != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new RegisterResponseModel
                {
                    IsSuccessful = false,
                    Message = "A user with this email address already exists!"
                });
            }

            if (!string.Equals(model.Password, model.ConfirmPassword, StringComparison.Ordinal))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new RegisterResponseModel
                {
                    IsSuccessful = false,
                    Message = "The two passwords do not match. Please try again."
                });
            }

            if (model.Roles == null || model.Roles.Length == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new RegisterResponseModel
                {
                    IsSuccessful = false,
                    Message = "At least one role must be specified. Please try again."
                });
            }

            // Validation passed. Create new user record.
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);

            var availableStaffRoles = new HashSet<string>() { UserRoles.SalesRep, UserRoles.Supervisor, UserRoles.Administrator };

            foreach (string role in model.Roles)
            {
                if (availableStaffRoles.Contains(role))
                {
                    // Create the role, if it does not exist already in the DB.
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));

                    // Assign the role to this user.
                    if (await roleManager.RoleExistsAsync(role))
                        await userManager.AddToRoleAsync(user, role);
                }
            }

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new RegisterResponseModel()
                {
                    IsSuccessful = false,
                    Message = "User registration failed unexpectedly. Please try again later."
                });
            }

            return StatusCode(StatusCodes.Status201Created, new RegisterResponseModel()
            {
                IsSuccessful = true,
                Message = "User registered successfully!"
            });
        }
    }
}
