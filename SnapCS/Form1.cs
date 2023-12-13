using System.Runtime.InteropServices;

namespace SnapCS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Application.ThreadException += (sender, e) =>
                MessageBox.Show(e.Exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            this.FormClosed += (sender, e) => UnregisterHotKey(this.Handle, hotkey_id);

            // ホットキーを変えるときはここ
            RegisterHotKey(this.Handle, hotkey_id, MODKEY.CONTROL | MODKEY.ALT, Keys.F2);
        }

        [Flags]
        enum MODKEY
        {
            ALT = 0x1,
            CONTROL = 0x2,
            SHIFT = 0x4
        }

        int hotkey_id = 1;

        const int WM_HOTKEY = 0x312;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_HOTKEY && (int)m.WParam == hotkey_id)
                new Form2();
        }

        [DllImport("user32.dll")]
        extern static int RegisterHotKey(IntPtr hWnd, int id, MODKEY mod, Keys key);

        [DllImport("user32.dll")]
        extern static int UnregisterHotKey(IntPtr hWnd, int id);
    }
}
