using AutoMapper;
using Tarefas.Application.DTO;
using Tarefas.Application.Interfaces;
using Tarefas.Domain.Interfaces;
using Tarefas.Domain.Models;

namespace Tarefas.Application.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IMapper _mapper;

        public TarefaService(ITarefaRepository tarefaRepository, IMapper mapper)
        {
            _tarefaRepository = tarefaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TarefaDTO>> BuscarTarefas()
        {
            var tarefas = await _tarefaRepository.BuscarTarefas();

            return _mapper.Map<IEnumerable<TarefaDTO>>(tarefas);
        }

        public async Task<TarefaDTO?> BuscarPorId(Guid id)
        {
            var tarefa = await _tarefaRepository.BuscarPorId(id);

            return _mapper.Map<TarefaDTO>(tarefa);
        }

        public async Task<TarefaDTO> CriarTarefa(TarefaDTO tarefaDTO)
        {
            var tarefa = _mapper.Map<Tarefa>(tarefaDTO);
            var tarefaCriada = await _tarefaRepository.CriarTarefa(tarefa);

            return _mapper.Map<TarefaDTO>(tarefaCriada);
        }

        public async Task<TarefaDTO> AtualizarTarefa(TarefaDTO tarefaDTO)
        {
            var tarefa = _mapper.Map<Tarefa>(tarefaDTO); 
            var tarefaCriada = await _tarefaRepository.AtualizarTarefa(tarefa);

            return _mapper.Map<TarefaDTO>(tarefaCriada);
        }

        public Task<bool> DeletarTarefa(Guid id)
        {
            return _tarefaRepository.DeletarTarefa(id);
        }
    }
}
