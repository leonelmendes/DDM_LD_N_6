using iTask_App_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Services.TipoTarefaService
{
    public interface ITipoTarefaService
    {
        Task<List<TipoTarefaModel>> GetAllAsync();
        Task<bool> CreateTipoTarefaAsync(TipoTarefaModel dto);
    }
}
