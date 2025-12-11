using iTaskAPI.Connection;
using iTaskAPI.Models;
using iTaskAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace iTaskAPI.Repository.TarefaRepository
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly ConnectionDB _connection;
        public TarefaRepository(ConnectionDB connection)
        {
            _connection = connection;
        }

        public async Task<bool> AtualizarEstadoAsync(int id, string novoEstado)
        {
            var tarefa = await _connection.Tarefas.FindAsync(id);
            if (tarefa == null) return false;

            // Lógica para preencher datas reais automaticamente
            if (novoEstado == "Doing" && tarefa.EstadoAtual == "To Do")
            {
                // Começou agora
                tarefa.DataRealInicio = DateTime.UtcNow;
            }
            else if (novoEstado == "Done" && tarefa.EstadoAtual == "Doing")
            {
                // Terminou agora
                tarefa.DataRealFim = DateTime.UtcNow;

                // Proteção: Se por acaso DataRealInicio for null (erro antigo), define agora para não quebrar o cálculo
                if (tarefa.DataRealInicio == null)
                {
                    tarefa.DataRealInicio = tarefa.DataCriacao; // Fallback
                }
            }

            tarefa.EstadoAtual = novoEstado;

            await _connection.SaveChangesAsync();
            return true;
        }

        public async Task<DashboardGestorDTO> GetDashboardGlobalAsync()
        {
            // 1. Buscar TODAS as tarefas do sistema (Visão Global)
            var tarefas = await _connection.Tarefas.ToListAsync();

            // 2. Calcular Contadores Básicos
            var dto = new DashboardGestorDTO
            {
                TotalTarefas = tarefas.Count,
                TarefasToDo = tarefas.Count(t => t.EstadoAtual == "To Do"),
                TarefasDoing = tarefas.Count(t => t.EstadoAtual == "Doing"),
                TarefasDone = tarefas.Count(t => t.EstadoAtual == "Done")
            };

            // 3. ALGORITMO PREDITIVO (Requisito 28)

            // A. Pegar histórico válido (Tarefas Done com datas reais e StoryPoints)
            var historicoConcluidas = tarefas
                .Where(t => t.EstadoAtual == "Done" &&
                            t.DataRealInicio != null &&
                            t.DataRealFim != null &&
                            t.StoryPoints > 0)
                .ToList();

            // B. Pegar trabalho pendente (To Do com StoryPoints)
            var pendentes = tarefas
                .Where(t => t.EstadoAtual == "To Do" && t.StoryPoints > 0)
                .ToList();

            double horasTotais = 0;

            // Só calculamos se houver histórico e pendências
            if (historicoConcluidas.Any() && pendentes.Any())
            {
                // C. Calcular média de horas por StoryPoint (SP)
                // Dicionário: [Chave: SP] -> [Valor: Média Horas]
                var mediasPorSP = historicoConcluidas
                    .GroupBy(t => t.StoryPoints.Value)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Average(t => (t.DataRealFim.Value - t.DataRealInicio.Value).TotalHours)
                    );

                var chavesHistorico = mediasPorSP.Keys.ToList();

                // D. Iterar sobre o To Do e somar previsões
                foreach (var item in pendentes)
                {
                    int spAlvo = item.StoryPoints.Value;

                    if (mediasPorSP.ContainsKey(spAlvo))
                    {
                        // Temos histórico exato
                        horasTotais += mediasPorSP[spAlvo];
                    }
                    else
                    {
                        // "Nearest Neighbor": Pega a média do SP mais próximo matematicamente
                        int maisProximo = chavesHistorico.OrderBy(k => Math.Abs(k - spAlvo)).First();
                        horasTotais += mediasPorSP[maisProximo];
                    }
                }
            }

            // 4. Formatar Resultado
            dto.HorasPrevistasToDo = horasTotais;

            if (horasTotais > 0)
            {
                TimeSpan ts = TimeSpan.FromHours(horasTotais);
                // Formatação bonita para o Mobile
                dto.PrevisaoTexto = ts.Days > 0
                    ? $"{ts.Days} dias e {ts.Hours}h"
                    : $"{ts.Hours} horas";
            }
            else
            {
                dto.PrevisaoTexto = pendentes.Any() ? "Sem dados históricos" : "Tudo em dia!";
            }

            return dto;
        }
        public async Task AddAsync(Tarefa tarefa)
        {
            tarefa.DataPrevistaInicio = DateTime.SpecifyKind(tarefa.DataPrevistaInicio.Value, DateTimeKind.Unspecified);
            tarefa.DataPrevistaFim = DateTime.SpecifyKind(tarefa.DataPrevistaFim.Value, DateTimeKind.Unspecified);

            if (tarefa.DataRealInicio.HasValue)
                tarefa.DataRealInicio = DateTime.SpecifyKind(tarefa.DataRealInicio.Value, DateTimeKind.Unspecified);

            if (tarefa.DataRealFim.HasValue)
                tarefa.DataRealFim = DateTime.SpecifyKind(tarefa.DataRealFim.Value, DateTimeKind.Unspecified);

            tarefa.DataCriacao = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

            await _connection.Tarefas.AddAsync(tarefa);
            await _connection.SaveChangesAsync();
        }

        public async Task<double> CalcularPrevisaoTempoToDoAsync()
        {
            // 1. Buscar dados históricos (Tarefas DONE com datas válidas e StoryPoints > 0)
            var tarefasConcluidas = await _connection.Tarefas
                .Where(t => t.EstadoAtual == "Done" &&
                            t.DataRealInicio != null &&
                            t.DataRealFim != null &&
                            t.StoryPoints > 0)
                .ToListAsync();

            // 2. Buscar o trabalho pendente (Tarefas TODO)
            var tarefasToDo = await _connection.Tarefas
                .Where(t => t.EstadoAtual == "To Do" && t.StoryPoints > 0)
                .ToListAsync();

            if (!tarefasToDo.Any()) return 0; // Nada para fazer

            // 3. Criar Dicionário de Médias: [StoryPoints] -> [MediaHoras]
            // Agrupa por SP e calcula a média de tempo (Fim - Inicio) em Horas
            var mediasPorSP = tarefasConcluidas
                .GroupBy(t => t.StoryPoints.Value)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo.Average(t => (t.DataRealFim.Value - t.DataRealInicio.Value).TotalHours)
                );

            // Se não houver nenhum histórico, não dá para prever (ou retorna 0 ou lança erro)
            if (!mediasPorSP.Any()) return 0;

            double tempoTotalPrevisto = 0;
            var chavesComHistorico = mediasPorSP.Keys.ToList();

            // 4. Algoritmo de Previsão
            foreach (var tarefa in tarefasToDo)
            {
                int spAlvo = tarefa.StoryPoints.Value;

                if (mediasPorSP.ContainsKey(spAlvo))
                {
                    // Cenário Ideal: Temos histórico exato para esses pontos
                    tempoTotalPrevisto += mediasPorSP[spAlvo];
                }
                else
                {
                    // Cenário "Mais Próximo": Encontrar o SP mais próximo matematicamente
                    // Ex: Alvo = 4. Temos histórico de 2 e 8. 
                    // |2 - 4| = 2
                    // |8 - 4| = 4
                    // O mais próximo é 2. Usamos a média do 2.

                    int spMaisProximo = chavesComHistorico
                        .OrderBy(k => Math.Abs(k - spAlvo)) // Ordena pela menor diferença absoluta
                        .First();

                    tempoTotalPrevisto += mediasPorSP[spMaisProximo];
                }
            }

            return tempoTotalPrevisto;
        }

        public async Task<IEnumerable<TarefaDetalhesDTO>> GetAllTarefasDetalhesAsync()
        {
            // Acessa a tabela Tarefas, inclui as tabelas relacionadas e projeta para o DTO
            return await _connection.Tarefas
                // Inclui as entidades necessárias para acessar os nomes
                .Include(t => t.TipoTarefa)
                .Include(t => t.Gestor)
                    .ThenInclude(g => g.Utilizador) // Navega de Gestor para Utilizador para pegar o Nome
                .Include(t => t.Programador)
                    .ThenInclude(p => p.Utilizador) // Pega o nome do Programador
                .Select(t => new TarefaDetalhesDTO
                {
                    // Mapeamento de Propriedades da Tarefa
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descricao = t.Descricao,
                    EstadoAtual = t.EstadoAtual,
                    OrdemExecucao = t.OrdemExecucao,
                    DataPrevistaInicio = t.DataPrevistaInicio,
                    DataPrevistaFim = t.DataPrevistaFim,
                    DataRealInicio = t.DataRealInicio,
                    DataRealFim = t.DataRealFim,
                    StoryPoints = t.StoryPoints,

                    // Mapeamento dos Nomes (Join)
                    GestorNome = t.Gestor.Utilizador.Nome, // Tarefa -> Gestor -> Utilizador -> Nome
                    ProgramadorNome = t.Programador.Utilizador.Nome,
                    TipoTarefaNome = t.TipoTarefa.Nome,

                    // IDs (mantidos)
                    IdTipoTarefa = t.IdTipoTarefa,
                    IdGestor = t.IdGestor,
                    IdProgramador = t.IdProgramador
                })
                .ToListAsync();
        }

        public async Task DeleteAsync(Tarefa tarefa)
        {
            _connection.Tarefas.Remove(tarefa);
            await _connection.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tarefa>> GetAllAsync()
        {
            var tarefas = await _connection.Tarefas
                .Include(t => t.Gestor)
                .Include(t => t.Programador)
                .Include(t => t.TipoTarefa)
                .ToListAsync();

            return tarefas;
        }

        public async Task<IEnumerable<Tarefa>> GetByGestorAsync(int gestorId)
        {
            var tarefas = await _connection.Tarefas
                .Where(t => t.IdGestor == gestorId)
                .Include(t => t.Programador)
                .Include(t => t.TipoTarefa)
                .ToListAsync();

            return tarefas;
        }

        public async Task<Tarefa?> GetByIdAsync(int id)
        {
           var tarefa = await _connection.Tarefas
                .Include(t => t.Gestor)
                .Include(t => t.Programador)
                .Include(t => t.TipoTarefa)
                .FirstOrDefaultAsync(t => t.Id == id);

            return tarefa;
        }

        public async Task<IEnumerable<Tarefa>> GetByProgramadorAsync(int programadorId)
        {
            var tarefas = await _connection.Tarefas
                .Where(t => t.IdProgramador == programadorId)
                .Include(t => t.Gestor)
                .Include(t => t.TipoTarefa)
                .ToListAsync();

            return tarefas;

        }

        public async Task SaveAsync()
        {
            await _connection.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tarefa tarefa)
        {
            _connection.Tarefas.Update(tarefa);
            await _connection.SaveChangesAsync();
        }
    }
}