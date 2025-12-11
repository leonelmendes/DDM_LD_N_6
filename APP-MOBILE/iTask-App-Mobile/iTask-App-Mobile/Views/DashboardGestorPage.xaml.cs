using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class DashboardGestorPage : ContentPage
{
	public DashboardGestorPage(DashboardGestorPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
        //vm.LoadEquipeDoGestorAsync();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        // Verificamos se o BindingContext é realmente a nossa ViewModel
        if (BindingContext is DashboardGestorPageViewModel vm)
        {
            // Executamos o comando de carregar, se não estiver já carregando
            if (!vm.IsLoading)
            {
                await vm.LoadEquipeDoGestorAsync();
            }
        }
    }
}