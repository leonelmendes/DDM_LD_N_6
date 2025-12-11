using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.DTOs
{
    public class LoginResponseModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int IdGestor { get; set; }
        public int IdProgramador { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoUtilizador { get; set; } = string.Empty;
    }
}
