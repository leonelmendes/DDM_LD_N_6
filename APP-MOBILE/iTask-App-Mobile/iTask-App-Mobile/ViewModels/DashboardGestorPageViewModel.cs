using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using iTask_App_Mobile.Services.GestorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iTask_App_Mobile.ViewModels
{
    public partial class DashboardGestorPageViewModel : ObservableObject
    {
        #region Services
        private readonly IGestorService _service;
        #endregion

        #region Constructor
        public DashboardGestorPageViewModel(IGestorService service)
        {
            _service = service;

            PageCriarTarefaCommand = new Command(NavigateToCriarTarefaPage);
            PageAddProgramadorCommand = new Command(NavigateToAddProgramadorPage);
            PagePerfilCommand = new Command(NavigateToPerfilaPage);

            CarregarDados();
        }

        #endregion

        #region Properties
        [ObservableProperty]
        private string nome ;
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

        public ICommand PageCriarTarefaCommand { get; }
        public ICommand PageAddProgramadorCommand { get; }
        public ICommand PagePerfilCommand { get; }

        #endregion

        #region Métodos
        private async void NavigateToAddProgramadorPage()
        {
            await Shell.Current.GoToAsync(state: "/AddProgramadorPage");
        }
        private async void NavigateToCriarTarefaPage()
        {
            await Shell.Current.GoToAsync(state: "/CriarTarefaPage");
        }
        private async void NavigateToPerfilaPage()
        {
            await Shell.Current.GoToAsync("//PerfilPage");
        }
        private void CarregarDados()
        {
            Nome = Preferences.Get("user_nome", "Sem nome");
        }
        #endregion
    }
}

