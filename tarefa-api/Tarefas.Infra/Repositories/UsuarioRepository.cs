using Microsoft.EntityFrameworkCore;
using Tarefas.Domain.Interfaces;
using Tarefas.Domain.Models;
using Tarefas.Infra.Context;

namespace Tarefas.Infra.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly TarefaDbContext _context;

        public UsuarioRepository(TarefaDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> BuscarPorLoginESenha(Usuario usuario)
        {
            var usuarioBanco = await _context.Usuarios.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login.ToUpper() == usuario.Login.ToUpper());

            if (usuarioBanco == null || !BCrypt.Net.BCrypt.Verify(usuario.Senha, usuarioBanco.Senha))
                return null;

            return usuarioBanco;
        }
    }
}
