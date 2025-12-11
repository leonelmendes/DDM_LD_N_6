namespace iTaskAPI.Models.DTOs
{
    public class ProgramadorDetalhe
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string GestorResponsavel { get; set; } = string.Empty;
        public string NivelExperiencia { get; set; } = string.Empty;
    }
}
