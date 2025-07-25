using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tarefas.Application.DTO;
using Tarefas.Application.Interfaces;

namespace TarefaAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;

        public TarefaController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TarefaDTO>>> ListarTarefas()
        {
            var tarefas = await _tarefaService.ListarTarefas();

            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TarefaDTO>> BuscarTarefaPorId(Guid id, [FromServices] IValidator<TarefaDTO> validator)
        {
            var tarefaDTO = new TarefaDTO() { Id = id };

            ValidationResult validationResult = await validator.ValidateAsync(tarefaDTO, options => options.IncludeRuleSets("Buscar"));

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            var result = await _tarefaService.BuscarPorId(id);

            if (result == null)
                throw new ApplicationException("Erro ao buscar tarefa.");

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TarefaDTO>> CriarTarefa([FromBody] TarefaDTO tarefaDTO, [FromServices] IValidator<TarefaDTO> validator)
        {
            ValidationResult validationResult = await validator.ValidateAsync(tarefaDTO, options => options.IncludeRuleSets("Cadastrar"));

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            var result = await _tarefaService.CriarTarefa(tarefaDTO);

            if (result == null)
                throw new ApplicationException("Erro ao criar a tarefa.");

            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<TarefaDTO>> AtualizarTarefa([FromBody] TarefaDTO tarefaDTO, [FromServices] IValidator<TarefaDTO> validator)
        {
            ValidationResult validationResult = await validator.ValidateAsync(tarefaDTO, options => options.IncludeRuleSets("Atualizar"));

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            var result = await _tarefaService.AtualizarTarefa(tarefaDTO);

            if (result == null)
                throw new ApplicationException("Erro ao atualizar a tarefa.");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeletarTarefa(Guid id, [FromServices] IValidator<TarefaDTO> validator)
        {
            var tarefaDTO = new TarefaDTO() { Id = id };

            ValidationResult validationResult = await validator.ValidateAsync(tarefaDTO, options => options.IncludeRuleSets("Deletar"));

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            var result = await _tarefaService.DeletarTarefa(id);

            if (!result)
                throw new ApplicationException("Erro ao atualizar a tarefa.");

            return NoContent();
        }
    }
}
