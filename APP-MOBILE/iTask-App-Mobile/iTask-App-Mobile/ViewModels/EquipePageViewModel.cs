using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Services.ProgramadorService;
using iTask_App_Mobile.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace iTask_App_Mobile.ViewModels
{
    public partial class EquipePageViewModel : ObservableObject
    {
        #region Services
        private readonly IProgramadorService _programadorService;
        #endregion

        #region Constructor
        public EquipePageViewModel(IProgramadorService programadorService)
        {
            _programadorService = programadorService;
            Programadores = new ObservableCollection<ProgramadorCardEquipeDTO>();

            //LoadDataCommand.Execute(null);
        }
        #endregion

        #region Properties
        [ObservableProperty]
        private ObservableCollection<ProgramadorCardEquipeDTO> programadores;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isEmpty;
        #endregion

        [ObservableProperty]
        private ProgramadorCardEquipeDTO selectedProgramador;

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


        // Método disparado automaticamente quando SelectedProgramador muda
        // (Recurso do CommunityToolkit.Mvvm)
        async partial void OnSelectedProgramadorChanged(ProgramadorCardEquipeDTO value)
        {
            // Se for nulo (deseleção), não faz nada
            if (value == null) return;

            // Chama o comando de navegação
            await IrParaDetalhe(value);

            // Limpa a seleção visualmente para permitir clicar de novo depois
            selectedProgramador = null;
        }

        [RelayCommand]
        public async Task LoadDataAsync()
        {
            //if (IsLoading) return;

            try
            {
                IsLoading = true;

                var lista = await _programadorService.GetAllAsync();

                // ATENÇÃO: Atualizar a UI sempre na MainThread
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
                // IMPORTANTE: Desliga a animação na MainThread para garantir que o RefreshView pare
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = false;
                });
            }
        }

        [RelayCommand]
        private async Task GoToAddProgramadorAsync()
        {
            // Navega para a página de adicionar programador já criada
            await Shell.Current.GoToAsync(state: "/AddProgramadorPage");
        }

        [RelayCommand]
        private async Task IrParaDetalhe(ProgramadorCardEquipeDTO cardSelecionado)
        {
            if (cardSelecionado == null) return;

            // Navega passando apenas o parâmetro "Id"
            // Exemplo de URL gerada: DetalheProgramadorPage?Id=10
            //await Shell.Current.GoToAsync($"{nameof(DetalheProgramadorPage)}?Id={cardSelecionado.Id}");
            await Shell.Current.GoToAsync($"DetalheProgramadorPage?Id={cardSelecionado.Id}");
        }
        #endregion
    }
}
