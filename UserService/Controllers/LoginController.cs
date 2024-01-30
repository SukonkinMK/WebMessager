using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Abstractions;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserAuthenticationService _userAuthenticationService;

        public LoginController(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginForm form)
        {
            string token = _userAuthenticationService.Authenticate(form);
            if (!string.IsNullOrEmpty(token))
                return Ok(token);

            return NotFound("User not found");
        }
    }
}
