using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Models
{
    public class ProgramadorModel
    {
        public int Id { get; set; }
        public string NivelExperiencia { get; set; } = string.Empty;
        public int IdGestor { get; set; }
        public int IdUtilizador { get; set; }
    }
}
