using iTask_App_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Services.GestorService
{
    public interface IGestorService
    {
        Task<GestorModel> GetByIdAsync(int id);
        Task<List<GestorModel>> GetAllAsync();
        Task<bool> CreateGestorAsync(GestorModel dto);
    }
}
