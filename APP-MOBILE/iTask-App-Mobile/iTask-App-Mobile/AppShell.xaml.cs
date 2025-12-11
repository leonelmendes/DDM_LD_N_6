using iTask_App_Mobile.Views;

namespace iTask_App_Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            #region Routing
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("AddProgramadorPage", typeof(AddProgramadorPage));
            Routing.RegisterRoute("CriarTarefaPage", typeof(CriarTarefaPage));
            Routing.RegisterRoute("DashboardGestorPage", typeof(DashboardGestorPage));
            Routing.RegisterRoute("DashboardProgramadorPage", typeof(DashboardProgramadorPage));
            Routing.RegisterRoute("DetalheProgramadorPage", typeof(DetalheProgramadorPage));
            Routing.RegisterRoute("EditarTarefaPage", typeof(EditarTarefaPage));
            Routing.RegisterRoute("EquipePage", typeof(EquipePage));
            Routing.RegisterRoute("KanbanPage", typeof(KanbanPage));
            Routing.RegisterRoute("PerfilPage", typeof(PerfilPage));
            Routing.RegisterRoute("RelatorioPage", typeof(RelatorioPage));
            Routing.RegisterRoute("TarefaConcluidaPage", typeof(TarefaConcluidaPage));
            Routing.RegisterRoute("TarefaDetailPage", typeof(TarefaDetailPage));
            Routing.RegisterRoute("TarefaListPage", typeof(TarefaListPage));
            Routing.RegisterRoute("TarefasConcluidasGestorPage", typeof(TarefasConcluidasGestorPage));
            #endregion
        }

        // ========== MÉTODOS PARA MOSTRAR O TABBAR CORRETO ==========

    }
}
