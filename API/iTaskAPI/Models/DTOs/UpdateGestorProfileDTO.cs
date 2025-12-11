namespace iTaskAPI.Models.DTOs
{
    public class UpdateGestorProfileDTO
    {
        public int Id { get; set; } // Id do Gestor
        public string? Nome { get; set; }
        public string? Username { get; set; }
        public string? Departamento { get; set; }
        // Password é opcional, só atualiza se vier preenchida
        public string? Password { get; set; }
    }
}
