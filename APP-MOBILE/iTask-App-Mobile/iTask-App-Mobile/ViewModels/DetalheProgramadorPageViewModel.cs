using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iTask_App_Mobile.DTOs;
using iTask_App_Mobile.Services.ProgramadorService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace iTask_App_Mobile.ViewModels
{
    
    public partial class DetalheProgramadorPageViewModel : ObservableObject, IQueryAttributable
    {
        #region Services
        private readonly IProgramadorService _service;
        #endregion

        #region Constructor
        public DetalheProgramadorPageViewModel(IProgramadorService service)
        {
            _service = service;
        }
        #endregion

        #region Properties
        [ObservableProperty]
        private ProgramadorDetalhe programadorInicial;
        [ObservableProperty]
        private DashboardDTO _dashboardData;

        // Propriedade para exibir o Gestor na tela (somente leitura)
        [ObservableProperty]
        private string gestorResponsavelNome;
        #endregion
        [ObservableProperty]
        private int id;
        [ObservableProperty] 
        private string nome;
        [ObservableProperty] 
        private string email;
        [ObservableProperty] 
        private string username;
        [ObservableProperty] 
        private string nivelExperiencia;

        [ObservableProperty] 
        private bool isLoading;

        // Lista de opções para o Picker
        public ObservableCollection<string> ListaNiveis { get; } = new ObservableCollection<string>
        {
            "Junior",
            "Pleno",
            "Sênior",
            "Top Xuxa" // Mantive sua opção criativa :)
        };

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
        private async Task SalvarAsync()
        {
            if (IsLoading) return;

            // Validação simples
            if (string.IsNullOrWhiteSpace(Nome) || string.IsNullOrWhiteSpace(Username))
            {
                await Shell.Current.DisplayAlert("Erro", "Nome e Username são obrigatórios.", "OK");
                return;
            }

            try
            {
                IsLoading = true;

                // Montamos o objeto ProgramadorDetalhe para envio
                var programadorParaAtualizar = new ProgramadorDetalhe
                {
                    Id = this.Id,
                    Nome = this.Nome,
                    Email = this.Email,
                    Username = this.Username,
                    NivelExperiencia = this.NivelExperiencia,
                    GestorResponsavel = this.GestorResponsavelNome
                };

                // Chama o método: UpdateProgramadorAsync(ProgramadorDetalhe programador)
                bool sucesso = await _service.UpdateProgramadorAsync(programadorParaAtualizar);

                if (sucesso)
                {
                    await Shell.Current.DisplayAlert("Sucesso", "Dados atualizados com sucesso!", "OK");
                    await Shell.Current.GoToAsync(".."); // Volta para a tela anterior
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erro", "Falha ao atualizar. Verifique os dados ou a conexão.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task DeletarAsync()
        {
            if (IsLoading) return;

            bool confirm = await Shell.Current.DisplayAlert("Atenção",
                $"Tem a certeza que deseja remover o programador {Nome}?",
                "Remover", "Cancelar");

            if (!confirm) return;

            try
            {
                IsLoading = true;

                // Chama o método: DeleteProgramadorAsync(int id)
                bool sucesso = await _service.DeleteProgramadorAsync(this.Id);

                if (sucesso)
                {
                    await Shell.Current.DisplayAlert("Sucesso", "Programador removido.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erro", "Não foi possível remover o programador.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
        // Precisamos chamar este método quando a ViewModel for iniciada.
        // Podes fazer isso no OnProgramadorChanged ou usando EventToCommand no Appearing.
        /*partial void OnProgramadorInicialChanged(ProgramadorDetalhe value)
        {
            if (value != null)
            {
                // 1. Preenchemos os campos imediatamente com o que veio da lista (feedback rápido)
                Id = value.Id;
                Nome = value.Nome;
                NivelExperiencia = value.NivelExperiencia;

                // Preenchemos provisoriamente caso venha preenchido, mas vamos buscar atualizado na API
                Email = value.Email;
                Username = value.Username;
                GestorResponsavelNome = value.GestorResponsavel;

                // 2. Buscamos os detalhes completos na API para garantir dados frescos
                // (Executa em background para não travar a UI)
                Task.Run(async () => await CarregarDetalhesCompletosAsync(value.Id));
            }
        }*/

        private async Task CarregarDetalhesCompletosAsync(int idProgramador)
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;

                // Chama o novo método que retorna o ProgramadorDetalhe
                var detalhe = await _service.GetDetalheAsync(idProgramador);

                if (detalhe != null)
                {
                    // Atualiza a tela com os dados completos vindos do banco
                    Nome = detalhe.Nome;
                    Email = detalhe.Email;
                    Username = detalhe.Username;
                    NivelExperiencia = detalhe.NivelExperiencia;
                    GestorResponsavelNome = detalhe.GestorResponsavel;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar detalhes: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // Verifica se a chave "Id" foi passada na navegação
            if (query.ContainsKey("Id"))
            {
                // O valor vem como string ou object, convertemos para int
                string idString = query["Id"].ToString();

                if (int.TryParse(idString, out int idRecebido))
                {
                    this.Id = idRecebido;

                    // 1. Opcional: Limpar os campos visuais para não mostrar dados da navegação anterior
                    Nome = string.Empty;
                    Email = string.Empty;
                    Username = string.Empty;
                    NivelExperiencia = string.Empty;
                    GestorResponsavelNome = string.Empty;

                    // 2. Chamar a API para buscar os dados
                    // Usamos MainThread para garantir segurança ao disparar o async
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await CarregarDetalhesCompletosAsync(this.Id);
                    });
                }
            }
        }
        #endregion
    }
}
