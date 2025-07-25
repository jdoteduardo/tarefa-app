using Tarefas.Domain.Models;

namespace Tarefas.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> BuscarPorLoginESenha(Usuario usuario);
    }
}
