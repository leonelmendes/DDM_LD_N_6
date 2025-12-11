using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class EditarTarefaPage : ContentPage
{
    private readonly EditarTarefaPageViewModel _viewModel;
    public EditarTarefaPage(EditarTarefaPageViewModel vm)
	{
		InitializeComponent();

        _viewModel = vm;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Opção 1: Usando a referência privada salva no construtor
        if (_viewModel != null)
        {
            await _viewModel.CarregarTiposTarefa();
        }
    }
}