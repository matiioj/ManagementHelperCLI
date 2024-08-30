using System;
using System.Diagnostics;

namespace ManagementHelperCLI
{
    public class ActivateOfficeService
    {
        public ActivateOfficeService() { }

        string officePath = @"C:\Program Files\Microsoft Office\Office16"; 
        
         public void ExecuteActivation()
        {
            Console.WriteLine("Introduce your Office key:");
            string key = Console.ReadLine();

            if (!string.IsNullOrEmpty(key))
            {
                string inputKeyCommand = $"cscript ospp.vbs /inpkey:{key}";
                ExecuteCommand(inputKeyCommand);
                ExecuteCommand("cscript ospp.vbs /act");
                ExecuteCommand("cscript ospp.vbs /dstatus");
            }
            else
            {
                Console.WriteLine("No key provided.");
            }
        }

        private void ExecuteCommand(string command)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = officePath
                };

                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    Console.WriteLine(output);
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine("Error: " + error);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
                

    }

}