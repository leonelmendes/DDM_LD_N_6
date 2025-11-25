using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using iTask_App_Mobile.Services.AuthenticateService;
using iTask_App_Mobile.Services.GestorService;
using iTask_App_Mobile.Services.ProgramadorService;
using iTask_App_Mobile.Services.TarefaService;
using iTask_App_Mobile.Services.TipoTarefaService;
using iTask_App_Mobile.Services.UtilizadorService;
using iTask_App_Mobile.ViewModels;
using iTask_App_Mobile.Views;
using Microsoft.Extensions.Logging;

namespace iTask_App_Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit(options =>
                {
                    options.SetShouldEnableSnackbarOnWindows(true);
                })
                .UseMauiCommunityToolkitCore()
                .ConfigureFonts(fonts =>
                {
                    
                    fonts.AddFont("Inter-Thin.ttf", "InterThin");                 // Usar para textos muito leves / dicas
                    fonts.AddFont("Inter-ExtraLight.ttf", "InterExtraLight");     // Textos decorativos, subtítulos muito suaves
                    fonts.AddFont("Inter-Light.ttf", "InterLight");               // Placeholders, textos secundários
                    fonts.AddFont("Inter-Regular.ttf", "InterRegular");           // Texto padrão de parágrafos / inputs
                    fonts.AddFont("Inter-Medium.ttf", "InterMedium");             // Labels, botões pequenos, menus
                    fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");         // Botões principais, títulos médios
                    fonts.AddFont("Inter-Bold.ttf", "InterBold");                 // Títulos grandes, headers, seção destacada
                    fonts.AddFont("Inter-ExtraBold.ttf", "InterExtraBold");       // Para títulos fortes (home/dashboard)
                    fonts.AddFont("Inter-Black.ttf", "InterBlack");               // Uso raríssimo – grandes banners / impacto visual
                    
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            // Services
            builder.Services.AddSingleton<IAuthenticateService, AuthenticateService>();
            builder.Services.AddSingleton<IUtilizadorService, UtilizadorService>();
            builder.Services.AddSingleton<IGestorService, GestorService>();
            builder.Services.AddSingleton<IProgramadorService, ProgramadorService>();
            builder.Services.AddSingleton<ITarefaService, TarefaService>();
            builder.Services.AddSingleton<ITipoTarefaService, TipoTarefaService>();
            // View Models
            builder.Services.AddTransient<RegisterPageViewModel>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<DashboardGestorPageViewModel>();
            builder.Services.AddTransient<PerfilPageViewModel>();

            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<DashboardGestorPage>();
            builder.Services.AddTransient<DashboardProgramadorPage>();
            builder.Services.AddTransient<AddProgramadorPage>();
            builder.Services.AddTransient<CriarTarefaPage>();
            builder.Services.AddTransient<EquipePage>();
            builder.Services.AddTransient<DetalheProgramadorPage>();
            builder.Services.AddTransient<EditarTarefaPage>();
            builder.Services.AddTransient<KanbanPage>();
            builder.Services.AddTransient<PerfilPage>();
            builder.Services.AddTransient<RelatorioPage>();
            builder.Services.AddTransient<TarefaConcluidaPage>();
            builder.Services.AddTransient<TarefaDetailPage>();
            builder.Services.AddTransient<TarefaListPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
