using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.Models;
using iTask_App_Mobile.Services.TarefaService;
using iTask_App_Mobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.ViewModels
{
    public partial class KanbanPageViewModel : ObservableObject
    {
        #region Services
        private readonly ITarefaService _tarefaService;

        #endregion

        #region Constructor
        public KanbanPageViewModel( ITarefaService service)
        {
            _tarefaService = service;
            Task.Run(CarregarTarefasAsync);
        }
        #endregion

        #region Properties
        public ObservableCollection<TarefaModel> ToDoList { get; set; } = new();
        public ObservableCollection<TarefaModel> DoingList { get; set; } = new();
        public ObservableCollection<TarefaModel> DoneList { get; set; } = new();
        #endregion

        #region Controls
        SnackbarOptions snackbaroptionErro = new SnackbarOptions
        {
            BackgroundColor = Color.Parse("#cc3429"),
            CornerRadius = 10,
            ActionButtonTextColor = Colors.White,
            TextColor = Colors.White,
        };

        SnackbarOptions snackbaroptionTrue = new SnackbarOptions
        {
            BackgroundColor = Colors.Green,
            CornerRadius = 10,
            TextColor = Colors.White,
        };
        #endregion

        #region Command
        public async Task CarregarTarefasAsync()
        {
            try
            {
                // Exemplo: Pegando tarefas do Programador 1 (Ajuste conforme sua lógica de login)
                //int idProgramador = 1;
                var tarefas = await _tarefaService.GetTarefasByProgramadorAsync(Preferences.Get("programador_id", 0));

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ToDoList.Clear();
                    DoingList.Clear();
                    DoneList.Clear();

                    foreach (var tarefa in tarefas)
                    {
                        switch (tarefa.EstadoAtual)
                        {
                            case "To Do": ToDoList.Add(tarefa); break;
                            case "Doing": DoingList.Add(tarefa); break;
                            case "Done": DoneList.Add(tarefa); break;
                            default: ToDoList.Add(tarefa); break; // Fallback
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao carregar: {ex.Message}");
            }
        }

        public async Task MoverTarefaAsync(TarefaModel tarefa, string novoEstado)
        {
            if (tarefa == null || tarefa.EstadoAtual == novoEstado) return;

            // ====================================================================
            // VALIDAÇÕES DE REGRAS DE NEGÓCIO (PONTOS 14, 15, 16)
            // ====================================================================

            // REGRA 14: As Tarefas no estado "Done" não podem ser alteradas para outro estado 
            if (tarefa.EstadoAtual == "Done")
            {
                await App.Current.MainPage.DisplayAlert("Ação Bloqueada",
                    "Tarefas concluídas (Done) não podem ser movidas.", "OK");
                return;
            }

            // REGRA 15: Cada Programador apenas pode ter duas tarefas no estado "Doing" em simultâneo 
            if (novoEstado == "Doing")
            {
                if (DoingList.Count >= 2)
                {
                    await App.Current.MainPage.DisplayAlert("Limite Atingido",
                        "Você só pode ter 2 tarefas em 'Doing' ao mesmo tempo.", "OK");
                    return;
                }
            }

            // REGRA 16: Obrigatório executar pela ordem definida (OrdemExecucao) 
            // Validar se existem tarefas com Ordem menor que ainda não foram processadas
            if (!ValidarOrdemExecucao(tarefa, novoEstado))
            {
                await App.Current.MainPage.DisplayAlert("Ordem Incorreta",
                   $"Você deve seguir a ordem definida pelo Gestor.\nTarefa atual: {tarefa.OrdemExecucao}", "OK");
                return;
            }

            // ====================================================================
            // EXECUÇÃO DA MOVIMENTAÇÃO (Se passou nas regras acima)
            // ====================================================================

            string estadoAntigo = tarefa.EstadoAtual;

            try
            {
                // 1. ATUALIZAÇÃO VISUAL (Muda na tela primeiro)
                // Remove da lista onde ela está agora
                RemoverDaListaVisual(tarefa.Id);

                // Atualiza o estado
                tarefa.EstadoAtual = novoEstado;

                // Adiciona na nova lista
                AdicionarNaListaVisual(tarefa, novoEstado);

                // =================================================================================
                // 2. CHAMADA DE API (COMENTE ISSO PARA TESTAR SE O ARRASTAR FUNCIONA)
                // Se a API falhar, ele devolve a tarefa. Comente as linhas abaixo para testar apenas o visual.
                // =================================================================================

                bool sucesso = await _tarefaService.AtualizarEstadoAsync(tarefa.Id, novoEstado);

                if (sucesso)
                {
                    await App.Current.MainPage.DisplayAlert("Sucesso", "Movido com Sucesso!", "Ok");

                    if (tarefa.EstadoAtual == "Doing")
                        tarefa.DataRealInicio = DateTime.Now;
                    if (tarefa.EstadoAtual == "Done")
                        tarefa.DataRealFim = DateTime.Now;

                    var request = await _tarefaService.AtualizarTarefaAsync(tarefa.Id, tarefa);

                }
                else
                {
                    // Se deu erro na API, desfaz tudo (Rollback)
                    RemoverDaListaVisual(tarefa.Id); // Tira da nova
                    tarefa.EstadoAtual = estadoAntigo; // Volta estado
                    AdicionarNaListaVisual(tarefa, estadoAntigo); // Põe na velha

                    await App.Current.MainPage.DisplayAlert("Erro", "Falha ao salvar no servidor", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao mover: {ex.Message}");
            }
        }

        private void RemoverDaListaVisual(int idTarefa)
        {
            // Busca o item na lista pelo ID, em vez de comparar o objeto inteiro
            var itemToDo = ToDoList.FirstOrDefault(t => t.Id == idTarefa);
            if (itemToDo != null) ToDoList.Remove(itemToDo);

            var itemDoing = DoingList.FirstOrDefault(t => t.Id == idTarefa);
            if (itemDoing != null) DoingList.Remove(itemDoing);

            var itemDone = DoneList.FirstOrDefault(t => t.Id == idTarefa);
            if (itemDone != null) DoneList.Remove(itemDone);
        }

        private void AdicionarNaListaVisual(TarefaModel tarefa, string estado)
        {
            switch (estado)
            {
                case "To Do":
                    if (!ToDoList.Any(t => t.Id == tarefa.Id)) ToDoList.Add(tarefa);
                    break;
                case "Doing":
                    if (!DoingList.Any(t => t.Id == tarefa.Id)) DoingList.Add(tarefa);
                    break;
                case "Done":
                    if (!DoneList.Any(t => t.Id == tarefa.Id)) DoneList.Add(tarefa);
                    break;
            }
        }

        // Método auxiliar para Regra 16
        private bool ValidarOrdemExecucao(TarefaModel tarefaAlvo, string novoEstado)
        {
            // Converter string de ordem para int para comparação correta
            if (!int.TryParse(tarefaAlvo.OrdemExecucao, out int ordemAlvo)) return true; // Se não tiver ordem, deixa passar

            // CASO 1: Tentando mover de "To Do" -> "Doing"
            if (tarefaAlvo.EstadoAtual == "To Do" && novoEstado == "Doing")
            {
                // Verifica se existe alguma tarefa no "To Do" com ordem MENOR que a atual
                bool existeAnterior = ToDoList.Any(t =>
                    int.TryParse(t.OrdemExecucao, out int o) && o < ordemAlvo);

                if (existeAnterior) return false; // Bloqueia
            }

            // CASO 2: Tentando mover de "Doing" -> "Done"
            if (tarefaAlvo.EstadoAtual == "Doing" && novoEstado == "Done")
            {
                // Verifica se existe alguma tarefa no "Doing" com ordem MENOR que a atual
                // (Obriga a terminar as tarefas na ordem que foram iniciadas/definidas)
                bool existeAnterior = DoingList.Any(t =>
                    int.TryParse(t.OrdemExecucao, out int o) && o < ordemAlvo);

                if (existeAnterior) return false; // Bloqueia
            }

            return true;
        }

        // Método auxiliar de Reversão
        private void ReverterMovimento(TarefaModel tarefa, string estadoAntigo)
        {
            RemoverDaListaVisual(tarefa.Id);
            tarefa.EstadoAtual = estadoAntigo;
            AdicionarNaListaVisual(tarefa, estadoAntigo);
        }

        // REGRA 22: Consultar detalhes (ReadOnly) 
        [RelayCommand]
        public async Task VerDetalhes(TarefaModel tarefa)
        {
            if (tarefa == null) return;

            // Navega para uma página de detalhes passando o objeto tarefa
            // Precisaremos criar a rota no AppShell ou usar Navigation
            var navigationParameter = new Dictionary<string, object>
            {
                { "TarefaSelecionada", tarefa }
            };

            // Certifique-se de registrar a rota "DetalhesPage" no AppShell.xaml.cs
            await Shell.Current.GoToAsync(nameof(TarefaDetailPage), navigationParameter);
        }
        #endregion

    }
}
