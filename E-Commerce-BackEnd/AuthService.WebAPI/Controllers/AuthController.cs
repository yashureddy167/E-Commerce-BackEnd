using AuthService.Application.DTO_s;
using AuthService.Application.Features.Commands;
using AuthService.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {

        [HttpGet("Login/{email}/{password}")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var loginDetails = new LoginDTO { Email = email, Password = password };
                var query = new LoginQuery(loginDetails);
                var result = await mediator.Send(query);
                await AddRefreshTokenToCookie(result.refreshToken);
                return Ok(result.accessToken);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "error occured while loging in", details = ex.Message });
            }
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDTO userDTO)
        {

            try
            {
                var command = new CreateUserCommand(userDTO);
                var isAdded = await mediator.Send(command);
                return Created();
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "error occured while adding user", details = ex.Message });
            }
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> GetRefreshedTokensAsync()
        {
            Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
            //var query = new RefreshTokenQuery(refreshToken);
            return Ok();
        }

        private Task AddRefreshTokenToCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,             // Makes the cookie inaccessible to JavaScript
                Secure = true,               // Only sends cookie over HTTPS
                SameSite = SameSiteMode.Strict, // Protects against CSRF
                Expires = DateTime.UtcNow.AddDays(7), // Cookie expiry - adjust as needed
                Path = "/api/auth/refresh"    // Path where the cookie is valid
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            return Task.CompletedTask;
        }
    }
}
