using iTaskAPI.Models;
using iTaskAPI.Repository.TarefaRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace iTaskAPI.Controllers.TarefaControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaRepository _repository;
        public TarefaController(ITarefaRepository repository)
        {
            _repository = repository;
        }

        // GET /api/tarefa
        // Lista todas as tarefas
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetAll()
        {
            IEnumerable<Tarefa> tarefas = await _repository.GetAllAsync();

            if (!tarefas.Any())
            {
                return NotFound("Nenhuma tarefa encontrada.");
            }

            return Ok(tarefas);
        }
        
        // GET /api/tarefa/{id}
        // Retorna uma tarefa específica pelo ID
        // ============================================================
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Tarefa>> GetById(int id)
        {
            Tarefa? tarefa = await _repository.GetByIdAsync(id);

            if (tarefa == null)
            {
                return NotFound($"Tarefa com ID {id} não encontrada.");
            }

            return Ok(tarefa);
        }

        // GET /api/tarefa/gestor/{gestorId}
        // Retorna todas as tarefas criadas por um Gestor
        [HttpGet("GetByGestor/{gestorId}")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetByGestor(int gestorId)
        {
            IEnumerable<Tarefa> tarefas = await _repository.GetByGestorAsync(gestorId);

            if (!tarefas.Any())
            {
                return NotFound($"Nenhuma tarefa encontrada para o gestor com ID {gestorId}.");
            }

            return Ok(tarefas);
        }

        // GET /api/tarefa/programador/{programadorId}
        // Retorna todas as tarefas atribuídas a um Programador
        [HttpGet("GetByProgramador/{programadorId}")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetByProgramador(int programadorId)
        {
            IEnumerable<Tarefa> tarefas = await _repository.GetByProgramadorAsync(programadorId);

            if (!tarefas.Any())
            {
                return NotFound($"Nenhuma tarefa encontrada para o programador com ID {programadorId}.");
            }

            return Ok(tarefas);
        }

        // POST /api/tarefa
        // Cria uma nova tarefa
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Tarefa tarefa)
        {
            if (tarefa == null)
            {
                return BadRequest("Os dados da tarefa não foram fornecidos.");
            }

            await _repository.AddAsync(tarefa);

            return CreatedAtAction(nameof(GetById), new { id = tarefa.Id }, tarefa);
        }

        // PUT /api/tarefa/{id}
        // Atualiza os dados de uma tarefa existente
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Tarefa tarefa)
        {
            if (id != tarefa.Id)
            {
                return BadRequest("O ID informado não corresponde ao ID da tarefa.");
            }

            Tarefa? tarefaExistente = await _repository.GetByIdAsync(id);

            if (tarefaExistente == null)
            {
                return NotFound($"Tarefa com ID {id} não encontrada.");
            }

            tarefaExistente.Descricao = tarefa.Descricao;
            tarefaExistente.EstadoAtual = tarefa.EstadoAtual;
            tarefaExistente.DataPrevistaInicio = tarefa.DataPrevistaInicio;
            tarefaExistente.DataPrevistaFim = tarefa.DataPrevistaFim;
            tarefaExistente.DataRealInicio = tarefa.DataRealInicio;
            tarefaExistente.DataRealFim = tarefa.DataRealFim;
            tarefaExistente.StoryPoints = tarefa.StoryPoints;
            tarefaExistente.IdProgramador = tarefa.IdProgramador;
            tarefaExistente.IdTipoTarefa = tarefa.IdTipoTarefa;

            await _repository.UpdateAsync(tarefaExistente);

            return Ok("Tarefa atualizada com sucesso!");
        }

        // PATCH /api/tarefa/{id}/estado
        // Atualiza somente o estado da tarefa (To Do, Doing, Done)
        [HttpPatch("{id}/estado")]
        public async Task<ActionResult> AtualizarEstado(int id, [FromBody] string novoEstado)
        {
            Tarefa? tarefa = await _repository.GetByIdAsync(id);

            if (tarefa == null)
            {
                return NotFound($"Tarefa com ID {id} não encontrada.");
            }

            tarefa.EstadoAtual = novoEstado;

            await _repository.UpdateAsync(tarefa);

            return Ok($"Estado da tarefa atualizado para: {novoEstado}");
        }

        // DELETE /api/tarefa/{id}
        // Remove uma tarefa do banco de dados
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Tarefa? tarefa = await _repository.GetByIdAsync(id);

            if (tarefa == null)
            {
                return NotFound($"Tarefa com ID {id} não encontrada.");
            }

            await _repository
            .DeleteAsync(tarefa);

            return Ok($"Tarefa com ID {id} foi removida com sucesso.");
        }
    }
}