using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class CriarTarefaPage : ContentPage
{
	public CriarTarefaPage(CriarTarefaPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;


        // Carrega ao abrir a página
        _ = vm.CarregarProgramadoresAsync();
        _ = vm.CarregarTiposTarefaAsync();
    }
}