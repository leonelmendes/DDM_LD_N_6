using iTaskAPI.Models;

namespace iTaskAPI.Repository.ProgramadorRepository
{
    public interface IProgramadorRepository
    {
        Task<IEnumerable<Programador>> GetAllAsync();
        Task<Programador?> GetByIdAsync(int id);
        Task<IEnumerable<Tarefa>> GetTarefasByProgramadorAsync(int programadorId);
        Task AddAsync(Programador programador);
        Task UpdateAsync(Programador programador);
        Task DeleteAsync(Programador programador);
        Task SaveAsync();
    }
}