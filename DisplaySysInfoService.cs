using System.Management;
using System.Net.NetworkInformation;

namespace ManagementHelperCLI
{
    class DisplaySysInfoService
    {
        public DisplaySysInfoService()
        {

        }

        public static void DisplaySyscomp()
        {
            Console.WriteLine("Displaying system components...");
            
            GetOSInfo();

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


        public static void GetOSInfo() 
        {
            Console.WriteLine("Operating System: " + Environment.OSVersion);
            Console.WriteLine("OS Architecture: " + (Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit"));
        }

        public static void GetNetworkInfo()
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
                    Console.WriteLine("");
                }
            }
        }

        public static void DisplayHardwareInfo()
        {
            // Usar GC para obtener información de memoria disponible para la aplicación
            var totalMemory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / (1024 * 1024); // en MB
            var usedMemory = GC.GetTotalMemory(false) / (1024 * 1024); // en MB

            Console.WriteLine($"Memoria RAM Total disponible para la aplicación: {totalMemory} MB");
            Console.WriteLine($"Memoria RAM usada por la aplicación: {usedMemory} MB");

            // Obtener información del procesador
            Console.WriteLine($"Procesador: {Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")}");
        }

        public static void GetDiskInfo()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select * from Win32_DiskDrive");
                foreach (var item in searcher.Get())
                {
                    Console.WriteLine("Disk Model: " + item["Model"]);
                    Console.WriteLine("Disk Size: " + FormattingService.FormatBytes((ulong)item["Size"]));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error retrieving disk information: " + e.Message);
            }
        }

        public static void GetMonitorInfo()
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

        public static void GetMotherboardInfo()
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