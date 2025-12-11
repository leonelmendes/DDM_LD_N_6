using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.DTOs
{
    public class ProgramadorCardEquipeDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NivelExperiencia { get; set; } // "Junior", "Mid", "Senior"

        // Propriedades para o Dashboard/Card
        public int Ativas { get; set; }
        public int Concluidas { get; set; }

        // Propriedades calculadas para a View (Helpers)
        public int TaxaConclusao => (Ativas + Concluidas) == 0 ? 0 : (int)((double)Concluidas / (Ativas + Concluidas) * 100);

        // Largura da barra de progresso (Baseado no WidthRequest=160 do teu XAML)
        public double ProgressoWidth => (Ativas + Concluidas) == 0 ? 0 : ((double)Concluidas / (Ativas + Concluidas)) * 160;
    }
}
