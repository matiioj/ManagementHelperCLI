using Cocona;

namespace ManagementHelperCLI
{


    public class HelpCommands
    {
        public HelpCommands()
        {
        }

        [Command("info", Description = "Display help information about the CLI.")]
        public static void ShowInfo()
        {
            Console.WriteLine("ManagementHelperCLI - Version 1.0");
            Console.WriteLine("This CLI tool helps you manage various system settings and configurations.");
        }
    }
}