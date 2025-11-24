using CommunityToolkit.Mvvm.ComponentModel;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Services.AuthenticateService;
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

            LoginCommand = new Command(async () => await RegistrarGestorAsync());
            PageCadastrarCommand = new Command(NavigateToRegisterPage);
        }
        #endregion

        #region Properties
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;
        #endregion

        #region Command

        public ICommand LoginCommand { get; }
        public ICommand PageCadastrarCommand { get; }

        #endregion

        #region Métodos
        private async Task RegistrarGestorAsync()
        {
            var model = new LoginRequestModel()
            {
                Username = Username,
                Password = Password
            };

            var resultado = await _service.LoginAsync(model);

            if (!string.IsNullOrEmpty(resultado.Email))
            {
                await Shell.Current.DisplayAlert("Sucesso", $"{resultado.TipoUtilizador} {resultado.Nome}, Logado com sucesso!", "OK");
                LimparCampos();
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Erro", "Não foi possível fazer o login.", "OK");
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
