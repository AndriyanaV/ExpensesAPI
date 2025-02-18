using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ExpensesAPI.Dto.UserDto;
using ExpensesAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpensesAPI.Controllers
{

    [Authorize(Roles = "Admin")]
    [Route("api/users")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // Ruta za prijavu korisnika pomocu emaila i lozinke, vraca JWT token i ulogu.
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = await CreateToken(user);
            return Ok(new
            {
                Message = "Logged in successfully",
                Token = token,
                Role = roles.FirstOrDefault()
            });
        }

        // Ruta za dodavanje novog korisnika sa ulogom.
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] UserForCreationDto userForCreation)
        {
            var userToCreate = new User
            {
                FirstName = userForCreation.FirstName,
                LastName = userForCreation.LastName,
                Email = userForCreation.Email,
                UserName = userForCreation.Email
            };

            var result = await _userManager.CreateAsync(userToCreate, userForCreation.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

    
            var roleResult = await _userManager.AddToRoleAsync(userToCreate, userForCreation.Role);
            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            return Ok(new { Message = "User added successfully." });
        }

        // Ruta za dohvat korisnika po ID-u.
        [HttpGet("get-user/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };

            return Ok(userDto);
        }

        // Ruta za dohvat svih korisnika u sistemu.
        [HttpGet("get-users")]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users.ToList();

            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            }).ToList();

            return Ok(userDtos);
        }

        // Ruta za azuriranje podataka korisnika po ID-u.
        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserForUpdateDto updateUserDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = "User updated successfully." });
        }


        // Ruta za brisanje korisnika po ID-u.
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = "User deleted successfully" });
        }

        // Funkcija za kreiranje JWT tokena za korisnika.
        private async Task<string> CreateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);
            var secret = new SymmetricSecurityKey(key);
            var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }



    }
}
