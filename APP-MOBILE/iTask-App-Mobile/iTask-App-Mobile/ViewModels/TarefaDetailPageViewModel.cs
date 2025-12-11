using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.Models;
using iTask_App_Mobile.Services.TarefaService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.ViewModels
{
    public partial class TarefaDetailPageViewModel : ObservableObject, IQueryAttributable
    {
        #region Services
        private readonly ITarefaService _tarefaService;
        #endregion

        #region Properties
        [ObservableProperty]
        private TarefaModel _tarefa;

        [ObservableProperty]
        private string _tituloPagina;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SalvarTarefaCommand))]
        private bool _isBusy;
        #endregion

        #region Constructor
        public TarefaDetailPageViewModel(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }
        #endregion

        #region Commands

        [RelayCommand(CanExecute = nameof(CanSalvar))]
        public async Task SalvarTarefa()
        {
            if (!CanSalvar()) return;

            IsBusy = true;
            bool sucesso = false;

            try
            {
                if (Tarefa.Id == 0)
                {
                    // CRIAÇÃO
                    sucesso = await _tarefaService.CriarTarefaAsync(Tarefa);
                }
                else
                {
                    // EDIÇÃO
                    sucesso = await _tarefaService.AtualizarTarefaAsync(Tarefa.Id, Tarefa);
                }

                if (sucesso)
                {
                    await Shell.Current.GoToAsync(".."); // Volta para a página anterior (GestaoTarefasPage)
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Falha ao salvar a tarefa. Verifique os dados.", "OK");
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine($"Erro ao salvar: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Erro", "Ocorreu um erro inesperado.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanSalvar()
        {
            // Validação mínima: Título não pode ser vazio e a OrdemExecucao deve ser um número
            return !string.IsNullOrWhiteSpace(Tarefa?.Titulo) &&
                   int.TryParse(Tarefa?.OrdemExecucao, out _);
        }

        [RelayCommand]
        private async Task BackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
        #endregion

        #region IQueryAttributable (Receber dados de navegação)
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // Recebe a tarefa passada na navegação
            if (query.ContainsKey("TarefaSelecionada"))
            {
                Tarefa = query["TarefaSelecionada"] as TarefaModel;

                // Define o título da página baseado se é criação ou edição
                //TituloPagina = (Tarefa.Id == 0) ? "Criar Nova Tarefa" : $"Editar: {Tarefa.Titulo}";
            }
        }
        #endregion
    }
}
