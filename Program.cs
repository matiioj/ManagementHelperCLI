using System;
using System.CommandLine;
using System.Threading.Tasks;
using System.Management;
using System.Net.NetworkInformation;


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
            Console.WriteLine("Operating System: " + Environment.OSVersion);
            Console.WriteLine("OS Architecture: " + (Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit"));

            // Información de la dirección MAC e IP
            GetNetworkInfo();

            DisplayHardwareInfo();

            // Información del disco duro
            GetDiskInfo();

            // Información del monitor
            GetMonitorInfo();

            // Información de la placa madre
            GetMotherboardInfo();
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

        static string FormatBytes(ulong bytes)
        {
            const int scale = 1024;
            string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
            ulong max = (ulong)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }

        static void GetNetworkInfo()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    var macAddress = networkInterface.GetPhysicalAddress();
                    var ipProperties = networkInterface.GetIPProperties();
                    var ipv4Address = ipProperties.UnicastAddresses
                        .FirstOrDefault(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.Address.ToString();

                    Console.WriteLine($"Network Interface: {networkInterface.Name}");
                    Console.WriteLine($"MAC Address: {macAddress}");
                    Console.WriteLine($"IP Address: {ipv4Address}");
                }
            }
        }

        static void DisplayHardwareInfo()
        {
            // Usar GC para obtener información de memoria disponible para la aplicación
            var totalMemory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / (1024 * 1024); // en MB
            var usedMemory = GC.GetTotalMemory(false) / (1024 * 1024); // en MB

            Console.WriteLine($"Memoria RAM Total disponible para la aplicación: {totalMemory} MB");
            Console.WriteLine($"Memoria RAM usada por la aplicación: {usedMemory} MB");

            // Obtener información del procesador
            Console.WriteLine($"Procesador: {Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")}");
        }

        static void GetDiskInfo()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select * from Win32_DiskDrive");
                foreach (var item in searcher.Get())
                {
                    Console.WriteLine("Disk Model: " + item["Model"]);
                    Console.WriteLine("Disk Size: " + FormatBytes((ulong)item["Size"]));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error retrieving disk information: " + e.Message);
            }
        }

        static void GetMonitorInfo()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select * from Win32_DesktopMonitor");
                foreach (var item in searcher.Get())
                {
                    Console.WriteLine("Monitor Name: " + item["Name"]);
                    Console.WriteLine("Screen Width: " + item["ScreenWidth"]);
                    Console.WriteLine("Screen Height: " + item["ScreenHeight"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error retrieving monitor information: " + e.Message);
            }
        }

        static void GetMotherboardInfo()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select * from Win32_BaseBoard");
                foreach (var item in searcher.Get())
                {
                    Console.WriteLine("Motherboard Manufacturer: " + item["Manufacturer"]);
                    Console.WriteLine("Motherboard Product: " + item["Product"]);
                    Console.WriteLine("Motherboard Serial Number: " + item["SerialNumber"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error retrieving motherboard information: " + e.Message);
            }
        }
    }
}
