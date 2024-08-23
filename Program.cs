using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace ManagementHelperCLI
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            // Crear el comando raíz
            var rootCommand = new RootCommand("ManagementHelperCLI - A CLI tool to manage system configurations.");

            var helpOption = new Option<bool>(
                new[] { "-h", "--help" },
                "Display help");
            rootCommand.AddOption(helpOption);

            var infoOption = new Option<bool>(
                new[] { "-i", "--info" },
                "Display information about the CLI");
            rootCommand.AddOption(infoOption);

            var syscompCommand = new Command("syscomp", "Display system components and system information and copy to clipboard.");
            syscompCommand.SetHandler(DisplaySyscomp);
            rootCommand.AddCommand(syscompCommand);

            var setmanualCommand = new Command("setmanual", "Set services to manual.");
            setmanualCommand.SetHandler(SetServicesToManual);
            rootCommand.AddCommand(setmanualCommand);

            var browsersupdateCommand = new Command("browsersupdate", "Turn manual or automatic your browser updates.");
            browsersupdateCommand.SetHandler(UpdateBrowserSettings);
            rootCommand.AddCommand(browsersupdateCommand);

            var netresetCommand = new Command("netreset", "Reset your internet adapter.");
            netresetCommand.SetHandler(ResetInternetAdapter);
            rootCommand.AddCommand(netresetCommand);

            var winkeyCommand = new Command("winkey", "Activate your Windows.");
            winkeyCommand.SetHandler(ActivateWindows);
            rootCommand.AddCommand(winkeyCommand);

            var officekeyCommand = new Command("officekey", "Activate your Office package.");
            officekeyCommand.SetHandler(ActivateOffice);
            rootCommand.AddCommand(officekeyCommand);

            // Asignar el manejador para manejar opciones globales
            rootCommand.SetHandler((bool help, bool info) =>
            {
                if (help)
                {
                    ShowInitialHelp();
                }
                else if (info)
                {
                    ShowInfo();
                }
                else
                {
                    Console.WriteLine("No valid option selected. Type 'managehelp --help' to see the available commands.");
                }
            }, helpOption, infoOption);

            return await rootCommand.InvokeAsync(args);
        }

        static void ShowInitialHelp()
        {
            Console.WriteLine("Usage: managehelp [options]");
            Console.WriteLine("");
            Console.WriteLine("Options:\n ");
            Console.WriteLine("-h | --help      Display help");
            Console.WriteLine("-i | --info      Display information about the CLI");
            ShowCommandsHelp();
        }

        static void ShowInfo()
        {
            Console.WriteLine("ManagementHelperCLI - Version 1.0");
            Console.WriteLine("This CLI tool helps you manage various system settings and configurations.");
        }

        static void ShowCommandsHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("Commands of use:");
            Console.WriteLine("syscomp        Display system components and system information and copy to clipboard.");
            Console.WriteLine("setmanual      Set services to manual");
            Console.WriteLine("browsersupdate Turn manual or automatic your browser updates.");
            Console.WriteLine("netreset       Reset your internet adapter.");
            Console.WriteLine("winkey         Activate your Windows");
            Console.WriteLine("officekey      Activate your Office package");
        }

        static void DisplaySyscomp()
        {
            Console.WriteLine("Displaying system components...");
            // Lógica para mostrar componentes del sistema
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
