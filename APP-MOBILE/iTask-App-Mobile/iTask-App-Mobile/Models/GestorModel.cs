using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Models
{
    public class GestorModel
    {
        public int Id { get; set; }
        public string Departamento { get; set; } = string.Empty;
        public int IdUtilizador { get; set; }
    }
}
