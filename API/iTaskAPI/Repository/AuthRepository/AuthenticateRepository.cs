using iTaskAPI.Connection;
using iTaskAPI.Models;
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
        public async Task<Utilizador?> LoginAsync(string username, string password)
        {
            Utilizador? utilizador = await _connection.Utilizadores
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            return utilizador;
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