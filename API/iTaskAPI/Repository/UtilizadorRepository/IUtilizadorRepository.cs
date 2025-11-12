using iTaskAPI.Models;

namespace iTaskAPI.Repository.UtilizadorRepository
{
    public interface IUtilizadorRepository
    {
        Task<IEnumerable<Utilizador>> GetAllAsync();
        Task<Utilizador?> GetByIdAsync(int id);
        Task<Utilizador?> GetByUsernameAsync(string username);
        Task AddAsync(Utilizador utilizador);
        Task UpdateAsync(Utilizador utilizador);
        Task DeleteAsync(Utilizador utilizador);
        Task SaveAsync();
    }
}