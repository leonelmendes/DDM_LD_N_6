using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class EditarPerfilPage : ContentPage
{
	public EditarPerfilPage(EditarPerfilPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}