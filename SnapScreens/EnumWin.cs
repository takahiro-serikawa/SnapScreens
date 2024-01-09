using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapScreens
{
    internal class EnumWin
    {
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;

        public const int WS_VISIBLE = 0x10000000;
        public const int WS_BORDER = 0x00800000;
        public const int WS_EX_TOOLWINDOW = 0x00000080;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public Int32 Left;
            public Int32 Top;
            public Int32 Right;
            public Int32 Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd,
                StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public static List<Rectangle> EnumWindowsOnScreen()
        {
            var bounds = new Dictionary<Rectangle, int>();
            EnumWindows((hWnd, lParam) => {
                int textLen = GetWindowTextLength(hWnd);
                var tsb = new StringBuilder(textLen + 1);
                var csb = new StringBuilder(256);
                if (0 < textLen) {
                    GetWindowText(hWnd, tsb, tsb.Capacity);
                    GetClassName(hWnd, csb, csb.Capacity);
                } else 
                    return true;

                //uint processId;
                //GetWindowThreadProcessId(hWnd, out processId);

                //Process process = Process.GetProcessById((int)processId);
                //string processName = process.ProcessName;

                //if (Screen.FromHandle(hWnd).DeviceName == Screen.AllScreens[screenIndex].DeviceName) {
                int style = GetWindowLong(hWnd, GWL_STYLE);
                int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
                                if ((style & WS_VISIBLE) != 0 && (exStyle & WS_EX_TOOLWINDOW) == 0) {
                    RECT rect;// = new RECT() { Left=0, Top=0, Right=0, Bottom=0 };
                    if (GetWindowRect(hWnd, out rect)) {
                        var r = new Rectangle(rect.Left, rect.Top, rect.Right-rect.Left, rect.Bottom-rect.Top);
                        bounds[r] = 1;
                        //Debug.WriteLine($"text={tsb}, class={csb},\r\n {{{rect.Left},{rect.Top}, {rect.Right-rect.Left},{rect.Bottom-rect.Top}}}");
                        //Debug.WriteLine($"process={processName}, B {style & WS_BORDER}, T {exStyle & WS_EX_TOOLWINDOW}");
                    }
                }
                //}

                return true;
            }, IntPtr.Zero);

            return bounds.Keys.ToList();
        }
    }
}
