using iTaskAPI.Connection;
using iTaskAPI.Models;
using iTaskAPI.Models.DTOs;
using iTaskAPI.Repository.AuthRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace iTaskAPI.Controllers.AuthenticateControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly ConnectionDB _connection;
        private readonly IAuthenticateRepository _repository;
        public AuthenticateController(IAuthenticateRepository repository, ConnectionDB connection)
        {
            _repository = repository;
            _connection = connection;
        }

        // POST /api/authenticate/login
        // Faz o login de um utilizador existente
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDTO loginData)
        {
            if (loginData == null || string.IsNullOrEmpty(loginData.Username) || string.IsNullOrEmpty(loginData.Password))
            {
                return BadRequest("Credenciais inválidas.");
            }

            var utilizador = await _repository.LoginAsync(loginData.Username, loginData.Password);

            if (utilizador == null)
            {
                return Unauthorized("Username ou senha incorretos.");
            }

            return Ok(utilizador);
        }

        // POST /api/authenticate/register
        // Regista um novo utilizador
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] Utilizador novoUtilizador)
        {
            if (novoUtilizador == null || string.IsNullOrEmpty(novoUtilizador.Username) || string.IsNullOrEmpty(novoUtilizador.Password))
            {
                return BadRequest("Dados de utilizador inválidos.");
            }

            Utilizador utilizador = await _repository.RegisterAsync(novoUtilizador);

            return CreatedAtAction(nameof(Register), new { id = utilizador.Id }, new
            {
                message = "Utilizador registado com sucesso.",
                utilizador.Id,
                utilizador.Nome,
                utilizador.Username
            });
        }

        // POST /api/authenticate/register-manager
        // Regista um novo utilizador e cria automaticamente um Gestor associado
        [HttpPost("register-manager")]
        public async Task<ActionResult> RegisterManager([FromBody] RegisterManagerDTO data)
        {
            if (data == null ||
                string.IsNullOrEmpty(data.Username) ||
                string.IsNullOrEmpty(data.Password) ||
                string.IsNullOrEmpty(data.Email) ||
                string.IsNullOrEmpty(data.Nome) ||
                string.IsNullOrEmpty(data.Departamento))
            {
                return BadRequest("Todos os campos são obrigatórios.");
            }

            bool usernameExists = await _connection.Utilizadores.AnyAsync(u => u.Username == data.Username);
            bool emailExists = await _connection.Utilizadores.AnyAsync(u => u.Email == data.Email);

            if (usernameExists)
                return Conflict("O nome de utilizador já está em uso.");
            if (emailExists)
                return Conflict("O email já está em uso.");

            // 1️⃣ Cria o utilizador
            var utilizador = new Utilizador
            {
                Nome = data.Nome,
                Email = data.Email,
                Username = data.Username,
                Password = data.Password
            };

            await _connection.Utilizadores.AddAsync(utilizador);
            await _connection.SaveChangesAsync();

            // 2️⃣ Cria o gestor associado
            var gestor = new Gestor
            {
                IdUtilizador = utilizador.Id,
                Departamento = data.Departamento,
                IsAdmin = data.IsAdmin
            };

            await _connection.Gestores.AddAsync(gestor);
            await _connection.SaveChangesAsync();

            return Ok(new
            {
                message = "Gestor registado com sucesso.",
                utilizador = new
                {
                    utilizador.Id,
                    utilizador.Nome,
                    utilizador.Email,
                    utilizador.Username
                },
                gestor = new
                {
                    gestor.Id,
                    gestor.Departamento,
                    gestor.IsAdmin
                }
            });
        }


        // POST /api/authenticate/register-programmer
        // Regista um novo utilizador e cria um Programador vinculado a um Gestor existente
        [HttpPost("register-programmer")]
        public async Task<ActionResult> RegisterProgrammer([FromQuery] int idGestor, [FromBody] RegisterProgrammerDTO data)
        {
            if (data == null ||
                string.IsNullOrEmpty(data.Username) ||
                string.IsNullOrEmpty(data.Password) ||
                string.IsNullOrEmpty(data.Email) ||
                string.IsNullOrEmpty(data.Nome) ||
                string.IsNullOrEmpty(data.NivelExperiencia))
            {
                return BadRequest("Todos os campos são obrigatórios.");
            }

            var gestor = await _connection.Gestores.FindAsync(idGestor);
            if (gestor == null)
                return NotFound($"Gestor com ID {idGestor} não encontrado.");

            bool usernameExists = await _connection.Utilizadores.AnyAsync(u => u.Username == data.Username);
            bool emailExists = await _connection.Utilizadores.AnyAsync(u => u.Email == data.Email);

            if (usernameExists)
                return Conflict("O nome de utilizador já está em uso.");
            if (emailExists)
                return Conflict("O email já está em uso.");

            // 1️⃣ Cria o utilizador
            var utilizador = new Utilizador
            {
                Nome = data.Nome,
                Email = data.Email,
                Username = data.Username,
                Password = data.Password
            };

            await _connection.Utilizadores.AddAsync(utilizador);
            await _connection.SaveChangesAsync();

            // 2️⃣ Cria o programador vinculado ao gestor
            var programador = new Programador
            {
                IdGestor = idGestor,
                IdUtilizador = utilizador.Id,
                NivelExperiencia = data.NivelExperiencia
            };

            await _connection.Programadores.AddAsync(programador);
            await _connection.SaveChangesAsync();

            return Ok(new
            {
                message = "Programador registado com sucesso.",
                utilizador = new
                {
                    utilizador.Id,
                    utilizador.Nome,
                    utilizador.Email,
                    utilizador.Username
                },
                programador = new
                {
                    programador.Id,
                    programador.NivelExperiencia,
                    programador.IdGestor
                }
            });
        }
    }
}