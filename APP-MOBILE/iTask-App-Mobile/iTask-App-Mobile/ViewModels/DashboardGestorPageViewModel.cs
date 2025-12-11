using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Services.GestorService;
using iTask_App_Mobile.Services.ProgramadorService;
using iTask_App_Mobile.Services.TarefaService;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace iTask_App_Mobile.ViewModels
{
    public partial class DashboardGestorPageViewModel : ObservableObject
    {
        #region Services
        private readonly IGestorService _service;
        private readonly IProgramadorService _programadorService;
        private readonly ITarefaService _tarefaService;
        #endregion

        #region Constructor
        public DashboardGestorPageViewModel(IGestorService service, IProgramadorService programadorService, ITarefaService tarefaService)
        {
            _programadorService = programadorService;
            _tarefaService = tarefaService;
            _service = service;

            PageCriarTarefaCommand = new Command(NavigateToCriarTarefaPage);
            PageAddProgramadorCommand = new Command(NavigateToAddProgramadorPage);
            PagePerfilCommand = new Command(NavigateToPerfilaPage);
            PageTarefaConcluidaGestorCommand = new Command(NavigateToTarefaConcluidaGestor);

            Programadores = new ObservableCollection<ProgramadorCardEquipeDTO>();

            CarregarDados();
        }

        #endregion

        #region Properties
        [ObservableProperty]
        private string _previsaoEntregaTexto = "Calculando...";
        [ObservableProperty]
        private DashboardDTO _dashboardData;
        [ObservableProperty]
        private bool _temPrevisao = false;
        [ObservableProperty]
        private string nome ;
        [ObservableProperty]
        private bool isBusy;
        [ObservableProperty]
        private ObservableCollection<ProgramadorCardEquipeDTO> programadores;

        [ObservableProperty]
        private bool isLoading;
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
        public ICommand PageTarefaConcluidaGestorCommand { get; }


        [RelayCommand]
        public async Task LoadEquipeDoGestorAsync()
        {
            try
            {
                IsLoading = true;
                // CHAMA O NOVO MÉTODO FILTRADO:
                var lista = await _programadorService.GetByGestorAsync(Preferences.Get("gestor_id", 0));

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Programadores.Clear();
                    if (lista != null)
                    {
                        foreach (var item in lista)
                        {
                            Programadores.Add(item);
                        }
                    }
                });
                IsLoading = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}");
            }
            finally
            {
                MainThread.BeginInvokeOnMainThread(() => { IsLoading = false; });
                await CarregarDashboard();
                await CarregarDashboard2();
            }
            IsLoading = false;
        }

        public async Task CarregarDashboard()
        {
            try
            {
                // Chama a API (lógica que explicamos na resposta anterior)
                double horasTotais = await _tarefaService.GetPrevisaoEntregaAsync();

                if (horasTotais > 0)
                {
                    TimeSpan ts = TimeSpan.FromHours(horasTotais);

                    // Formatação bonita
                    if (ts.Days > 0)
                        PrevisaoEntregaTexto = $"{ts.Days} dias e {ts.Hours}h";
                    else
                        PrevisaoEntregaTexto = $"{ts.Hours} horas";

                    TemPrevisao = true;
                }
                else
                {
                    PrevisaoEntregaTexto = "Tudo em dia!";
                    TemPrevisao = false;
                }
            }
            catch
            {
                PrevisaoEntregaTexto = "--";
            }
        }

        [RelayCommand]
        public async Task CarregarDashboard2()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Chama o endpoint GLOBAL
                var dados = await _tarefaService.GetDashboardGlobalAsync();

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

        #endregion

        #region Métodos
        private async void NavigateToTarefaConcluidaGestor()
        {
            await Shell.Current.GoToAsync(state: "/TarefasConcluidasGestorPage");
        }
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

