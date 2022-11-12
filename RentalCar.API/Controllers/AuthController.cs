using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using RentalCar.API.Models;
using RentalCar.Model.Models;
using RentalCar.Service;

namespace RentalCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(ITokenService tokenService,IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody]  Register register)
        {
            try
            {
                var username = register.UserName.ToLower();
                if(_userService.GetUserByUsername(username) != null){
                    return BadRequest("Username already exists");
                }

                using var hmac = new HMACSHA512();

                var user = new User{
                    Username = register.UserName,
                    Contact = register.PhoneNumber,
                    Fullname = register.YourName,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                    PasswordSalt = hmac.Key
                };
                _userService.CreateUser(user);
                _userService.SaveChanges();
                var token = _tokenService.CreateToken(user);
                return Ok(token);
            }
            catch (BadHttpRequestException ex)
            {
                
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            try
            {
                // return Ok(_authService.Login(authUserDto));
                var user = _userService.GetUserByUsername(login.UserName);
                if(user == null) {
                    throw new UnauthorizedAccessException("Username is invalid.");
                } 
                
                using var hmac = new HMACSHA512(user.PasswordSalt);
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));
                for( var i = 0; i < computeHash.Length; i++){
                    if(computeHash[i] != user.PasswordHash[i]){
                        throw new UnauthorizedAccessException("Password is invalid.");
                    } 
                }
                var token = _tokenService.CreateToken(user);
                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}