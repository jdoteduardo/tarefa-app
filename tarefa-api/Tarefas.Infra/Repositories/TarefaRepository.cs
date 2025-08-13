using Microsoft.EntityFrameworkCore;
using Tarefas.Domain.Interfaces;
using Tarefas.Domain.Models;
using Tarefas.Infra.Context;

namespace Tarefas.Infra.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly TarefaDbContext _context;

        public TarefaRepository(TarefaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tarefa>> ListarTarefas()
        {
            return await _context.Tarefas
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Tarefa?> BuscarPorId(Guid id)
        {
            return await _context.Tarefas
                .AsNoTracking()
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Tarefa> CriarTarefa(Tarefa tarefa)
        {
            tarefa.Id = Guid.NewGuid();
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();

            return tarefa;
        }

        public async Task<Tarefa> AtualizarTarefa(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            await _context.SaveChangesAsync();

            return tarefa;
        }

        public async Task<bool> DeletarTarefa(Guid id)
        {
            var tarefa = await BuscarPorId(id);

            if (tarefa == null)
                return false;

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
