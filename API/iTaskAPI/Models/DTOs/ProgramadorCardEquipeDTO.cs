namespace iTaskAPI.Models.DTOs
{
    public class ProgramadorCardEquipeDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NivelExperiencia { get; set; } // "Junior", "Mid", "Senior"

        // Tarefas "To Do" + "Doing"
        public int Ativas { get; set; }
        // Tarefas "Done"
        public int Concluidas { get; set; }
    }
}
