using iTaskAPI.Models;
using iTaskAPI.Repository.TipoTarefaRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace iTaskAPI.Controllers.TipoTarefaControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoTarefaController : ControllerBase
    {
        private readonly ITipoTarefaRepository _repository;
        public TipoTarefaController(ITipoTarefaRepository repository)
        {
            _repository = repository;
        }

        // GET /api/tipotarefa
        // Retorna todos os tipos de tarefa
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<TipoTarefa>>> GetAll()
        {
            IEnumerable<TipoTarefa> tipos = await _repository.GetAllAsync();

            if (!tipos.Any())
            {
                return NotFound("Nenhum tipo de tarefa encontrado.");
            }

            return Ok(tipos);
        }

        // GET /api/tipotarefa/{id}
        // Retorna um tipo de tarefa específico
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<TipoTarefa>> GetById(int id)
        {
            TipoTarefa? tipo = await _repository.GetByIdAsync(id);

            if (tipo == null)
            {
                return NotFound($"Tipo de tarefa com ID {id} não encontrado.");
            }

            return Ok(tipo);
        }

        // POST /api/tipotarefa
        // Cria um novo tipo de tarefa
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] TipoTarefa tipoTarefa)
        {
            if (tipoTarefa == null)
            {
                return BadRequest("Os dados do tipo de tarefa não foram fornecidos.");
            }

            await _repository.AddAsync(tipoTarefa);
            return CreatedAtAction(nameof(GetById), new { id = tipoTarefa.Id }, tipoTarefa);
        }

        // PUT /api/tipotarefa/{id}
        // Atualiza um tipo de tarefa existente
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] TipoTarefa tipoTarefa)
        {
            if (id != tipoTarefa.Id)
            {
                return BadRequest("O ID informado não corresponde ao ID do tipo de tarefa.");
            }

            TipoTarefa? existente = await _repository.GetByIdAsync(id);

            if (existente == null)
            {
                return NotFound($"Tipo de tarefa com ID {id} não encontrado.");
            }

            existente.Nome = tipoTarefa.Nome;

            await _repository.UpdateAsync(existente);

            return Ok("Tipo de tarefa atualizado com sucesso.");
        }

        // DELETE /api/tipotarefa/{id}
        // Remove um tipo de tarefa
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            TipoTarefa? tipo = await _repository.GetByIdAsync(id);

            if (tipo == null)
            {
                return NotFound($"Tipo de tarefa com ID {id} não encontrado.");
            }

            await _repository.DeleteAsync(tipo);

            return Ok($"Tipo de tarefa com ID {id} removido com sucesso.");
        }
    }
}