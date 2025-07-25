using FluentValidation;
using Tarefas.Application.DTO;
using Tarefas.Domain.Interfaces;

namespace Tarefas.Application.Validator
{
    public class TarefaValidator : AbstractValidator<TarefaDTO>
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public TarefaValidator(ITarefaRepository tarefaRepository, IUsuarioRepository usuarioRepository)
        {
            _tarefaRepository = tarefaRepository;
            _usuarioRepository = usuarioRepository;

            RuleSet("Buscar", () =>
            {
                RuleFor(t => t.Id)
                .MustAsync(VerificarExistenciaTarefa).WithMessage("Tarefa não existe.");
            });

            RuleSet("Cadastrar", () =>
            {
                RuleFor(t => t.Titulo)
                .NotEmpty().WithMessage("Título da tarefa está vazio.")
                .NotNull().WithMessage("Título da tarefa não informado.");

                RuleFor(t => t.DataLimite)
                .NotNull().WithMessage("Data limite não informada.")
                .Must(data => data.Date > DateTime.Today).WithMessage("Data limite deve ser uma data futura.");
            });

            RuleSet("Atualizar", () =>
            {
                RuleFor(t => t.Id)
                .MustAsync(VerificarExistenciaTarefa).WithMessage("Tarefa não existe.");

                RuleFor(t => t.Titulo)
                .NotEmpty().WithMessage("Título da tarefa está vazio.")
                .NotNull().WithMessage("Título da tarefa não informado.");

                RuleFor(t => t.DataLimite)
                .NotNull().WithMessage("Data limite não informada.")
                .GreaterThan(DateTime.Today).WithMessage("Data limite deve ser uma data futura.");
            });

            RuleSet("Deletar", () =>
            {
                RuleFor(t => t.Id)
                .MustAsync(VerificarExistenciaTarefa).WithMessage("Tarefa não existe.");
            });

        }

        private async Task<bool> VerificarExistenciaTarefa(Guid? id, CancellationToken token)
        {
            if (!id.HasValue)
                return false;

            return await _tarefaRepository.BuscarPorId(id.Value) != null;
        }
    }
}
