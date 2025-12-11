using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class TarefasConcluidasGestorPage : ContentPage
{
	public TarefasConcluidasGestorPage(TarefasConcluidasGestorViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}