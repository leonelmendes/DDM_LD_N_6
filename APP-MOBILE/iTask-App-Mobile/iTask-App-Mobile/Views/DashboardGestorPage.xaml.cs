using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class DashboardGestorPage : ContentPage
{
	public DashboardGestorPage(DashboardGestorPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }
}