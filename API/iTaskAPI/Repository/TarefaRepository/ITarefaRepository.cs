using iTaskAPI.Models;
using iTaskAPI.Models.DTOs;

namespace iTaskAPI.Repository.TarefaRepository
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> GetAllAsync();
        Task<IEnumerable<TarefaDetalhesDTO>> GetAllTarefasDetalhesAsync();
        Task<Tarefa?> GetByIdAsync(int id);
        Task<IEnumerable<Tarefa>> GetByGestorAsync(int gestorId);
        Task<IEnumerable<Tarefa>> GetByProgramadorAsync(int programadorId);
        Task<double> CalcularPrevisaoTempoToDoAsync();// meotod para calcular o tempo previsto total das tarefas com estado To Do Algoritmo Preditivo.
        Task AddAsync(Tarefa tarefa);
        Task<bool> AtualizarEstadoAsync(int id, string novoEstado);
        Task UpdateAsync(Tarefa tarefa);
        Task DeleteAsync(Tarefa tarefa);
        Task SaveAsync();
    }
}