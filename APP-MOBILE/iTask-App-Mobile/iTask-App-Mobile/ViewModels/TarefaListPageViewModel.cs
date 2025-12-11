using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Models;
using iTask_App_Mobile.Services.TarefaService;
using iTask_App_Mobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.ViewModels
{
    public partial class TarefaListPageViewModel : ObservableObject
    {
        #region Services
        private readonly ITarefaService _tarefaService;
        #endregion

        #region Properties
        [ObservableProperty]
        private ObservableCollection<TarefaDetalhesDTO> _tarefasDoGestor = new();

        [ObservableProperty]
        private bool _isLoading;
        #endregion

        #region Constructor
        public TarefaListPageViewModel(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
            Task.Run(CarregarTarefasAsync);
        }
        #endregion

        #region Commands

        [RelayCommand]
        public async Task CarregarTarefasAsync()
        {
            //if (IsLoading) return;

            try
            {
                //IsLoading = true;
                // ATENÇÃO: Substitua 1 pelo ID do Gestor logado
                //int idGestorLogado = Preferences.Get("gestor_id", 0);

                var tarefas = await _tarefaService.GetTarefasDetalhesAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TarefasDoGestor.Clear();
                    foreach (var tarefa in tarefas)
                    {
                        TarefasDoGestor.Add(tarefa);
                    }
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar tarefas do gestor: {ex.Message}", "Ok");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task NavegarParaCriacao()
        {
            // Cria um novo modelo vazio para a tela de edição
            var novaTarefa = new TarefaModel { IdGestor = 1, EstadoAtual = "To Do" };
            var param = new Dictionary<string, object> { { "TarefaSelecionada", novaTarefa } };
            // Navega para a página de Detalhes, que será usada para Criação/Edição
            //await Shell.Current.GoToAsync(nameof(DetalhesPage), param);
            await Shell.Current.GoToAsync($"TarefaDetailPage?TarefaSelecionada={param}");
        }

        [RelayCommand]
        public async Task NavegarParaEdicao(TarefaDetalhesDTO tarefa)
        {
            if (tarefa == null) return;

            var param = new Dictionary<string, object>
            {
                { "TarefaSelecionada", tarefa }
            };

            // CORREÇÃO: Passe 'param' como segundo argumento, não dentro da string
            await Shell.Current.GoToAsync(nameof(EditarTarefaPage), param);
        }

        [RelayCommand]
        public async Task DeletarTarefa(TarefaDetalhesDTO tarefa)
        {
            if (tarefa == null) return;

            bool confirm = await App.Current.MainPage.DisplayAlert("Confirmar Exclusão",
                $"Tem certeza que deseja deletar a tarefa '{tarefa.Titulo}'?", "Sim", "Não");

            if (confirm)
            {
                bool sucesso = await _tarefaService.DeleteTarefaAsync(tarefa.Id);
                if (sucesso)
                {
                    TarefasDoGestor.Remove(tarefa);
                    await App.Current.MainPage.DisplayAlert("Sucesso", "Tarefa excluída com sucesso.", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Falha ao excluir a tarefa.", "OK");
                }
            }
        }
        #endregion
    }
}

