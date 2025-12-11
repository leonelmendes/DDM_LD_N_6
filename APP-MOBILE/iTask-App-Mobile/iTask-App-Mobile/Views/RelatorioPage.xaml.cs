using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class RelatorioPage : ContentPage
{
	public RelatorioPage(RelatorioPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}