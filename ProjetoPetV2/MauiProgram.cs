using Microsoft.Extensions.Logging;
using ProjetoPetV2.Data;
using ProjetoPetV2.ViewModels;

namespace ProjetoPetV2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddTransient<UsuarioViewModel>();
            builder.Services.AddTransient<View.CadastroPet>();

            return builder.Build();
        }
    }
}
