using iTaskAPI.Connection;
using iTaskAPI.Models;
using iTaskAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace iTaskAPI.Repository.AuthRepository
{
    public class AuthenticateRepository : IAuthenticateRepository
    {
        private readonly ConnectionDB _connection;

        public AuthenticateRepository(ConnectionDB connection)
        {
            _connection = connection;
        }

        // POST /api/authenticate/login
        // Faz o login do utilizador validando username e password
        public async Task<LoginResponseDTO?> LoginAsync(string username, string password)
        {
            var utilizador = await _connection.Utilizadores
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (utilizador == null)
                return null;

            // Verifica se é gestor
            var gestor = await _connection.Gestores
                .FirstOrDefaultAsync(g => g.IdUtilizador == utilizador.Id);

            if (gestor != null)
            {
                return new LoginResponseDTO
                {
                    Id = utilizador.Id,
                    Nome = utilizador.Nome,
                    Username = utilizador.Username,
                    Email = utilizador.Email,
                    IdGestor = gestor.Id,
                    TipoUtilizador = "Gestor"
                };
            }

            // Verifica se é programador
            var programador = await _connection.Programadores
                .FirstOrDefaultAsync(p => p.IdUtilizador == utilizador.Id);

            if (programador != null)
            {
                return new LoginResponseDTO
                {
                    Id = utilizador.Id,
                    Nome = utilizador.Nome,
                    Username = utilizador.Username,
                    Email = utilizador.Email,
                    IdProgramador = programador.Id,
                    TipoUtilizador = "Programador"
                };
            }

            // Se não for nenhum (caso raro)
            return new LoginResponseDTO
            {
                Id = utilizador.Id,
                Nome = utilizador.Nome,
                Username = utilizador.Username,
                IdGestor = 0,
                IdProgramador = 0,
                Email = utilizador.Email,
                TipoUtilizador = "Desconhecido"
            };
        }

        // POST /api/authenticate/register
        // Regista um novo utilizador
        public async Task<Utilizador> RegisterAsync(Utilizador utilizador)
        {
            await _connection.Utilizadores.AddAsync(utilizador);
            await _connection.SaveChangesAsync();
            return utilizador;
        }
    }
}