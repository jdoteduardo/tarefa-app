using Microsoft.AspNetCore.Mvc;
using Tarefas.Application.DTO;
using Tarefas.Application.Interfaces;

namespace TarefaAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] UsuarioDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tokenString = await _authService.Login(usuarioDTO);

            return Ok(new { access_token = tokenString });
        }
    }
}
