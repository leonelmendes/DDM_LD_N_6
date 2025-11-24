using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Services.AuthenticateService;
using System.Windows.Input;

namespace iTask_App_Mobile.ViewModels
{
    public partial class RegisterPageViewModel : ObservableObject
    {
        #region Services
        private readonly IAuthenticateService _service;
        #endregion

        #region Construvtor
        public RegisterPageViewModel(IAuthenticateService service)
        {
            _service = service;

            RegistrarGestorCommand = new Command(async () => await RegistrarGestorAsync());
            BackCommand = new Command(Back);
        }
        #endregion

        #region Properties
        [ObservableProperty]
        private string nome;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string departamento;

        [ObservableProperty]
        private bool isAdmin = true; // Sempre true no gestor
        #endregion

        #region Command

        public ICommand RegistrarGestorCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand Clean { get; }

        #endregion

        #region Métodos
        private async Task RegistrarGestorAsync()
        {
            var model = new RegisterManagerDTO()
            {
                Nome = Nome,
                Email = Email,
                Username = Username,
                Password = Password,
                Departamento = Departamento,
                IsAdmin = true
            };

            var resultado = await _service.RegisterManagerAsync(model);

            if (resultado)
            {
                await Shell.Current.DisplayAlert("Sucesso", $"Gestor {model.Nome}, registado com sucesso!", "OK");
                LimparCampos();
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Erro", "Não foi possível registar o gestor.", "OK");
            }
        }
        private void LimparCampos()
        {
            Nome = string.Empty;
            Email = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            Departamento = string.Empty;
        }
        private void Back()
        {
            Shell.Current.GoToAsync("..");
        }
        #endregion
    }
}
