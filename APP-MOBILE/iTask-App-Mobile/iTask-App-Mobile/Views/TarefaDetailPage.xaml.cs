using iTask_App_Mobile.Models;
using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

//[QueryProperty(nameof(Tarefa), "TarefaSelecionada")]
public partial class TarefaDetailPage : ContentPage
{

    public TarefaDetailPage(TarefaDetailPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}