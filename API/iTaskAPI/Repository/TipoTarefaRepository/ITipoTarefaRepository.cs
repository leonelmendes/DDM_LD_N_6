using iTaskAPI.Models;

namespace iTaskAPI.Repository.TipoTarefaRepository
{
    public interface ITipoTarefaRepository
    {
        Task<IEnumerable<TipoTarefa>> GetAllAsync();
        Task<TipoTarefa?> GetByIdAsync(int id);
        Task AddAsync(TipoTarefa tipoTarefa);
        Task UpdateAsync(TipoTarefa tipoTarefa);
        Task DeleteAsync(TipoTarefa tipoTarefa);
        Task SaveAsync();
    }
}