using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using WindowsInput.Native;

namespace ScreenSwitchApp
{
    public partial class ScreenSwitch : Form
    {
        private readonly LowLevelKeyboardListener KeyboardListener = new LowLevelKeyboardListener();

        //Registry
        private readonly string RegistryKey = $"HKEY_CURRENT_USER\\Software\\{FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName).ProductName}";
        private readonly string StartupValue = "Startup";

        //Startup
        private readonly string AppName = Assembly.GetEntryAssembly().GetName().Name;
        private readonly string AppPath = Process.GetCurrentProcess().MainModule.FileName;

        public ScreenSwitch()
        {
            InitializeComponent();
            KeyboardListener.KeyDown += KeyboardListener_KeyDown;
        }

        private void KeyboardListener_KeyDown(VirtualKeyCode obj)
        {
            if (obj == VirtualKeyCode.F11)
            {
                RunCMDCommand("1");

            }
            else if (obj == VirtualKeyCode.F12)
            {
                RunCMDCommand("4");
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ScreenSwitch form = sender as ScreenSwitch;
            form.Hide();
            notifyIcon.Visible = true;

            bool? startWithWindows = (int)Registry.GetValue(RegistryKey, StartupValue, false) == 1;

            CheckBox startupCheckbox = new CheckBox()
            {
                Text = "Start with Windows",
                Checked = startWithWindows.GetValueOrDefault()
            };
            startupCheckbox.CheckedChanged += OnStartupCheckChanged;

            contextMenuStrip.Items.Add(new ToolStripControlHost(startupCheckbox));
            contextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, ExitProgram));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Unhook the keyboard listener when the form is closing
            KeyboardListener.UnhookKeyboard();
            base.OnFormClosing(e);
        }

        private void RunCMDCommand(string argument)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = @"DisplaySwitch.exe",
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = argument
            };

            // Create the process
            Process process = new Process { StartInfo = processStartInfo };

            // Start the process
            process.Start();
        }

        private void ExitProgram(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void OnStartupCheckChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                bool checkedState = checkBox.Checked;

                //Remember the checkbox state
                Registry.SetValue(RegistryKey, StartupValue, checkedState, RegistryValueKind.DWord);

                //Now deal with statup
                if (checkedState)
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                    {
                        // Add a Registry entry to start your app on startup
                        key.SetValue(AppName, AppPath);
                    }
                }
                else
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                    {
                        // Remove a Registry entry
                        key.DeleteValue(AppName);
                    }
                }
            }
        }
    }
}
