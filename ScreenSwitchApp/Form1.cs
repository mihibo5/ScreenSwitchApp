using System;
using System.Diagnostics;
using System.Windows.Forms;
using WindowsInput.Native;

namespace ScreenSwitchApp
{
    public partial class ScreenSwitch : Form
    {
        private readonly LowLevelKeyboardListener KeyboardListener = new LowLevelKeyboardListener();

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
    }
}
