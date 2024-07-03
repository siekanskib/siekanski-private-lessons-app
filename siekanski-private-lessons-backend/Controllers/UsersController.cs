using Microsoft.AspNetCore.Mvc; 
using siekanski_private_lessons_backend.Data; 
using siekanski_private_lessons_backend.Models; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace siekanski_private_lessons_backend.Controllers 
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class UsersController : ControllerBase 
    {
        private readonly ApplicationDbContext _context; 
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public UsersController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(UserRegistrationModel registrationModel)
        {
            var userExists = await _userManager.FindByEmailAsync(registrationModel.Email);
            if (userExists != null)
            {
                return BadRequest(new { message = "Użytkownik z tym adresem email już istnieje." });
            }

            userExists = await _userManager.FindByNameAsync(registrationModel.Username);
            if (userExists != null)
            {
                return BadRequest(
                    new { message = "Użytkownik z tą nazwą użytkownika już istnieje." }
                );
            }

            User user = new User
            {
                Email = registrationModel.Email,
                UserName = registrationModel.Username,
                FirstName = registrationModel.FirstName,
                LastName = registrationModel.LastName,
                Role = registrationModel.Role,
            };

            var result = await _userManager.CreateAsync(user, registrationModel.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "Użytkownik został zarejestrowany." });
        }

        private string GenerateJwtToken(User user)
        {
            var secret = _configuration["Jwt:Key"];
            var key = Encoding.ASCII.GetBytes(secret); 
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            };

            if (!string.IsNullOrEmpty(user.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), 
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token = token });
            }
            return Unauthorized();
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetUserIdByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest("Uczeń o podanym adresie e-mail nie istnieje.");
            }

            return user;
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<User>> GetUserId(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("Uczeń o podanym ID nie istnieje.");
            }

            return user;
        }
        
    }
}
