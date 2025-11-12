using iTaskAPI.Connection;
using iTaskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace iTaskAPI.Repository.ProgramadorRepository
{
    public class ProgramadorRepository : IProgramadorRepository
    {
        private readonly ConnectionDB _connection;

        public ProgramadorRepository(ConnectionDB connection)
        {
            _connection = connection;
        }

        // GET /api/programador
        // Retorna todos os programadores
        public async Task<IEnumerable<Programador>> GetAllAsync()
        {
            IEnumerable<Programador> programadores = await _connection.Programadores
                .Include(p => p.Utilizador)
                .Include(p => p.Gestor)
                .ToListAsync();

            return programadores;
        }

        // GET /api/programador/{id}
        // Retorna um programador específico
        public async Task<Programador?> GetByIdAsync(int id)
        {
            Programador? programador = await _connection.Programadores
                .Include(p => p.Utilizador)
                .Include(p => p.Gestor)
                .Include(p => p.Tarefas)
                .FirstOrDefaultAsync(p => p.Id == id);

            return programador;
        }

        // GET /api/programador/{id}/tarefas
        // Retorna todas as tarefas atribuídas a um programador
        public async Task<IEnumerable<Tarefa>> GetTarefasByProgramadorAsync(int programadorId)
        {
            IEnumerable<Tarefa> tarefas = await _connection.Tarefas
                .Include(t => t.Gestor)
                .Include(t => t.TipoTarefa)
                .Where(t => t.IdProgramador == programadorId)
                .ToListAsync();

            return tarefas;
        }

        // POST /api/programador
        // Cria um novo programador
        public async Task AddAsync(Programador programador)
        {
            await _connection.Programadores.AddAsync(programador);
            await _connection.SaveChangesAsync();
        }

        // PUT /api/programador/{id}
        // Atualiza os dados de um programador
        public async Task UpdateAsync(Programador programador)
        {
            _connection.Programadores.Update(programador);
            await _connection.SaveChangesAsync();
        }

        // DELETE /api/programador/{id}
        // Remove um programador
        public async Task DeleteAsync(Programador programador)
        {
            _connection.Programadores.Remove(programador);
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