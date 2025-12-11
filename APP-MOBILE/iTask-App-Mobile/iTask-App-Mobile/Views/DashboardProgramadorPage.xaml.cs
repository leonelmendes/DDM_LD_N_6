using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class DashboardProgramadorPage : ContentPage
{
    private readonly DashboardProgramadorPageViewModel _viewModel;
    public DashboardProgramadorPage(DashboardProgramadorPageViewModel vm)
	{
		InitializeComponent();
       _viewModel = vm;
         BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DashboardProgramadorPageViewModel vm)
        {
            vm.CarregarDashboardCommand.Execute(null);
        }
    }
}