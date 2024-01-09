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
            RegisterHotKey(hotkey, modkeys);

            ImageForm.SnapPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData,
                Environment.SpecialFolderOption.Create), "SnapScreens");
            Debug.WriteLine($" SnapPath={ImageForm.SnapPath}");
            //RestoreImages();

            // refresh app. settings
            HotKeyCombo.Items.Clear();
            var keys = Enum.GetValues(typeof(Keys)).Cast<Keys>()
                .Where(key => Keys.None < key && key < Keys.KeyCode)
                .Distinct();
            foreach (var k in keys) {
                HotKeyCombo.Items.Add(k);
            }
            HotKeyCombo.SelectedItem = hotkey;

            if (modkeys.HasFlag(MODKEY.ALT))
                Alt.Checked = true;
            if (modkeys.HasFlag(MODKEY.CONTROL))
                Control.Checked = true;
            if (modkeys.HasFlag(MODKEY.SHIFT))
                Shift.Checked = true;
        }

        private void SnapMain_Load(object sender, EventArgs e)
        {
        }

        ~SnapMain()
        {
            UnregisterHotKey(this.Handle, hotkey_id);
            Debug.WriteLine($"UnregisterHotKey({hotkey_id})");
        }

        private int hotkey_id = -1;

        void RegisterHotKey(Keys hotkey, MODKEY modkeys)
        {
            if (hotkey_id >= 0)
                UnregisterHotKey(this.Handle, hotkey_id);

            for (int i = 1; i < 100; i++)
                if (RegisterHotKey(this.Handle, i, modkeys, hotkey) !=0) {
                    hotkey_id = i;
                    Debug.WriteLine($" RegisterHotKey({hotkey_id}, [{modkeys}], {hotkey})");
                    break;
                }
        }


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
            NONE = 0x0,
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
            this.Activate();
        }

        private void QuitItem_Click(object sender, EventArgs e)
        {
            Debug.WriteLine($"menu; quit");

            Application.Exit();
        }

        // apply and save changed settings
        private void applyButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("applyButton_Click()");

            var hotkey = (Keys)HotKeyCombo.SelectedItem;
            Properties.Settings.Default.HotKey = hotkey.ToString();

            var modkeys = MODKEY.NONE;
            if (Alt.Checked)
                modkeys |= MODKEY.ALT;
            if (Control.Checked)
                modkeys |= MODKEY.CONTROL;
            if (Shift.Checked)
                modkeys |= MODKEY.SHIFT;
            Properties.Settings.Default.ModKeys = (int)modkeys;

            Properties.Settings.Default.Save();

            RegisterHotKey(hotkey, modkeys);
        }

        public void SaveImageAs(Image bitmap)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                bitmap.Save(saveFileDialog1.FileName);
            }
        }

    }

    public struct xRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public xRect(Rectangle rect)
        {
            this.Left = rect.Left;
            this.Top = rect.Top;
            this.Right = rect.Right;
            this.Bottom = rect.Bottom;
        }

    }
}
