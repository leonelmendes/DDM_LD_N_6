using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class EquipePage : ContentPage
{
	public EquipePage(EquipePageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        // Verificamos se o BindingContext é realmente a nossa ViewModel
        if (BindingContext is EquipePageViewModel vm)
        {
            // Executamos o comando de carregar, se não estiver já carregando
            if (!vm.IsLoading)
            {
                await vm.LoadDataAsync();
            }
        }
    }
}