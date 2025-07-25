using Tarefas.Application.DTO;

namespace Tarefas.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(UsuarioDTO usuarioDTO);
    }
}
