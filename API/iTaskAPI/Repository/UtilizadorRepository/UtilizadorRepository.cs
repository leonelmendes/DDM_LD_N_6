using iTaskAPI.Connection;
using iTaskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace iTaskAPI.Repository.UtilizadorRepository

{
    public class UtilizadorRepository : IUtilizadorRepository
    {
        private readonly ConnectionDB _connection;

        public UtilizadorRepository(ConnectionDB connection)
        {
            _connection = connection;
        }

        // GET /api/utilizador
        // Retorna todos os utilizadores
        public async Task<IEnumerable<Utilizador>> GetAllAsync()
        {
            IEnumerable<Utilizador> utilizadores = await _connection.Utilizadores.ToListAsync();
            return utilizadores;
        }

        // GET /api/utilizador/{id}
        // Retorna um utilizador específico
        public async Task<Utilizador?> GetByIdAsync(int id)
        {
            Utilizador? utilizador = await _connection.Utilizadores.FirstOrDefaultAsync(u => u.Id == id);
            return utilizador;
        }

        // GET /api/utilizador/username/{username}
        // Retorna um utilizador pelo nome de usuário
        public async Task<Utilizador?> GetByUsernameAsync(string username)
        {
            Utilizador? utilizador = await _connection.Utilizadores.FirstOrDefaultAsync(u => u.Username == username);
            return utilizador;
        }

        // POST /api/utilizador
        // Cria um novo utilizador
        public async Task AddAsync(Utilizador utilizador)
        {
            await _connection.Utilizadores.AddAsync(utilizador);
            await _connection.SaveChangesAsync();
        }

        // PUT /api/utilizador/{id}
        // Atualiza um utilizador existente
        public async Task UpdateAsync(Utilizador utilizador)
        {
            _connection.Utilizadores.Update(utilizador);
            await _connection.SaveChangesAsync();
        }

        // DELETE /api/utilizador/{id}
        // Remove um utilizador
        public async Task DeleteAsync(Utilizador utilizador)
        {
            _connection.Utilizadores.Remove(utilizador);
            await _connection.SaveChangesAsync();
        }

        // SAVE
        // Salva alterações no banco de dados
        public async Task SaveAsync()
        {
            await _connection.SaveChangesAsync();
        }
    }
}