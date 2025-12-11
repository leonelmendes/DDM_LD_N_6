using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Models; // Para TipoTarefaModel
using iTask_App_Mobile.Services.TarefaService;
using iTask_App_Mobile.Services.TipoTarefaService; // Importe o serviço de tipos
using System.Collections.ObjectModel;

namespace iTask_App_Mobile.ViewModels
{
    public partial class EditarTarefaPageViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ITarefaService _tarefaService;
        private readonly ITipoTarefaService _tipoTarefaService; // Serviço de Tipos

        [ObservableProperty]
        private TarefaDetalhesDTO _tarefa;

        [ObservableProperty]
        private string _tituloPagina;

        // Lista para o Picker
        public ObservableCollection<TipoTarefaModel> TiposTarefa { get; } = new();

        // Item selecionado no Picker
        [ObservableProperty]
        private TipoTarefaModel _tipoTarefaSelecionado;

        public EditarTarefaPageViewModel(ITarefaService tarefaService, ITipoTarefaService tipoTarefaService)
        {
            _tarefaService = tarefaService;
            _tipoTarefaService = tipoTarefaService;
            _tarefa = new TarefaDetalhesDTO();

            // Carrega os tipos assim que abre a tela
            //Task.Run(CarregarTiposTarefa);
        }

        public async Task CarregarTiposTarefa()
        {
            try
            {
                var tipos = await _tipoTarefaService.GetAllAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TiposTarefa.Clear();
                    foreach (var tipo in tipos)
                    {
                        TiposTarefa.Add(tipo);
                    }

                    // LÓGICA CRUCIAL: Re-selecionar o tipo no Picker após recarregar a lista
                    if (Tarefa != null && Tarefa.IdTipoTarefa > 0)
                    {
                        TipoTarefaSelecionado = TiposTarefa.FirstOrDefault(t => t.Id == Tarefa.IdTipoTarefa);
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar tipos: {ex.Message}");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("TarefaSelecionada"))
            {
                Tarefa = query["TarefaSelecionada"] as TarefaDetalhesDTO;
                TituloPagina = (Tarefa.Id == 0) ? "Nova Tarefa" : "Editar Tarefa";

                // Tenta selecionar o tipo no Picker se a lista já estiver carregada
                if (TiposTarefa.Any() && Tarefa.IdTipoTarefa > 0)
                {
                    TipoTarefaSelecionado = TiposTarefa.FirstOrDefault(t => t.Id == Tarefa.IdTipoTarefa);
                }
            }
        }

        [RelayCommand]
        public async Task Voltar()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task SalvarTarefa()
        {
            // Atualiza o ID do tipo baseado no que foi selecionado no Picker
            if (TipoTarefaSelecionado != null)
            {
                Tarefa.IdTipoTarefa = TipoTarefaSelecionado.Id;
            }

            // Converter DTO para Model para salvar
            var model = new TarefaModel
            {
                Id = Tarefa.Id,
                Titulo = Tarefa.Titulo,
                Descricao = Tarefa.Descricao,
                OrdemExecucao = Tarefa.OrdemExecucao,
                StoryPoints = Tarefa.StoryPoints,
                DataPrevistaInicio = Tarefa.DataPrevistaInicio,
                DataPrevistaFim = Tarefa.DataPrevistaFim,
                IdTipoTarefa = Tarefa.IdTipoTarefa,
                IdProgramador = Tarefa.IdProgramador,
                IdGestor = Tarefa.IdGestor,
                EstadoAtual = Tarefa.EstadoAtual ?? "To Do"
            };

            bool sucesso;
            if (Tarefa.Id == 0)
                sucesso = await _tarefaService.CriarTarefaAsync(model);
            else
                sucesso = await _tarefaService.AtualizarTarefaAsync(Tarefa.Id, model);

            if (sucesso)
            {
                await Shell.Current.DisplayAlert("Sucesso", "Tarefa alterada com Sucesso!", "Ok");
                await Shell.Current.GoToAsync("..");
            }
            else
                await App.Current.MainPage.DisplayAlert("Erro", "Falha ao salvar", "OK");
        }
    }
}