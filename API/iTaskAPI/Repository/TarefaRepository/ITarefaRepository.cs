using iTaskAPI.Models;

namespace iTaskAPI.Repository.TarefaRepository
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> GetAllAsync();
        Task<Tarefa?> GetByIdAsync(int id);
        Task<IEnumerable<Tarefa>> GetByGestorAsync(int gestorId);
        Task<IEnumerable<Tarefa>> GetByProgramadorAsync(int programadorId);
        Task AddAsync(Tarefa tarefa);
        Task UpdateAsync(Tarefa tarefa);
        Task DeleteAsync(Tarefa tarefa);
        Task SaveAsync();
    }
}