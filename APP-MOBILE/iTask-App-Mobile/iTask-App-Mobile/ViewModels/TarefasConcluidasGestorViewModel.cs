using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Services.TarefaService;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace iTask_App_Mobile.ViewModels
{
    public partial class TarefasConcluidasGestorViewModel : ObservableObject
    {
        private readonly ITarefaService _tarefaService;

        [ObservableProperty]
        private ObservableCollection<TarefaDetalhesDTO> _tarefasConcluidas = new();

        [ObservableProperty]
        private bool _isLoading;

        public TarefasConcluidasGestorViewModel(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
            Task.Run(CarregarTarefasAsync);
        }

        [RelayCommand]
        public async Task CarregarTarefasAsync()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;

                // 1. Busca todas as tarefas detalhadas da API
                var todasTarefas = await _tarefaService.GetTarefasDetalhesAsync();

                // 2. Filtra: Criadas pelo Gestor Logado (ID 1) E Status "Done"
                int idGestorLogado = Preferences.Get("gestor_id", 1);
                
                var filtradas = todasTarefas
                    .Where(t => t.IdGestor == idGestorLogado && t.EstadoAtual == "Done")
                    .ToList();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TarefasConcluidas.Clear();
                    foreach (var tarefa in filtradas)
                    {
                        TarefasConcluidas.Add(tarefa);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task Voltar()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}