using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.DTOs
{
    public class TarefaDetalhesDTO : ObservableObject
    {
        public int Id { get; set; }

        // Propriedades Editáveis
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? OrdemExecucao { get; set; }
        public DateTime? DataPrevistaInicio { get; set; }
        public DateTime? DataPrevistaFim { get; set; }
        public int? StoryPoints { get; set; }

        // Propriedades de Estado
        public string? EstadoAtual { get; set; }
        public int IdTipoTarefa { get; set; }
        public int IdProgramador { get; set; }

        // Dados de Relação (Vem do Join, Apenas para exibição - ReadOnly)
        public string? GestorNome { get; set; }
        public string? ProgramadorNome { get; set; }
        public string? TipoTarefaNome { get; set; }

        // Datas Reais (ReadOnly, exceto em lógica específica de transição de estado)
        public DateTime? DataRealInicio { get; set; }
        public DateTime? DataRealFim { get; set; }
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
