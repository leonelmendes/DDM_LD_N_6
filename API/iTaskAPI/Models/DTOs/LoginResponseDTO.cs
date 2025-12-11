namespace iTaskAPI.Models.DTOs
{
    public class LoginResponseDTO
    {
        public int Id { get; set; }
        public int IdGestor { get; set; }
        public int IdProgramador { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoUtilizador { get; set; } = string.Empty;
    }
}
