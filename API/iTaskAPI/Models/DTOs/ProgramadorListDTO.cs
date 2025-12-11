namespace iTaskAPI.Models.DTOs
{
    public class ProgramadorListDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string NivelExperiencia { get; set; } = string.Empty;
        public int IdUtilizador { get; set; }
    }
}
