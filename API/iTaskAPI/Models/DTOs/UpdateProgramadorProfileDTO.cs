namespace iTaskAPI.Models.DTOs
{
    public class UpdateProgramadorProfileDTO
    {
        public int Id { get; set; } // Id do Programador
        public string? Nome { get; set; }
        public string? Username { get; set; }
        public string? NivelExperiencia { get; set; }
        public string? Password { get; set; }
    }
}
