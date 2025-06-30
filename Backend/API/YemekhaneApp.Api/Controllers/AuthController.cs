using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YemekhaneApp.Application.CQRS.Queries.TrustedDevice;
using YemekhaneApp.Application.DTOs.Auth;
using YemekhaneApp.Application.Services.Auth;
using YemekhaneApp.Application.Services.Token;

namespace YemekhaneApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequestDto request)
        {
            var userAgent = Request.Headers["User-Agent"].ToString();

            try
            {
                var result = await _authService.AuthenticateAsync(request.Password, userAgent);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("istrusted")]
        public async Task<IActionResult> IsTrustedDevice([FromBody] UserAgentRequestDto request)
        {
            var isTrusted = await _authService.IsTrustedDeviceAsync(request.UserAgent);

            if (isTrusted)
            {
                var token = _authService.GenerateQuickToken();
                return Ok(new AuthResponseDto { Token = token });
            }

            return Unauthorized();
        }

        [HttpGet("trusted-devices")]
        public async Task<IActionResult> GetTrustedDevices([FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new GetTrustedDevicesQuery());
            if (result == null || !result.Success)
                return NotFound(result?.ErrorMessage ?? "Kayıtlı cihaz bulunamadı.");

            return Ok(result.Value);
        }
    }
}
