using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Models
{
    public class TarefaModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string EstadoAtual { get; set; } = string.Empty;
        public int IdGestor { get; set; }
        public int IdProgramador { get; set; }
        public int IdTipoTarefa { get; set; }
        public DateTime? DataPrevistaInicio { get; set; }
        public DateTime? DataPrevistaFim { get; set; }
        public DateTime? DataRealInicio { get; set; }
        public DateTime? DataRealFim { get; set; }
        public int? StoryPoints { get; set; }
    }
}
