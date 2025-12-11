namespace iTaskAPI.Models.DTOs
{
    public class DashboardGestorDTO
    {
        // Contadores
        public int TotalTarefas { get; set; }
        public int TarefasToDo { get; set; }
        public int TarefasDoing { get; set; }
        public int TarefasDone { get; set; }

        // Previsão (Requisito 28)
        public double HorasPrevistasToDo { get; set; }
        public string PrevisaoTexto { get; set; } // Ex: "3 dias e 2 horas"
    }
}
