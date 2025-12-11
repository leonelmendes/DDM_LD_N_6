using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class TarefaConcluidaPage : ContentPage
{
	public TarefaConcluidaPage(TarefasConcluidasPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Garante recarregamento ao entrar na tela
        if (BindingContext is TarefasConcluidasPageViewModel vm)
        {
            vm.CarregarTarefasCommand.Execute(null);
        }
    }
}