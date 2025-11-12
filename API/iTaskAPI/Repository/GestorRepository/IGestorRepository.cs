using iTaskAPI.Models;

namespace iTaskAPI.Repository.GestorRepository
{
    public interface IGestorRepository
    {
        Task<IEnumerable<Gestor>> GetAllAsync();
        Task<Gestor?> GetByIdAsync(int id);
        Task<IEnumerable<Programador>> GetProgramadoresByGestorAsync(int gestorId);
        Task AddAsync(Gestor gestor);
        Task UpdateAsync(Gestor gestor);
        Task DeleteAsync(Gestor gestor);
        Task SaveAsync();
    }
}