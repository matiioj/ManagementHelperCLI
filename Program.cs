using Microsoft.Extensions.DependencyInjection;
using Cocona;


namespace ManagementHelperCLI
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var builder = CoconaApp.CreateBuilder();
            builder.Services.AddHttpClient();
            //builder.Services.AddSingleton<IWeatherService, OpenWeatherMapService>();
            var app = builder.Build();
            app.AddCommands<InfoCommands>();
            app.AddCommands<SysinfoCommands>();
            add.AddCommands<OfficeCommands>();

            app.Run();

            await Task.CompletedTask;
        }




        static void SetServicesToManual()
        {
            Console.WriteLine("Setting services to manual...");
            // Lógica para configurar servicios a manual
        }

        static void UpdateBrowserSettings()
        {
            Console.WriteLine("Updating browser settings...");
            // Lógica para actualizar configuración de navegadores
        }

        static void ResetInternetAdapter()
        {
            Console.WriteLine("Resetting internet adapter...");
            // Lógica para resetear el adaptador de internet
        }

        static void ActivateWindows()
        {
            Console.WriteLine("Activating Windows...");
            // Lógica para activar Windows
        }

        static void ActivateOffice()
        {
            Console.WriteLine("Activating Office...");
            // Lógica para activar Office
        }
    }
}
