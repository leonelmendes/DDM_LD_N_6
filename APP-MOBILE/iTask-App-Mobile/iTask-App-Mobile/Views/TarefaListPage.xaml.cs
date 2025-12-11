using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class TarefaListPage : ContentPage
{
    private readonly TarefaListPageViewModel _viewModel;

    public TarefaListPage(TarefaListPageViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // Recarrega a lista sempre que a página aparecer (ex: ao voltar da edição)
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Força o carregamento dos dados
        if (_viewModel.CarregarTarefasCommand.CanExecute(null))
        {
            _viewModel.CarregarTarefasCommand.Execute(null);
        }
    }
}