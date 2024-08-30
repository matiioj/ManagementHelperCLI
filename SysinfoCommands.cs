using Cocona;

namespace ManagementHelperCLI
{


    public class SysinfoCommands
    {
        public SysinfoCommands()
        {
        }

        [Command("sysinfo", Description = "Display information from the system and network of this PC.")]

         public static void DisplaySyscomp()
        {
            Console.WriteLine("Displaying system components...");
            Console.WriteLine(new string('-', 40));


            DisplaySysInfoService.GetOSInfo();
            Console.WriteLine(new string('-', 40));
            
            DisplaySysInfoService.GetNetworkInfo();
            Console.WriteLine(new string('-', 40));

            DisplaySysInfoService.DisplayRamInfo();
            Console.WriteLine(new string('-', 40));

            DisplaySysInfoService.DisplayProcessorInfo();
            Console.WriteLine(new string('-', 40));

            DisplaySysInfoService.GetDiskInfo();
            Console.WriteLine(new string('-', 40));
            
            DisplaySysInfoService.GetMonitorInfo();
            Console.WriteLine(new string('-', 40));
            
            DisplaySysInfoService.GetMotherboardInfo();
            Console.WriteLine(new string('-', 40));
        }

    }
}