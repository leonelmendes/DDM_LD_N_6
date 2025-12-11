using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Services.ProgramadorService;

namespace iTask_App_Mobile.ViewModels
{
    public partial class AddProgramadorPageViewModel : ObservableObject
    {
        #region Services
        private readonly IProgramadorService _service;
        #endregion

        #region Constructor
        public AddProgramadorPageViewModel(IProgramadorService service)
        {
            _service = service;
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
        private string nivelExperiencia;
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
        [RelayCommand]
        private async Task CreateProgramador()
        {
            if(string.IsNullOrEmpty(Nome) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(NivelExperiencia))
            {
                await Shell.Current.DisplayAlert("Atenção", "Todos os Campos são de preenchimento Aleatorio.", "Ok");
            }
            else
            {
                var model = new CreateProgramadorDTO
                {
                    Nome = Nome,
                    Email = Email,
                    Username = Username,
                    Password = Password,
                    NivelExperiencia = NivelExperiencia,
                    IdGestor = Preferences.Get("gestor_id", 0)
                };

                var response = await _service.CreateProgramadorAsync(model);

                if (response)
                {
                    await Shell.Current.DisplayAlert("Sucesso","Programador Cadastrado com Sucesso!","Ok");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erro", "Erro ao Cadastrar Programador!", "Ok");
                }
            }
        }

        [RelayCommand]
        private async Task Voltar()
        {
            await Shell.Current.GoToAsync("..");
        }
        #endregion
    }
}
