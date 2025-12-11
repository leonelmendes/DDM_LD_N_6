using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.DTOs
{
    public class UpdateProgramadorProfileDTO
    {
        public int Id { get; set; } // Id do Programador
        public string? Nome { get; set; }
        public string? Username { get; set; }
        public string? NivelExperiencia { get; set; }
        public string? Password { get; set; }
    }
}
