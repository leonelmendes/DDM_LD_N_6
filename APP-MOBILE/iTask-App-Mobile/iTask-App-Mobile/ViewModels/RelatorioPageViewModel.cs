using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.Services.TarefaService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.ViewModels
{
    public partial class RelatorioPageViewModel : ObservableObject
    {
        private readonly ITarefaService _tarefaService;

        public RelatorioPageViewModel(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [RelayCommand]
        public async Task ExportarCSV()
        {
            try
            {
                // 1. Busca dados da API
                var todasTarefas = await _tarefaService.GetTarefasDetalhesAsync();

                // 2. Filtra: Criadas pelo Gestor Logado E Status "Done"
                int idGestorLogado = Preferences.Get("gestor_id", 1);
                var tarefasFiltradas = todasTarefas
                    .Where(t => t.IdGestor == idGestorLogado && t.EstadoAtual == "Done")
                    .ToList();

                if (!tarefasFiltradas.Any())
                {
                    await App.Current.MainPage.DisplayAlert("Aviso", "Não há tarefas concluídas para exportar.", "OK");
                    return;
                }

                // 3. Gera o conteúdo do CSV (Separado por ponto e vírgula ';')
                var csv = new StringBuilder();

                // Cabeçalho
                csv.AppendLine("Programador;Descricao;DataPrevistaInicio;DataPrevistaFim;TipoTarefa;DataRealInicio;DataRealFim");

                // Linhas
                foreach (var t in tarefasFiltradas)
                {
                    string linha = $"{t.ProgramadorNome};{t.Descricao};{FormatDate(t.DataPrevistaInicio)};{FormatDate(t.DataPrevistaFim)};{t.TipoTarefaNome};{FormatDate(t.DataRealInicio)};{FormatDate(t.DataRealFim)}";
                    csv.AppendLine(linha);
                }

                // 4. Salvar o arquivo
                string nomeArquivo = $"Relatorio_Tarefas_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                string caminhoArquivo = Path.Combine(FileSystem.CacheDirectory, nomeArquivo);

                File.WriteAllText(caminhoArquivo, csv.ToString(), Encoding.UTF8);

                // 5. Compartilhar o arquivo (Permite salvar ou enviar por email/whatsapp)
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Exportar Relatório de Tarefas",
                    File = new ShareFile(caminhoArquivo)
                });

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", $"Falha ao exportar: {ex.Message}", "OK");
            }
        }

        // Helper para formatar data ou retornar vazio
        private string FormatDate(DateTime? data)
        {
            return data.HasValue ? data.Value.ToString("dd/MM/yyyy") : "";
        }
    }
}
