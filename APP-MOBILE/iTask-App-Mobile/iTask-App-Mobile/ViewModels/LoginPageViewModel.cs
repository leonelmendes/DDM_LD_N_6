using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Services.AuthenticateService;
using iTask_App_Mobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iTask_App_Mobile.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        #region Services
        private readonly IAuthenticateService _service;
        #endregion

        #region Construvtor
        public LoginPageViewModel(IAuthenticateService service)
        {
            _service = service;

            LoginCommand = new Command(async () => await LoginAsync());
            PageCadastrarCommand = new Command(NavigateToRegisterPage);
        }
        #endregion

        #region Properties
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;
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

        public ICommand LoginCommand { get; }
        public ICommand PageCadastrarCommand { get; }

        #endregion

        #region Métodos
        private async Task LoginAsync()
        {
            var model = new LoginRequestModel()
            {
                Username = Username,
                Password = Password
            };

            var resultado = await _service.LoginAsync(model);

            if (resultado != null)
            {
                // SALVAR OS DADOS DO LOGIN - Sem muita segurança por enquanto
                Preferences.Set("user_id", resultado.Id);
                Preferences.Set("user_nome", resultado.Nome);
                Preferences.Set("user_username", resultado.Username);
                Preferences.Set("user_email", resultado.Email);
                Preferences.Set("user_tipo", resultado.TipoUtilizador);
                // Caso qeu quiser usar token futuramente o recomendo salvar em SecureStorage

                var shell = (AppShell)Shell.Current;
                if (resultado.TipoUtilizador == "Gestor")
                {
                    //shell.MostrarTabbarGestor();
                    await Shell.Current.GoToAsync(state: $"//DashboardGestorPage");

                    var snackbar = Snackbar.Make($"Login realizado com sucesso!\nBem Vindo Gestor {resultado.Nome} ", null, "ok", TimeSpan.FromSeconds(3), snackbaroptionTrue);
                    await snackbar.Show();
                }
                else
                {
                    //shell.MostrarTabbarProgramador();
                    //await Shell.Current.GoToAsync(state: $"//DashboardProgramadorPage");
                    Application.Current.MainPage = new ProgrammerShell();

                    var snackbar = Snackbar.Make($"Login realizado com sucesso!\nBem Vindo Programador {resultado.Nome} ", null, "ok", TimeSpan.FromSeconds(3), snackbaroptionTrue);
                    await snackbar.Show();
                }
                //await Shell.Current.DisplayAlert("Sucesso", $"{resultado.TipoUtilizador} {resultado.Nome}, Logado com sucesso!", "OK");


                //await Shell.Current.GoToAsync("..");

                LimparCampos();
            }
            else
            {
                await Shell.Current.DisplayAlert("Erro", "Não foi possível fazer o login.", "OK");
                LimparCampos();
                //var snackbar = Snackbar.Make("Erro ao efetuar login. Verifique as credenciais e tente novamente.", null, "ok", TimeSpan.FromSeconds(3), snackbaroptionErro);
                //await snackbar.Show();
            }
        }
        private void LimparCampos()
        {
            Username = string.Empty;
            Password = string.Empty;
        }

        private async void NavigateToRegisterPage()
        {
            await Shell.Current.GoToAsync(state: "RegisterPage");
        }
        #endregion
    }
}
