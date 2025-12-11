using iTaskAPI.Models;
using iTaskAPI.Models.DTOs;
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

        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProgramadorProfileDTO dto)
        {
            var sucesso = await _repository.UpdatePerfilProgramadorAsync(dto);
            if (!sucesso) return NotFound("Programador não encontrado.");
            return Ok("Perfil atualizado com sucesso.");
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

        // GET /api/programador/EquipeCards
        // Retorna os cards de programadores para a equipe
        [HttpGet("EquipeCards")]
        public async Task<ActionResult<List<ProgramadorCardEquipeDTO>>> GetEquipeCards()
        {
            try
            {
                var resultado = await _repository.GetProgramadoresParaEquipeAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                // Logar o erro se tiveres logger
                return StatusCode(500, "Erro interno ao buscar equipe: " + ex.Message);
            }
        }

        // GET /api/programador/EquipeCards/{idGestor}
        // Retorna os cards de programadores para a equipe de um gestor específico
        [HttpGet("EquipeCards/{idGestor}")]
        public async Task<ActionResult<List<ProgramadorCardEquipeDTO>>> GetEquipeCardsPorGestor(int idGestor)
        {
            try
            {
                var equipe = await _repository.GetProgramadoresPorGestorAsync(idGestor);
                return Ok(equipe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET 
        [HttpGet("{id}/detalhe")]
        public async Task<ActionResult<ProgramadorDetalhe>> GetDetalhe(int id)
        {
            var programador = await _repository.GetDetalheByIdAsync(id);

            if (programador == null)
            {
                return NotFound("Programador não encontrado.");
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
        public async Task<ActionResult> Update(int id, [FromBody] ProgramadorDetalhe programador)
        {
            // 1. Validação de consistência da Requisição
            if (id != programador.Id)
            {
                return BadRequest(new { mensagem = "O ID da URL difere do ID do corpo da requisição." });
            }

            // 2. Validação dos campos (Data Annotations do DTO)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 3. Chamada ao Repositório
            // A mágica da atualização das tabelas (Programador, Utilizador, Gestor) acontece aqui dentro
            var atualizado = await _repository.UpdateAsync(programador);

            // 4. Tratamento do Resultado
            if (!atualizado)
            {
                // Se retornou false, assumimos que o ID não existe no banco
                return NotFound(new { mensagem = $"Programador com ID {id} não encontrado." });
            }

            // 5. Retorno de Sucesso (204 No Content)
            // Significa: "Deu tudo certo, não tenho conteúdo novo para te mostrar"
            return NoContent();
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