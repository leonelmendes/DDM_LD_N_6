using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.ViewModels
{
    public partial class PerfilPageViewModel : ObservableObject
    {
        #region Services
        #endregion

        #region Properties
        [ObservableProperty]
        private string nome;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string tipoUtilizador;
        #endregion

        #region Constructor
        public PerfilPageViewModel()
        {
            CarregarPerfil();
        }
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

        #endregion

        #region Métodos
        private void CarregarPerfil()
        {
            Nome = Preferences.Get("user_nome", "Sem nome");
            Email = Preferences.Get("user_email", "Sem email");
            Username = Preferences.Get("user_username", "Sem username");
            TipoUtilizador = Preferences.Get("user_tipo", "Desconhecido");
        }
        [RelayCommand]
        private async Task LogoutAsync()
        {
            // 1. limpar preferences
            Preferences.Clear();

            // 2. voltar para o Shell de login
            Application.Current.MainPage = new AppShell();

            //await Shell.Current.GoToAsync("//LoginPage");
        }
        #endregion
    }
}
