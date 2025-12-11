using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Services.ProgramadorService;
using iTask_App_Mobile.Services.TarefaService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.ViewModels
{
    public partial class DashboardProgramadorPageViewModel : ObservableObject
    {
        private readonly IProgramadorService _tarefaService;

        [ObservableProperty]
        private DashboardDTO _dashboardData;

        [ObservableProperty]
        private string _nomeProgramador;

        [ObservableProperty]
        private bool isBusy;


        public DashboardProgramadorPageViewModel(IProgramadorService tarefaService)
        {
            _tarefaService = tarefaService;
            _dashboardData = new DashboardDTO();
            _nomeProgramador = Preferences.Get("user_name", "Programador");
        }

        [RelayCommand]
        public async Task CarregarDashboard()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Pega o ID do Utilizador salvo no Login
                int idUser = Preferences.Get("user_id", 0);
                if (idUser == 0) return;

                var dados = await _tarefaService.GetDashboardProgramadorAsync(idUser);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DashboardData = dados;
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Comandos de Navegação
        [RelayCommand]
        public async Task IrParaKanban() => await Shell.Current.GoToAsync("/KanbanPage");

        [RelayCommand]
        public async Task IrParaHistorico() => await Shell.Current.GoToAsync("/TarefaConcluidaPage");
    }
}
