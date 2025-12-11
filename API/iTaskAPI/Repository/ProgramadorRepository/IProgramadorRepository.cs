using iTaskAPI.Models;
using iTaskAPI.Models.DTOs;

namespace iTaskAPI.Repository.ProgramadorRepository
{
    public interface IProgramadorRepository
    {
        Task<IEnumerable<Programador>> GetAllAsync();
        Task<List<ProgramadorCardEquipeDTO>> GetProgramadoresParaEquipeAsync();
        // Traz apenas os programadores de um Gestor
        Task<List<ProgramadorCardEquipeDTO>> GetProgramadoresPorGestorAsync(int idGestor);
        Task<Programador?> GetByIdAsync(int id);
        Task<IEnumerable<Tarefa>> GetTarefasByProgramadorAsync(int programadorId);
        Task<ProgramadorDetalhe> GetDetalheByIdAsync(int id);
        Task AddAsync(Programador programador);
        Task<bool> UpdateAsync(ProgramadorDetalhe programador);
        Task DeleteAsync(Programador programador);
        Task SaveAsync();
    }
}