using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Services.UtilizadorService
{
    public interface IUtilizadorService
    {
        Task<UtilizadorModel> GetByIdAsync(int id);
        Task<bool> UpdateUtilizadorAsync(int id, UtilizadorModel dto);

        Task<bool> AtualizarPerfilGestorAsync(UpdateGestorProfileDTO dto);
        Task<bool> AtualizarPerfilProgramadorAsync(UpdateProgramadorProfileDTO dto);
    }
}
