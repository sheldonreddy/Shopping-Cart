using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GG_shopping_cart.Entities;
using GG_shopping_cart.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using GG_shopping_cart.Helpers;

namespace GG_shopping_cart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly string _jwtSecretKey;
        private readonly ResponseDto _response;
        private readonly ILogger<UsersController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger<UsersController> logger
        )
		{
            _userManager = userManager;
            _jwtIssuer = ConfigurationHelpers.GetConfigurationValue(configuration, "JWTAuthorizer:Issuer", "");
            _jwtAudience = ConfigurationHelpers.GetConfigurationValue(configuration, "JWTAuthorizer:Audience", "");
            _jwtSecretKey = ConfigurationHelpers.GetConfigurationValue(configuration, "JWTAuthorizer:SecretKey", "");
            _response = new ResponseDto();
            _logger = logger;
        }

        // POST: api/<UserController>
        [HttpPost("register")]
        [AllowAnonymous]

        public async Task<object> Register([FromBody] RegistrationDto registration)
        {
            _logger.LogInformation("Register: Request initiated");

            if (!ModelState.IsValid)
            {
                _logger.LogError("Register: Invalid data");

                return BadRequest(ModelState);
            }

            try
            {
                var user = new ApplicationUser
                {
                    UserName = registration.Email,
                    Email = registration.Email,
                    Firstname = registration.Firstname,
                    Lastname = registration.Lastname,
                };

                var result = await _userManager.CreateAsync(user, registration.Password);
                _response.Result = result;

                if (!result.Succeeded)
                {
                    _logger.LogError("Register: User not registerd:", registration.Email);
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Register: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                return StatusCode(500, _response);
            }

        }

        // POST: api/<UserController>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<object> Login([FromBody] UserLogin model)
        {
            _logger.LogInformation("Login: Request initiated");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Login: Invalid data");
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        _jwtIssuer,
                        _jwtAudience,
                        claims,
                        expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: creds
                    );

                    _response.Result = new
                    {
                        user.Id,
                        user.Email,
                        user.Firstname,
                        user.Lastname,
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                    };
                    return Ok(_response);
                }

                _logger.LogError("Login: User not found", model.Email);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { "User not found" };
                return StatusCode(404, _response);

            }
            catch (Exception ex)
            {
                _logger.LogError("Login: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);

            }

        }

        [HttpPost("logout")]
        [Authorize]

        public async Task<object> Logout()
        {
            _logger.LogInformation("Logout: Requeste initiated");
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                _response.Message = "Logout successful";

                _logger.LogInformation("Logout: Logout successful");
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Logout: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);

            }
        }
    }

}

