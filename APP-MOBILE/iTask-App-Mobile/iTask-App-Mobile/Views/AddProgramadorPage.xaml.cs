using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class AddProgramadorPage : ContentPage
{
	public AddProgramadorPage(AddProgramadorPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }
}