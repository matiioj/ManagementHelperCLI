using System.Management;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace ManagementHelperCLI
{
    public class DisplaySysInfoService
    {
        public DisplaySysInfoService()
        {

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

        public static void DisplayRamInfo()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");

            foreach (var obj in searcher.Get())
            {
                // Each ManagementObject represents a RAM stick
                var ram = (ManagementObject)obj;

                ulong capacity = (ulong)ram["Capacity"];
                string capacityInGB = (capacity / (1024 * 1024 * 1024)).ToString() + " GB";

                string manufacturer = ram["Manufacturer"]?.ToString() ?? "Unknown";
                string partNumber = ram["PartNumber"]?.ToString() ?? "Unknown";
                ushort memoryTypeCode = (ushort)ram["MemoryType"];
                string memoryType = GetMemoryType(memoryTypeCode);

                Console.WriteLine($"RAM Capacity: {capacityInGB}");
                Console.WriteLine($"Manufacturer: {manufacturer}");
                Console.WriteLine($"Model (Part Number): {partNumber}");
                Console.WriteLine($"Memory Type: {memoryType}");
            }
        }

        public static void DisplayProcessorInfo()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select * from Win32_Processor");

                foreach (var item in searcher.Get())
                {
                    Console.WriteLine($"Processor Name: {item["Name"]}");
                    Console.WriteLine($"Manufacturer: {item["Manufacturer"]}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error retrieving processor information: " + e.Message);
            }
        }

        public static void GetDiskInfo()
        {
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = @"
                    Get-PhysicalDisk | Select-Object DeviceID, Model, @{Name='Size(GB)';Expression={[math]::round($_.Size / 1GB, 2)}}, MediaType",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                using (var reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error retrieving disk information via PowerShell: " + e.Message);
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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error retrieving motherboard information: " + e.Message);
            }
        }

        private static string GetMemoryType(ushort memoryTypeCode)
        {
            return memoryTypeCode switch
            {
                20 => "DDR",
                21 => "DDR2",
                24 => "DDR3",
                26 => "DDR4",
                // Add other memory types as needed
                _ => "Unknown"
            };
        }

    }
}