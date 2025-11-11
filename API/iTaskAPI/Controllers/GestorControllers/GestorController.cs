using Microsoft.AspNetCore.Mvc;
using iTaskAPI.Repository.GestorRepository;
using System;
using System.Threading.Tasks;
using iTaskAPI.Models;

namespace iTaskAPI.Controllers.GestorControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GestorController : ControllerBase
    {
        private readonly IGestorRepository _repository;
        public GestorController(IGestorRepository repository)
        {
            _repository = repository;
        }

        // GET /api/gestor
        // Lista todos os gestores
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Gestor>>> GetAll()
        {
            IEnumerable<Gestor> gestores = await _repository.GetAllAsync();

            if (!gestores.Any())
            {
                return NotFound("Nenhum gestor encontrado.");
            }

            return Ok(gestores);
        }

        // GET /api/gestor/{id}
        // Busca um gestor específico
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Gestor>> GetById(int id)
        {
            Gestor? gestor = await _repository.GetByIdAsync(id);

            if (gestor == null)
            {
                return NotFound($"Gestor com ID {id} não encontrado.");
            }

            return Ok(gestor);
        }

        // GET /api/gestor/{id}/programadores
        // Retorna todos os programadores do gestor
        [HttpGet("GetProgramadores/{id}/programadores")]
        public async Task<ActionResult<IEnumerable<Programador>>> GetProgramadores(int id)
        {
            IEnumerable<Programador> programadores = await _repository.GetProgramadoresByGestorAsync(id);

            if (!programadores.Any())
            {
                return NotFound($"Nenhum programador encontrado para o gestor com ID {id}.");
            }

            return Ok(programadores);
        }

        // POST /api/gestor
        // Cria um novo gestor
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Gestor gestor)
        {
            if (gestor == null)
            {
                return BadRequest("Os dados do gestor não foram fornecidos.");
            }

            await _repository.AddAsync(gestor);
            return CreatedAtAction(nameof(GetById), new { id = gestor.Id }, gestor);
        }

        // PUT /api/gestor/{id}
        // Atualiza dados do gestor
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Gestor gestor)
        {
            if (id != gestor.Id)
            {
                return BadRequest("O ID informado não corresponde ao ID do gestor.");
            }

            Gestor? gestorExistente = await _repository.GetByIdAsync(id);

            if (gestorExistente == null)
            {
                return NotFound($"Gestor com ID {id} não encontrado.");
            }

            gestorExistente.Departamento = gestor.Departamento;
            gestorExistente.IsAdmin = gestor.IsAdmin;
            gestorExistente.IdUtilizador = gestor.IdUtilizador;

            await _repository.UpdateAsync(gestorExistente);

            return Ok("Gestor atualizado com sucesso!");
        }

        // DELETE /api/gestor/{id}
        // Remove um gestor
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Gestor? gestor = await _repository.GetByIdAsync(id);

            if (gestor == null)
            {
                return NotFound($"Gestor com ID {id} não encontrado.");
            }

            await _repository.DeleteAsync(gestor);

            return Ok($"Gestor com ID {id} removido com sucesso!");
        }
    }
}