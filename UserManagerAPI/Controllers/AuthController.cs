using Microsoft.AspNetCore.Mvc;
using UserManagerAPI.Services.Interfaces;
using UserManagerAPI.Exceptions;
using UserManagerAPI.Dtos;

namespace UserManagerAPI.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController( IAuthService authService )
        {
            _authService = authService;
        }

        [HttpPost( "login" )]
        public IActionResult Login( LoginRequestDto input )
        {
            try
            {
                return Ok( _authService.Login( input.Username, input .Password) );
            }
            catch (ApiException ex)
            {
                return StatusCode( ex.StatusCode, new { error = true, message = ex.Message } );
            }
        }
        
    }
}
