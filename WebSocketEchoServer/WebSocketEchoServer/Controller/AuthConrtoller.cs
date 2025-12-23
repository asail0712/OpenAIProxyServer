using Service.DTO.Auth;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

using XPlan.Controller;

namespace OpenAIProxyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : GenericController<IdentityRequest, LoginResponse, IAuthService>
    {
        public AuthController(IAuthService service)
            : base(service)
        {

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] IdentityRequest request)
        {
            var bResult = await _service.Login(request);

            return Ok(bResult);
        }

        [HttpPost("CreateIdentity")]
        public async Task<IActionResult> CreateIdentity([FromBody] IdentityRequest request)
        {
            var bResult = await _service.CreateIdentity(request);

            return Ok(bResult);
        }


        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var bResult = await _service.ChangePassword(request);

            return Ok(bResult);
        }
    }
}
