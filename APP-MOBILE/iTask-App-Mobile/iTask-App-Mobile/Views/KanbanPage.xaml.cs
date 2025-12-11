using iTask_App_Mobile.Models;
using iTask_App_Mobile.ViewModels;

namespace iTask_App_Mobile.Views;

public partial class KanbanPage : ContentPage
{
    private readonly KanbanPageViewModel _viewModel;

    public KanbanPage(KanbanPageViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
    }

    // 1. INÍCIO DO ARRASTO
    // Ocorre quando você segura o card
    private void OnDragStarting(object sender, DragStartingEventArgs e)
    {
        var frame = (Element)sender;
        var tarefa = (TarefaModel)frame.BindingContext;

        if (tarefa != null)
        {
            // Coloca a tarefa no "pacote" de dados do arrasto
            e.Data.Properties.Add("TarefaArrastada", tarefa);
        }
    }

    // 2. SOLTAR EM "TO DO"
    private async void OnDropToDo(object sender, DropEventArgs e)
    {
        await ProcessarDrop(e, "To Do");
    }

    // 3. SOLTAR EM "DOING"
    private async void OnDropDoing(object sender, DropEventArgs e)
    {
        await ProcessarDrop(e, "Doing");
    }

    // 4. SOLTAR EM "DONE"
    private async void OnDropDone(object sender, DropEventArgs e)
    {
        await ProcessarDrop(e, "Done");
    }

    // LÓGICA DE MOVIMENTAÇÃO
    private async Task ProcessarDrop(DropEventArgs e, string novoEstado)
    {
        // Tenta recuperar a tarefa do pacote
        if (e.Data.Properties.ContainsKey("TarefaArrastada"))
        {
            var tarefa = e.Data.Properties["TarefaArrastada"] as TarefaModel;

            if (tarefa != null)
            {
                await _viewModel.MoverTarefaAsync(tarefa, novoEstado);
            }
        }
    }
}