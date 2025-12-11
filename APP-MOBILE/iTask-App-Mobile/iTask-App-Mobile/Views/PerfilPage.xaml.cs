using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class PerfilPage : ContentPage
{
	public PerfilPage(PerfilPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }
}