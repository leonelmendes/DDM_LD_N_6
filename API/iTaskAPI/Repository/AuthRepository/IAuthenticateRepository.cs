using iTaskAPI.Models;
using iTaskAPI.Models.DTOs;

namespace iTaskAPI.Repository.AuthRepository

{
    public interface IAuthenticateRepository
    {
        Task<LoginResponseDTO?> LoginAsync(string username, string password);
        Task<Utilizador> RegisterAsync(Utilizador utilizador);
    }
}