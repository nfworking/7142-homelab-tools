using System;
using System.Diagnostics;

Console.WriteLine("****************************8This is going to execute Portable Joiner*************************");
Console.WriteLine(String.Empty);
Console.WriteLine(String.Empty);
Console.WriteLine(String.Empty);

static void WriteWinRegUpdate()
{
    // PowerShell commands to stop and disable the Windows Update service
    string psScript = @"
            Stop-Service -Name wuauserv -Force -ErrorAction SilentlyContinue
            Set-Service -Name wuauserv -StartupType Disabled -ErrorAction SilentlyContinue
        ";

    // Start PowerShell process with the script
    ProcessStartInfo psi = new ProcessStartInfo()
    {
        FileName = "powershell.exe",
        Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{psScript}\"",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,
        Verb = "runas" // Run as admin
    };

    using (Process process = new Process())
    {
        process.StartInfo = psi;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string errors = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (!string.IsNullOrEmpty(errors))
        {
            Console.WriteLine("Errors: " + errors);
        }
        else
        {
            Console.WriteLine("Windows Update service disabled successfully.");
        }
    }
}

static void WriteWinPowerUpdate()
{
    string[] commands = new string[]
    {
            // Set power plan to High Performance
            "powercfg -setactive SCHEME_MIN",

            // Disable system sleep and display turn-off timeouts (set to 0 = never)
            "powercfg -change -standby-timeout-ac 0",
            "powercfg -change -monitor-timeout-ac 0",
            "powercfg -change -hibernate-timeout-ac 0",

            // Set visual effects for best performance via registry
            "reg add \"HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VisualEffects\" /v VisualFXSetting /t REG_DWORD /d 2 /f",
            "reg add \"HKCU\\Control Panel\\Desktop\" /v UserPreferencesMask /t REG_BINARY /d 9012008010000000 /f"
    };

    foreach (var command in commands)
    {
        ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + command)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            Verb = "runas"
        };

        using (Process process = Process.Start(psi))
        {
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (!string.IsNullOrWhiteSpace(error))
            {
                Console.WriteLine($"Error: {error}");
            }
        }
    }

    Console.WriteLine("Power settings and appearance updated to best performance.");
}



static void WriteWinLogonDesc()
{
    string command = "reg add \"HKLM\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System\" /v VerboseStatus /t REG_DWORD /d 1 /f";

    ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + command)
    {
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        Verb = "runas" // Ensure it runs as administrator
    };

    using (Process process = Process.Start(psi))
    {
        process.WaitForExit();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        if (!string.IsNullOrWhiteSpace(error))
        {
            Console.WriteLine("Error enabling verbose logon: " + error);
        }
        else
        {
            Console.WriteLine("Verbose logon messages have been enabled.");
        }
    }
}

static void WriteWinCompName()
{
    Console.WriteLine("Not changing computer name, logic is production");
}

static void WriteWinJoinComp()
{
    string domain = "corp.bsg.local";
    string ou = "OU=Workstation,DC=corp,DC=bsg,DC=local";
    string user = "CORP\\joinerservice"; // <-- Replace with actual domain user
    string password = "runcorn.1";         // <-- Replace with actual password

    string command = $"powershell -Command \"Add-Computer -DomainName '{domain}' -OUPath '{ou}' -Credential (New-Object System.Management.Automation.PSCredential('{user}',(ConvertTo-SecureString '{password}' -AsPlainText -Force))) -PassThru -Verbose -ErrorAction Stop\"";

    ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + command)
    {
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        Verb = "runas"
    };

    using (Process process = Process.Start(psi))
    {
        process.WaitForExit();
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        Console.WriteLine("Join Output:\n" + output);
        if (!string.IsNullOrWhiteSpace(error))
        {
            Console.WriteLine("Join Error:\n" + error);
        }
        else
        {
            Console.WriteLine("Successfully joined domain without reboot.");
        }
    }
}

static void WriteWinGroupPol()
{
    string command = "gpupdate /force";

    ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + command)
    {
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        Verb = "runas" // Ensures the command runs with administrative privileges
    };

    using (Process process = Process.Start(psi))
    {
        process.WaitForExit();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        Console.WriteLine("Group Policy Update Output:\n" + output);
        if (!string.IsNullOrWhiteSpace(error))
        {
            Console.WriteLine("Group Policy Update Error:\n" + error);
        }
        else
        {
            Console.WriteLine("Group Policy successfully updated.");
        }
    }
}

Console.WriteLine("Do youwant to run the Portable Joiner? (Y/N)");
string input = Console.ReadLine()?.Trim().ToUpper();
if (input != "Y")
{
  Console.WriteLine("Exiting Portable Joiner");
  return;
}
else
{
  Console.WriteLine("Running Portable Joiner");
  Console.Clear();
  startJoiner();
}
static void startJoiner()
{
Console.WriteLine("Configuring Windows Update.......");
WriteWinRegUpdate();
Console.WriteLine("Windows update Configured");
Thread.Sleep(2000);
Console.Clear();

Console.WriteLine("Configuring Windows Powerplan and Appearence Settings");
WriteWinPowerUpdate();
Console.WriteLine("Configured Settings");
Thread.Sleep(2000);
Console.Clear();

Console.WriteLine("Setting Logon Description");
WriteWinLogonDesc();
Console.WriteLine("Configured Logon Descripton");
Thread.Sleep(2000);
Console.Clear();

Console.WriteLine("Setting Computer Name");
WriteWinCompName();
Console.WriteLine("Set computer name to Value");
Thread.Sleep(2000);
Console.Clear();

Console.WriteLine("Connecting Computer to Domain and moving to OU");
WriteWinJoinComp();
Console.WriteLine("Configured Computer Domain and OU");
Thread.Sleep(2000);
Console.Clear();

Console.WriteLine("Updating Group Policy");
WriteWinGroupPol();
Console.WriteLine("Update Group Policy");
Thread.Sleep(2000);
Console.Clear();

Console.WriteLine("Rebooting.....");
Thread.Sleep(2000);
ProcessStartInfo psi = new ProcessStartInfo("shutdown", "/r /t 0")
{
    CreateNoWindow = true,
    UseShellExecute = false
};

Process.Start(psi);




}
