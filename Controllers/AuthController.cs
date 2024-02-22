using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TheCats.Application.Dtos;
using TheCats.Application.Entities;
using TheCats.Application.Intefaces.Auth;
using TheCats.Application.Intefaces.User;

namespace TheCats.API.Controllers
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

        [HttpPost]
        public async Task<AuthResponseDTO> Login(UserCredentialsDTO userCredentialsDto, CancellationToken cancellationToken)
        {
            var ret = await _authService.Login(userCredentialsDto, cancellationToken);
            return ValidateResponse(ret, cancellationToken);
        }


        private AuthResponseDTO ValidateResponse(AuthResponseDTO ret, CancellationToken cancellationToken)
            => ret == null & cancellationToken.IsCancellationRequested
                ? null
                : ret;
    }
}