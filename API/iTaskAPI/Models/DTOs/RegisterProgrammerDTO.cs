namespace iTaskAPI.Models.DTOs
{
    public class RegisterProgrammerDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int IdGestor { get; set; }
        public string NivelExperiencia  { get; set; } = string.Empty;
    }
}