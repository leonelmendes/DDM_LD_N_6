using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class DetalheProgramadorPage : ContentPage
{
	public DetalheProgramadorPage(DetalheProgramadorPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}