namespace iTaskAPI.Models.DTOs
{
    public class TarefaDetalhesDTO
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? EstadoAtual { get; set; }
        public string? OrdemExecucao { get; set; }
        public DateTime? DataPrevistaInicio { get; set; }
        public DateTime? DataPrevistaFim { get; set; }
        public DateTime? DataRealInicio { get; set; }
        public DateTime? DataRealFim { get; set; }
        public int? StoryPoints { get; set; }

        // Propriedades de Navegação (Nomes em vez de IDs)
        public string? GestorNome { get; set; } // Nome do Gestor associado
        public string? ProgramadorNome { get; set; } // Nome do Programador associado
        public string? TipoTarefaNome { get; set; } // Nome do Tipo da Tarefa

        // Mantemos os IDs para facilitar atualizações (PUT/PATCH), se necessário.
        public int IdProgramador { get; set; }
        public int IdTipoTarefa { get; set; }
        public int IdGestor { get; set; }

        // Helper para exibir "Tempo previsto: X dias"
        public string TempoPrevistoTexto
        {
            get
            {
                if (DataPrevistaInicio.HasValue && DataPrevistaFim.HasValue)
                {
                    var dias = (DataPrevistaFim.Value - DataPrevistaInicio.Value).Days;
                    // Garante pelo menos 1 dia se for no mesmo dia, ou ajusta conforme regra de negócio
                    return dias == 0 ? "1 dia" : $"{dias} dias";
                }
                return "N/A";
            }
        }

        // Helper para exibir "Tempo real: Y dias"
        public string TempoRealTexto
        {
            get
            {
                if (DataRealInicio.HasValue && DataRealFim.HasValue)
                {
                    var dias = (DataRealFim.Value - DataRealInicio.Value).Days;
                    return dias == 0 ? "1 dia" : $"{dias} dias";
                }
                return "Pend.";
            }
        }
    }
}
