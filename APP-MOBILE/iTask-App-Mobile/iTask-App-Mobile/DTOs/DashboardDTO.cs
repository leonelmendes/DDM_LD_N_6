using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.DTOs
{
    public class DashboardDTO
    {
        public int TotalTarefas { get; set; }
        public int TarefasToDo { get; set; }
        public int TarefasDoing { get; set; }
        public int TarefasDone { get; set; }
        public double HorasPrevistasToDo { get; set; }
        public string PrevisaoTexto { get; set; }
    }
}
