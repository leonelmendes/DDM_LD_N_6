namespace iTaskAPI.Models.DTOs
{
    public class RegisterManagerDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;
    }
}