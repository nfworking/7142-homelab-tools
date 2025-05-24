using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PreReqChkPreview
{
    public partial class MainWindow : Window
    {
        private readonly string logFile = "prereq-check.log";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void RunChecks_Click(object sender, RoutedEventArgs e)
        {
            CheckResults.Items.Clear();
            File.AppendAllText(logFile, $"\n=== Post Deployment Prerequisite Checker ===\n");

            bool allPassed = true;

            var checks = new List<(string Name, Func<Task<(string, bool)>> Func)>
            {
                ("Operating System", async () => await Task.Run(CheckOSEdition)),
                ("Installed RAM", async () => await Task.Run(CheckRAM)),
                ("Logical Processors", async () => await Task.Run(CheckProcessorCount)),
                ("Subnet Mask", async () => await Task.Run(CheckSubnetMask))
            };

            foreach (var (name, checkFunc) in checks)
            {
                AddResult($"Running check: {name}...", Brushes.Gray);
                var (detail, passed) = await checkFunc();
                allPassed &= passed;
                AddResult($"{(passed ? "✔" : "✖")} {name}: {detail}", passed ? Brushes.Green : Brushes.Red);
            }

            AddResult("----------------------------------------", Brushes.Gray);

            if (allPassed)
            {
                AddResult("✔ All checks passed! You can continue.", Brushes.Green);
                ContinueButton.IsEnabled = true;
            }
            else
            {
                AddResult("✖ Some checks failed. Please review above.", Brushes.Red);
            }
        }

        private void AddResult(string message, Brush color)
        {
            var tb = new TextBlock
            {
                Text = message,
                Foreground = color,
                Margin = new Thickness(0, 2, 0, 2)
            };

            CheckResults.Items.Add(tb);
            File.AppendAllText(logFile, message + Environment.NewLine);
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            string exePath = "PreReqChkSel.exe";
            if (File.Exists(exePath))
            {
                Process.Start(exePath);
            }
            else
            {
                MessageBox.Show($"{exePath} not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private (string, bool) CheckOSEdition()
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

            string lower = edition.ToLower();
            bool pass = lower.Contains("pro") || lower.Contains("professional") ||
                        lower.Contains("enterprise") || lower.Contains("education");

            return (edition, pass);
        }

        private (string, bool) CheckRAM()
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
            return ($"{totalGB} GB", pass);
        }

        private (string, bool) CheckProcessorCount()
        {
            int count = Environment.ProcessorCount;
            bool pass = count >= 2;
            return (count.ToString(), pass);
        }

        private (string, bool) CheckSubnetMask()
        {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var unicast in nic.GetIPProperties().UnicastAddresses)
                    {
                        if (unicast.IPv4Mask != null && unicast.IPv4Mask.ToString() == "255.255.0.0")
                        {
                            return (unicast.IPv4Mask.ToString(), true);
                        }
                    }
                }
            }

            return ("You are not connected to the company network", false);
        }
    }
}
