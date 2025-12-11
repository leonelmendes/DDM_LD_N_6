using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Services.TarefaService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.ViewModels
{
    public partial class TarefasConcluidasPageViewModel : ObservableObject
    {
        private readonly ITarefaService _tarefaService;

        [ObservableProperty]
        private ObservableCollection<TarefaDetalhesDTO> _tarefasConcluidas = new();

        [ObservableProperty]
        private bool _isLoading;

        public TarefasConcluidasPageViewModel(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
            Task.Run(CarregarTarefasAsync);
        }

        [RelayCommand]
        public async Task CarregarTarefasAsync()
        {
            //if (IsLoading) return;

            try
            {
                IsLoading = true;

                // 1. Busca todas as tarefas detalhadas
                var todasTarefas = await _tarefaService.GetTarefasDetalhesAsync();

                // 2. Filtra: ID do Programador Logado (Ex: 1) E Estado "Done"
                int idProgramadorLogado = Preferences.Get("programador_id", 0); // Substituir pela lógica de sessão real

                var filtradas = todasTarefas
                    .Where(t => t.IdProgramador == idProgramadorLogado && t.EstadoAtual == "Done")
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
                //Debug.WriteLine($"Erro: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Erro", "Não foi possível carregar as tarefas concluídas.", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
