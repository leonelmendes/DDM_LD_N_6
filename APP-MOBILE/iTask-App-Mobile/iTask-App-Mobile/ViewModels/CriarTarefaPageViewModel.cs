using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Models;
using iTask_App_Mobile.Services.ProgramadorService;
using iTask_App_Mobile.Services.TarefaService;
using iTask_App_Mobile.Services.TipoTarefaService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.ViewModels
{
    public partial class CriarTarefaPageViewModel : ObservableObject
    {
        #region Services
        private readonly IProgramadorService _programadorService;
        private readonly ITarefaService _tarefaService;
        private readonly ITipoTarefaService _tipotarefaService;
        #endregion

        #region Constructor
        public CriarTarefaPageViewModel(IProgramadorService programadorService, ITarefaService tarefaService, ITipoTarefaService tipoTarefaService)
        {
            _programadorService = programadorService;
            _tarefaService = tarefaService;
            _tipotarefaService = tipoTarefaService;

            Programadores = new ObservableCollection<ProgramadorListDTO>();
            TiposTarefa = new ObservableCollection<TipoTarefaModel>();

            // Carrega os programadores e tipos tarefa automaticamente
            //Task.Run(async () => await CarregarProgramadoresAsync()); 
            //Task.Run(async () => await CarregarTiposTarefaAsync());
        }
        #endregion

        #region Properties
        [ObservableProperty]
        private int idGestor;

        [ObservableProperty]
        private string titulo;

        [ObservableProperty]
        private ProgramadorListDTO programadorSelecionado;

        [ObservableProperty]
        private TipoTarefaModel tipoTarefaSelecionado;

        [ObservableProperty]
        private string ordemExecucao;

        [ObservableProperty]
        private string descricao;

        [ObservableProperty]
        private DateTime dataPrevistaInicio = DateTime.Now;

        [ObservableProperty]
        private DateTime dataPrevistaFim = DateTime.Now.AddDays(1);

        [ObservableProperty]
        private int storyPoints;

        public ObservableCollection<ProgramadorListDTO> Programadores { get; }
        public ObservableCollection<TipoTarefaModel> TiposTarefa { get; }
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
        public async Task CarregarProgramadoresAsync()
        {
            Programadores.Clear();

            var lista = await _programadorService.GetProgramadoresByGestorAsync(Preferences.Get("gestor_id", 0));

            foreach (var item in lista)
                Programadores.Add(item);
        }

        public async Task CarregarTiposTarefaAsync()
        {
            TiposTarefa.Clear();

            var lista = await _tipotarefaService.GetAllAsync();

            foreach (var item in lista)
                TiposTarefa.Add(item);
        }

        [RelayCommand]
        public async Task CriarTarefaAsync()
        {
            if (!ValidarCampos())
            {
                //await Snackbar.Make("Preencha todos os campos corretamente.", duration: TimeSpan.FromSeconds(3)).Show();
                await Shell.Current.DisplayAlert("Alerta", $"Preencha todos os campos corretamente.", "OK");
                return;
            }

            var model = new TarefaModel
            {
                IdGestor = Preferences.Get("user_id", 0),
                IdProgramador = ProgramadorSelecionado.Id,
                IdTipoTarefa = TipoTarefaSelecionado.Id,
                Titulo = Titulo,
                OrdemExecucao = OrdemExecucao,
                Descricao = Descricao,
                DataPrevistaInicio = DataPrevistaInicio,
                DataPrevistaFim = DataPrevistaFim,
                StoryPoints = StoryPoints,
                // Valores opcionais
                EstadoAtual = "To Do",
                DataRealInicio = null,
                DataRealFim = null
            };

            bool sucesso = await _tarefaService.CriarTarefaAsync(model);

            if (sucesso)
            {
                //await Snackbar.Make("Tarefa criada com sucesso!", duration: TimeSpan.FromSeconds(3)).Show();
                await Shell.Current.DisplayAlert("Sucesso", $"Tarefa criada com sucesso!", "OK");

                LimparCampos();

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                //await Snackbar.Make("Erro ao criar a tarefa.", duration: TimeSpan.FromSeconds(3)).Show();
                await Shell.Current.DisplayAlert("Erro", $"Erro ao criar a tarefa.", "OK");
            }
        }

        #endregion

        #region Métodos

        private bool ValidarCampos()
        {
            if (ProgramadorSelecionado is null) return false;
            if (TipoTarefaSelecionado is null) return false;
            if (string.IsNullOrWhiteSpace(Descricao)) return false;
            if (string.IsNullOrWhiteSpace(OrdemExecucao)) return false;
            if (StoryPoints <= 0) return false;

            if (DataPrevistaFim <= DataPrevistaInicio) return false;

            return true;
        }

        private void LimparCampos()
        {
            ProgramadorSelecionado = null;
            TipoTarefaSelecionado = null;
            OrdemExecucao = string.Empty;
            Descricao = string.Empty;
            StoryPoints = 0;
            DataPrevistaInicio = DateTime.Now;
            DataPrevistaFim = DateTime.Now.AddDays(4);
        }

        [RelayCommand]
        private async Task VoltarAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        #endregion

    }
}
