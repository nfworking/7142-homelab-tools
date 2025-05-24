using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.Threading;

namespace PreReqChk
{
    class Program
    {
        static readonly string logFile = "prereq-check.log";
        static readonly bool logToFile = true;

        static void Main()
        {
            Console.Title = "Prerequisite Checker";
            Log("\n=== Post Deployment Prerequisite Checker ===\n");

            var checks = new List<PrerequisiteCheck>
            {
                new PrerequisiteCheck("Operating System", CheckOSEdition),
                new PrerequisiteCheck("Installed RAM", CheckRAM),
                new PrerequisiteCheck("Logical Processors", CheckProcessorCount),
                new PrerequisiteCheck("Subnet Mask", CheckSubnetMask)
            };

            bool allPassed = true;
            foreach (var check in checks)
            {
                Log($"Running check: {check.Name}...");
                bool result = check.Execute();
                allPassed &= result;
                Log($"Finished check: {check.Name}\n");
            }

            Log("\n----------------------------------------");
            if (allPassed)
            {
                WriteColored("✔ All checks passed! Continuing in 20 seconds...", ConsoleColor.Green);
                Thread.Sleep(20000);

                string exePath = "PreReqChkSel.exe";
                if (File.Exists(exePath))
                {
                    Log($"Launching Deployment Selector {exePath}...");
                    Process.Start(exePath);
                }
                else
                {
                    WriteColored($"✖ {exePath} not found.", ConsoleColor.Red);
                }
            }
            else
            {
                WriteColored("✖ Some checks failed. Press any key to exit...", ConsoleColor.Red);
                Console.ReadKey();
            }
        }

        class PrerequisiteCheck
        {
            public string Name { get; }
            private Func<bool> CheckFunction;

            public PrerequisiteCheck(string name, Func<bool> func)
            {
                Name = name;
                CheckFunction = func;
            }

            public bool Execute() => CheckFunction();
        }

        static bool CheckOSEdition()
        {
            string edition = "Unknown";
            using (var searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem"))
            {
                foreach (var os in searcher.Get())
                {
                    edition = os["Caption"]?.ToString() ?? "Unknown";
                    break;
                }
            }

            string lowerEdition = edition.ToLower();

            bool pass = lowerEdition.Contains("pro") ||
                        lowerEdition.Contains("professional") ||
                        lowerEdition.Contains("enterprise") ||
                        lowerEdition.Contains("education");

            PrintResult("Operating System", edition, pass);
            return pass;
        }

        static bool CheckRAM()
        {
            ulong totalMemory = 0;
            using (var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory"))
            {
                foreach (var obj in searcher.Get())
                {
                    totalMemory += (ulong)obj["Capacity"];
                }
            }

            double totalGB = Math.Round(totalMemory / (1024.0 * 1024 * 1024), 2);
            bool pass = totalGB >= 2;
            PrintResult("Installed RAM", $"{totalGB} GB", pass);
            return pass;
        }

        static bool CheckProcessorCount()
        {
            int count = Environment.ProcessorCount;
            bool pass = count >= 2;
            PrintResult("Logical Processors", count.ToString(), pass);
            return pass;
        }

        static bool CheckSubnetMask()
        {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var unicast in nic.GetIPProperties().UnicastAddresses)
                    {
                        if (unicast.IPv4Mask != null && unicast.IPv4Mask.ToString() == "255.255.0.0")
                        {
                            PrintResult("Subnet Mask", unicast.IPv4Mask.ToString(), true);
                            return true;
                        }
                    }
                }
            }

            PrintResult("Subnet Mask", "You are not connect to the company network", false);
            return false;
        }

        static void PrintResult(string checkName, string detail, bool passed)
        {
            Console.ForegroundColor = passed ? ConsoleColor.Green : ConsoleColor.Red;
            string symbol = passed ? "✔" : "✖";
            string result = $"{symbol} {checkName}: {detail}";
            Console.WriteLine(result);
            Console.ResetColor();
            if (logToFile) File.AppendAllText(logFile, result + Environment.NewLine);
        }

        static void Log(string message)
        {
            Console.WriteLine(message);
            if (logToFile) File.AppendAllText(logFile, message + Environment.NewLine);
        }

        static void WriteColored(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
            if (logToFile) File.AppendAllText(logFile, message + Environment.NewLine);
        }
    }
}
