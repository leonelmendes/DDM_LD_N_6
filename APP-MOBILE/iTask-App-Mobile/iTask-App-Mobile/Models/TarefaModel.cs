using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Models
{
    public enum StatusTarefa { ToDo, Doing, Done }

    public class TarefaModel : ObservableObject
    {
        public int Id { get; set; }

        private string _titulo = string.Empty;
        public string Titulo
        {
            get => _titulo;
            set => SetProperty(ref _titulo, value);
        }

        public string Descricao { get; set; } = string.Empty;

        // ESSENCIAL: Notificar a mudança de estado
        private string _estadoAtual = string.Empty;
        public string EstadoAtual
        {
            get => _estadoAtual;
            set => SetProperty(ref _estadoAtual, value);
        }

        public string OrdemExecucao { get; set; } = string.Empty;
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
