using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Models;

namespace iTask_App_Mobile.Services.ProgramadorService
{
    public interface IProgramadorService
    {
        // Lista TODOS os programadores
        Task<IEnumerable<ProgramadorListDTO>> GetAllProgramadoresAsync();

        // Lista programadores de um determinado gestor
        Task<IEnumerable<ProgramadorListDTO>> GetProgramadoresByGestorAsync(int gestorId);

        // Buscar um programador pelo ID
        Task<ProgramadorDetalhe?> GetProgramadorByIdAsync(int id);

        // Criar programador
        Task<bool> CreateProgramadorAsync(CreateProgramadorDTO model);

        Task<DashboardDTO> GetDashboardProgramadorAsync(int idUtilizador);

        // Editar programador
        Task<bool> UpdateProgramadorAsync(ProgramadorDetalhe programador);

        // Remover programador
        Task<bool> DeleteProgramadorAsync(int id);

        // 
        Task<ProgramadorDetalhe> GetDetalheAsync(int id);

        // Traz programadores com atributos para preencher o card equipe
        Task<List<ProgramadorCardEquipeDTO>> GetAllAsync();

        Task<List<ProgramadorCardEquipeDTO>> GetByGestorAsync(int idGestor); // Usa a rota /equipecards/{id}
    }
}
