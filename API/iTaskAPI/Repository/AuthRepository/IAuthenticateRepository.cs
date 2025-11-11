using iTaskAPI.Models;

namespace iTaskAPI.Repository.AuthRepository

{
    public interface IAuthenticateRepository
    {
        Task<Utilizador?> LoginAsync(string username, string password);
        Task<Utilizador> RegisterAsync(Utilizador utilizador);
    }
}