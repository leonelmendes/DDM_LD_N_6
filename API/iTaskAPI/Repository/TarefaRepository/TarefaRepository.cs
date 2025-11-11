using iTaskAPI.Connection;
using iTaskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace iTaskAPI.Repository.TarefaRepository
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly ConnectionDB _connection;
        public TarefaRepository(ConnectionDB connection)
        {
            _connection = connection;
        }
        public async Task AddAsync(Tarefa tarefa)
        {
            await _connection.Tarefas.AddAsync(tarefa);
            await _connection.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tarefa tarefa)
        {
            _connection.Tarefas.Remove(tarefa);
            await _connection.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tarefa>> GetAllAsync()
        {
            var tarefas = await _connection.Tarefas
                .Include(t => t.Gestor)
                .Include(t => t.Programador)
                .Include(t => t.TipoTarefa)
                .ToListAsync();

            return tarefas;
        }

        public async Task<IEnumerable<Tarefa>> GetByGestorAsync(int gestorId)
        {
            var tarefas = await _connection.Tarefas
                .Where(t => t.IdGestor == gestorId)
                .Include(t => t.Programador)
                .Include(t => t.TipoTarefa)
                .ToListAsync();

            return tarefas;
        }

        public async Task<Tarefa?> GetByIdAsync(int id)
        {
           var tarefa = await _connection.Tarefas
                .Include(t => t.Gestor)
                .Include(t => t.Programador)
                .Include(t => t.TipoTarefa)
                .FirstOrDefaultAsync(t => t.Id == id);

            return tarefa;
        }

        public async Task<IEnumerable<Tarefa>> GetByProgramadorAsync(int programadorId)
        {
            var tarefas = await _connection.Tarefas
                .Where(t => t.IdProgramador == programadorId)
                .Include(t => t.Gestor)
                .Include(t => t.TipoTarefa)
                .ToListAsync();

            return tarefas;

        }

        public async Task SaveAsync()
        {
            await _connection.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tarefa tarefa)
        {
            _connection.Tarefas.Update(tarefa);
            await _connection.SaveChangesAsync();
        }
    }
}