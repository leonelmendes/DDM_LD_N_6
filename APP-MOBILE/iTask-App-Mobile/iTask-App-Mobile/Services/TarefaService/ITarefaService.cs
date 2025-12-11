using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.Services.TarefaService
{
    public interface ITarefaService
    {
        // Criar nova tarefa
        Task<bool> CriarTarefaAsync(TarefaModel model);

        // Editar tarefa existente
        Task<bool> AtualizarTarefaAsync(int id, TarefaModel model);

        // Atualizar apenas o estado ("To Do", "Doing", "Done")
        Task<bool> AtualizarEstadoAsync(int id, string novoEstado);

        // Obter tarefa específica
        Task<TarefaModel?> GetTarefaByIdAsync(int id);

        Task<DashboardDTO> GetDashboardGlobalAsync();

        // Obter previsão de entrega (data mais próxima entre as tarefas "To Do")
        Task<double> GetPrevisaoEntregaAsync();

        // Lista todas as tarefas de um gestor
        Task<IEnumerable<TarefaModel>> GetTarefasByGestorAsync(int gestorId);

        // Lista tarefas atribuídas a um programador
        Task<IEnumerable<TarefaModel>> GetTarefasByProgramadorAsync(int programadorId);

        Task<IEnumerable<TarefaDetalhesDTO>> GetTarefasDetalhesAsync();

        // Deletar tarefa
        Task<bool> DeleteTarefaAsync(int id);
    }
}
