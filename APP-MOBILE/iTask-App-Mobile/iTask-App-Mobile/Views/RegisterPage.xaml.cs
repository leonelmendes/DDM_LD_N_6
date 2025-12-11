using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }
}