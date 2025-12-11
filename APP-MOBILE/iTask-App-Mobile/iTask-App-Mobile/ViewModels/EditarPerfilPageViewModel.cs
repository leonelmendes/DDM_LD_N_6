using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Services.UtilizadorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace iTask_App_Mobile.ViewModels
{
    public partial class EditarPerfilPageViewModel : ObservableObject
    {

        private readonly IUtilizadorService _userService;

        // Propriedades Comuns
        [ObservableProperty] private string _nome;
        [ObservableProperty] private string _username;
        [ObservableProperty] private string _password; // Opcional

        // Propriedades Específicas
        [ObservableProperty] private string _departamento; // Só Gestor
        [ObservableProperty] private string _nivelExperiencia; // Só Programador

        // Controle de Visibilidade
        [ObservableProperty] private bool _isGestor;
        [ObservableProperty] private bool _isProgramador;
        [ObservableProperty] private bool _isBusy;

        private int _userId; // ID do Gestor ou Programador

        public EditarPerfilPageViewModel(IUtilizadorService userService)
        {
            _userService = userService;
            CarregarDadosUsuario();
        }

        private void CarregarDadosUsuario()
        {
            // AQUI VOCÊ DEVE PEGAR OS DADOS DA SESSÃO/PREFERENCES
            // Exemplo Simulado:
            string tipoUsuario = Preferences.Get("user_tipo", "Gestor"); // "Gestor" ou "Programador"
            _userId = Preferences.Get("user_id", 1); // ID do Gestor ou Programador
            Nome = Preferences.Get("user_nome", "Utilizador Teste");
            Username = Preferences.Get("user_username", "user.teste");

            if (tipoUsuario == "Gestor")
            {
                IsGestor = true;
                IsProgramador = false;
                Departamento = "TI"; // Buscar do serviço se possível
            }
            else
            {
                IsGestor = false;
                IsProgramador = true;
                NivelExperiencia = "Júnior"; // Buscar do serviço se possível
            }
        }

        [RelayCommand]
        public async Task SalvarPerfil()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                bool sucesso = false;

                if (IsGestor)
                {
                    var dto = new UpdateGestorProfileDTO
                    {
                        Id = _userId,
                        Nome = Nome,
                        Username = Username,
                        Departamento = Departamento,
                        Password = string.IsNullOrWhiteSpace(Password) ? null : Password
                    };

                    // CHAMADA AO SERVIÇO (Limpo e organizado)
                    sucesso = await _userService.AtualizarPerfilGestorAsync(dto);
                }
                else
                {
                    var dto = new UpdateProgramadorProfileDTO
                    {
                        Id = _userId,
                        Nome = Nome,
                        Username = Username,
                        NivelExperiencia = NivelExperiencia,
                        Password = string.IsNullOrWhiteSpace(Password) ? null : Password
                    };

                    // CHAMADA AO SERVIÇO
                    sucesso = await _userService.AtualizarPerfilProgramadorAsync(dto);
                }

                if (sucesso)
                {
                    // Atualiza dados locais para refletir na UI imediatamente
                    Preferences.Set("user_nome", Nome);
                    Preferences.Set("user_username", Username);

                    await App.Current.MainPage.DisplayAlert("Sucesso", "Perfil atualizado!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Não foi possível atualizar o perfil.", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", $"Falha inesperada: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async void VoltarAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
