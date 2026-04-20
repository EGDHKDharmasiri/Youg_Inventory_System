using Microsoft.AspNetCore.Mvc;
using Youg_Inventory_System.Services;

namespace Youg_Inventory_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly CustomAuthenticationStateProvider _authProvider;

        public AuthController(CustomAuthenticationStateProvider authProvider)
        {
            _authProvider = authProvider;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authProvider.MarkUserAsLoggedOut();
                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
