using iTaskAPI.Models;
using iTaskAPI.Repository.UtilizadorRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace iTaskAPI.Controllers.UtilizadorControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilizadorController : ControllerBase
    {
        private readonly IUtilizadorRepository _repository;
        public UtilizadorController(IUtilizadorRepository repository)
        {
            _repository = repository;
        }

        // GET /api/utilizador
        // Retorna todos os utilizadores
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Utilizador>>> GetAll()
        {
            IEnumerable<Utilizador> utilizadores = await _repository.GetAllAsync();

            if (!utilizadores.Any())
            {
                return NotFound("Nenhum utilizador encontrado.");
            }

            return Ok(utilizadores);
        }

        // GET /api/utilizador/{id}
        // Retorna um utilizador específico
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Utilizador>> GetById(int id)
        {
            Utilizador? utilizador = await _repository.GetByIdAsync(id);

            if (utilizador == null)
            {
                return NotFound($"Utilizador com ID {id} não encontrado.");
            }

            return Ok(utilizador);
        }

        // GET /api/utilizador/username/{username}
        // Retorna um utilizador pelo nome de usuário
        [HttpGet("GetByUsername/{username}")]
        public async Task<ActionResult<Utilizador>> GetByUsername(string username)
        {
            Utilizador? utilizador = await _repository.GetByUsernameAsync(username);

            if (utilizador == null)
            {
                return NotFound($"Utilizador com username '{username}' não encontrado.");
            }

            return Ok(utilizador);
        }

        // POST /api/utilizador
        // Cria um novo utilizador
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Utilizador utilizador)
        {
            if (utilizador == null)
            {
                return BadRequest("Os dados do utilizador não foram fornecidos.");
            }

            await _repository.AddAsync(utilizador);
            return CreatedAtAction(nameof(GetById), new { id = utilizador.Id }, utilizador);
        }

        // PUT /api/utilizador/{id}
        // Atualiza um utilizador existente
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Utilizador utilizador)
        {
            if (id != utilizador.Id)
            {
                return BadRequest("O ID informado não corresponde ao ID do utilizador.");
            }

            Utilizador? existente = await _repository.GetByIdAsync(id);

            if (existente == null)
            {
                return NotFound($"Utilizador com ID {id} não encontrado.");
            }

            existente.Nome = utilizador.Nome;
            existente.Username = utilizador.Username;
            existente.Password = utilizador.Password;

            await _repository.UpdateAsync(existente);

            return Ok("Utilizador atualizado com sucesso.");
        }

        // DELETE /api/utilizador/{id}
        // Remove um utilizador
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Utilizador? utilizador = await _repository.GetByIdAsync(id);

            if (utilizador == null)
            {
                return NotFound($"Utilizador com ID {id} não encontrado.");
            }

            await _repository.DeleteAsync(utilizador);

            return Ok($"Utilizador com ID {id} removido com sucesso.");
        }
    }
}