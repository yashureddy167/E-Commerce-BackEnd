using AuthService.Application.DTO_s;
using AuthService.Application.Features.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var query = new LoginCommand(loginDTO);
                var result = await mediator.Send(query);
                await AddRefreshTokenToCookie(result.refreshToken);
                return Ok(result.accessToken);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = "invalid credentials", details = ex.Message });
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

        [HttpPut("refresh")]
        public async Task<IActionResult> GetRefreshedTokensAsync()
        {
            try
            {
                Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized("Refresh token is missing.");
                }
                var refreshCommand = new RefreshCommand(refreshToken);
                var (newAccessToken, newRefreshToken) = await mediator.Send(refreshCommand);
                await AddRefreshTokenToCookie(newRefreshToken);
                return Ok(newAccessToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "error occured while refreshing tokens", details = ex.Message });
            }
        }

        [HttpPut("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return BadRequest("Refresh token is missing.");
                }
                var logoutCommand = new LogoutCommand(refreshToken);
                await mediator.Send(logoutCommand);
                Response.Cookies.Delete("refreshToken", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/api/auth/refresh"
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "error occured while logging out", details = ex.Message });
            }
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
