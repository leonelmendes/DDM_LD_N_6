using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }
}