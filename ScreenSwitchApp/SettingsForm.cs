using System.Windows.Forms;

namespace ScreenSwitchApp
{
    public partial class SettingsForm : Form
    {
        private static SettingsForm instance;
        public static SettingsForm Instance {
            get
            {
                if (instance == null)
                {
                    instance = new SettingsForm();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        private SettingsForm()
        {
            Instance = this;
            Instance.FormBorderStyle = FormBorderStyle.Fixed3D;
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, System.EventArgs e)
        {
            Instance = this;
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Instance = null;
        }
    }
}
