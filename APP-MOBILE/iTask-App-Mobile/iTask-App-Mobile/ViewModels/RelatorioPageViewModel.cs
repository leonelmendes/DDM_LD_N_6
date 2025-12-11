using CommunityToolkit.Maui.Storage;
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
                // 1. Busca e Filtra (Código igual ao anterior)
                var todasTarefas = await _tarefaService.GetTarefasDetalhesAsync();
                int idGestorLogado = Preferences.Get("gestor_id", 1);
                var tarefasFiltradas = todasTarefas
                    .Where(t => t.IdGestor == idGestorLogado && t.EstadoAtual == "Done")
                    .ToList();

                if (!tarefasFiltradas.Any())
                {
                    await App.Current.MainPage.DisplayAlert("Aviso", "Não há tarefas para exportar.", "OK");
                    return;
                }

                // 2. Gera o conteúdo do CSV
                var csv = new StringBuilder();
                csv.AppendLine("Programador;Descricao;DataPrevistaInicio;DataPrevistaFim;TipoTarefa;DataRealInicio;DataRealFim");

                foreach (var t in tarefasFiltradas)
                {
                    // Nota: Importante tratar nulos para evitar crash
                    string linha = $"{t.ProgramadorNome};{t.Descricao};{t.DataPrevistaInicio:dd/MM/yyyy};{t.DataPrevistaFim:dd/MM/yyyy};{t.TipoTarefaNome};{t.DataRealInicio:dd/MM/yyyy};{t.DataRealFim:dd/MM/yyyy}";
                    csv.AppendLine(linha);
                }

                // 3. Prepara o arquivo para SALVAR (FileSaver)
                string nomeArquivo = $"Relatorio_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                // Converte a string do CSV para um Stream de memória
                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv.ToString()));

                // 4. Chama o FileSaver (Abre o "Salvar Como" do Android)
                var resultado = await FileSaver.Default.SaveAsync(nomeArquivo, stream, CancellationToken.None);

                if (resultado.IsSuccessful)
                {
                    await App.Current.MainPage.DisplayAlert("Sucesso", $"Arquivo salvo em: {resultado.FilePath}", "OK");
                }
                else
                {
                    // Se o usuário cancelou ou deu erro
                    if (resultado.Exception != null)
                        await App.Current.MainPage.DisplayAlert("Erro", $"Falha ao salvar: {resultado.Exception.Message}", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro Crítico", ex.Message, "OK");
            }
        }

        // Helper para formatar data ou retornar vazio
        private string FormatDate(DateTime? data)
        {
            return data.HasValue ? data.Value.ToString("dd/MM/yyyy") : "";
        }
    }
}
