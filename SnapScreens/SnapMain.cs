// SnapScreens main/settings form
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SnapScreens
{
    public partial class SnapMain : Form
    {
        public SnapMain()
        {
            Debug.WriteLine($"app start");
            InitializeComponent();
            
            foreach (var screen in Screen.AllScreens) {
                Debug.WriteLine($"{screen.DeviceName}: {screen.Bounds}");
            }

            // load application settings
            if (!Properties.Settings.Default.settings_is_valid) {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.settings_is_valid = true;
                Properties.Settings.Default.Save();
            }
            var hotkey = (Keys)Enum.Parse(typeof(Keys), Properties.Settings.Default.HotKey);
            var modkeys = (MODKEY)Properties.Settings.Default.ModKeys;

            // register global hot key
            for (int i = 1; i < 100; i++)
                if (RegisterHotKey(this.Handle, i, modkeys, hotkey) !=0) {
                    hotkey_id = i;
                    Debug.WriteLine($"RegisterHotKey({hotkey_id}, [{modkeys}]), {hotkey})");
                    break;
                }

            ImageForm.SnapPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, 
                Environment.SpecialFolderOption.Create), "SnapScreens");
            Debug.WriteLine($" SnapPath={ImageForm.SnapPath}");
            //RestoreImages();
        }

        ~SnapMain()
        {
            UnregisterHotKey(this.Handle, hotkey_id);
            Debug.WriteLine($"UnregisterHotKey({hotkey_id})");
        }

        private int hotkey_id = 1;

        private void SnapSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine($"SnapSettings_FormClosing({e.CloseReason})");

            if (e.CloseReason == CloseReason.UserClosing) {
                // hide when close box is clicked
                e.Cancel = true;
                this.Hide();
            }
        }

        [Flags]
        enum MODKEY
        {
            ALT = 0x1,
            CONTROL = 0x2,
            SHIFT = 0x4,
            WIN = 0x8
        }

        const int WM_HOTKEY = 0x312;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_HOTKEY && (int)m.WParam == hotkey_id) {
                Debug.WriteLine($"hotkey pressed at {Cursor.Position}");
                _ = new CaptureForm(Cursor.Position);
            }
        }

        [DllImport("user32.dll")]
        extern static int RegisterHotKey(IntPtr hWnd, int id, MODKEY mod, Keys key);

        [DllImport("user32.dll")]
        extern static int UnregisterHotKey(IntPtr hWnd, int id);


        //string SnapPath;


        //readonly List<ImageRec> ImageList = new List<ImageRec>();

        private void importItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine($"menu; import");

            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                var f = new ImageForm();
                f.LoadImage(openFileDialog1.FileName);
            }
        }

        private void SettingsItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine($"menu; settings");

            this.Show();
        }

        private void QuitItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine($"menu; quit");

            Application.Exit();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            //...

            //Properties.Settings.Default.Save();
        }

    }
}
