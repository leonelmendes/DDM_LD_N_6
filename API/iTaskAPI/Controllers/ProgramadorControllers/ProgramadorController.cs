using iTaskAPI.Models;
using iTaskAPI.Repository.ProgramadorRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace iTaskAPI.Controllers.ProgramadorControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramadorController : ControllerBase
    {
        private readonly IProgramadorRepository _repository;
        public ProgramadorController(IProgramadorRepository repository)
        {
            _repository = repository;
        }

        // GET /api/programador
        // Retorna todos os programadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Programador>>> GetAll()
        {
            IEnumerable<Programador> programadores = await _repository.GetAllAsync();

            if (!programadores.Any())
            {
                return NotFound("Nenhum programador encontrado.");
            }

            return Ok(programadores);
        }

        // GET /api/programador/{id}
        // Retorna um programador específico
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Programador>> GetById(int id)
        {
            Programador? programador = await _repository.GetByIdAsync(id);

            if (programador == null)
            {
                return NotFound($"Programador com ID {id} não encontrado.");
            }

            return Ok(programador);
        }

        // GET /api/programador/{id}/tarefas
        // Retorna as tarefas do programador
        [HttpGet("GetTarefas/{id}/tarefas")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefas(int id)
        {
            IEnumerable<Tarefa> tarefas = await _repository.GetTarefasByProgramadorAsync(id);

            if (!tarefas.Any())
            {
                return NotFound($"Nenhuma tarefa encontrada para o programador com ID {id}.");
            }

            return Ok(tarefas);
        }

        // POST /api/programador
        // Cria um novo programador
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Programador programador)
        {
            if (programador == null)
            {
                return BadRequest("Os dados do programador não foram fornecidos.");
            }

            await _repository.AddAsync(programador);
            return CreatedAtAction(nameof(GetById), new { id = programador.Id }, programador);
        }

        // PUT /api/programador/{id}
        // Atualiza um programador existente
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Programador programador)
        {
            if (id != programador.Id)
            {
                return BadRequest("O ID informado não corresponde ao ID do programador.");
            }

            Programador? existente = await _repository.GetByIdAsync(id);

            if (existente == null)
            {
                return NotFound($"Programador com ID {id} não encontrado.");
            }

            existente.NivelExperiencia = programador.NivelExperiencia;
            existente.IdGestor = programador.IdGestor;
            existente.IdUtilizador = programador.IdUtilizador;

            await _repository.UpdateAsync(existente);

            return Ok("Programador atualizado com sucesso.");
        }

        // DELETE /api/programador/{id}
        // Remove um programador
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Programador? programador = await _repository.GetByIdAsync(id);

            if (programador == null)
            {
                return NotFound($"Programador com ID {id} não encontrado.");
            }

            await _repository.DeleteAsync(programador);

            return Ok($"Programador com ID {id} removido com sucesso.");
        }
    }
}