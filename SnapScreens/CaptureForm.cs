// screen capture form
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace SnapScreens
{
    public partial class CaptureForm : Form
    {
        public CaptureForm(Point location)
        {
            InitializeComponent();

            //this.ID = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            Debug.WriteLine($"capture new {this.timestamp}");

            ScreenBounds = Screen.GetBounds(location);
            Debug.WriteLine($" screen bounds = {ScreenBounds}");
            var captured = new Bitmap(ScreenBounds.Width, ScreenBounds.Height);
            using var g = Graphics.FromImage(captured);
            g.CopyFromScreen(ScreenBounds.Location, Point.Empty, ScreenBounds.Size);

            image = captured;
            pic1.Size = captured.Size;
            pic1.Invalidate();

            otherRects = EnumWin.EnumWindowsOnScreen();

            this.Location = ScreenBounds.Location;
            this.TopMost = true;
            this.Show();
            //this.Activate();
            this.TopMost = false;
            //this.WindowState = FormWindowState.Maximized;
        }

        private void CaptureForm_Resize(object sender, EventArgs e)
        {
            Debug.WriteLine($"capture Resize({this.WindowState}, {this.Size})");
        }

        private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Debug.WriteLine($"capture FormClosed({e.CloseReason})");
        }


        readonly List<Rectangle> otherRects;
        int otherIndex = 0;

        void SelectNextRect()
        {
            otherIndex = (otherIndex+1) % otherRects.Count;
            var rect = this.RectangleToClient(otherRects[otherIndex]);
            p1 = rect.Location;
            p2 = rect.Location + rect.Size;
            pic1.Invalidate();
        }


        Rectangle ScreenBounds;
        bool isSelecting = false;
        Point p1 = Point.Empty;
        Point p2 = Point.Empty;
        Rectangle SelRect => new(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Abs(p2.X-p1.X), Math.Abs(p2.Y-p1.Y));

        private void pic1_MouseDown(object sender, MouseEventArgs e)
        {
            Debug.WriteLine($"capture MouseDown({e.Button}, {e.Location})");

            if (e.Button == MouseButtons.Left) {
                isSelecting = true;
                p1 = e.Location;
            }
        }

        private void pic1_MouseMove(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine($"capture MouseMove({e.Button}, {e.Location})");

            if (isSelecting) {
                p2 = e.Location;
                pic1.Invalidate();
            }
        }

        private void pic1_MouseUp(object sender, MouseEventArgs e)
        {
            Debug.WriteLine($"capture MouseUp({e.Button}, {e.Location})");

            if (e.Button == MouseButtons.Left) {
                isSelecting = false;
                if (p1.X != p2.X && p1.Y != p2.Y)
                    Clipboard.SetImage(GetCropped(SelRect));
            }
        }

        //readonly string ID;
        Bitmap image;
        readonly DateTime timestamp = DateTime.Now;

        private void pic1_Paint(object sender, PaintEventArgs e)
        {
            // draw faded screen capture
            var ia = new ImageAttributes();
            ia.SetColorMatrix(new ColorMatrix { Matrix00 = 0.3f, Matrix11 = 0.3f, Matrix22 = 0.3f });
            e.Graphics.DrawImage(image, new Rectangle(0, 0, pic1.Width, pic1.Height),
                0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);

            e.Graphics.DrawImage(image, SelRect,
                SelRect.X, SelRect.Y, SelRect.Width, SelRect.Height, GraphicsUnit.Pixel);

            using var pen0 = new Pen(Color.Brown, 1);
            foreach (var r in otherRects)
                e.Graphics.DrawRectangle(pen0, this.RectangleToClient(r));

            // draw select rectangle
            using var pen1 = new Pen(Color.White, 1);
            e.Graphics.DrawRectangle(pen1, SelRect.X, SelRect.Y, SelRect.Width-1, SelRect.Height-1);
            pen1.Color = Color.Red;
            pen1.DashStyle = DashStyle.Dash;
            e.Graphics.DrawRectangle(pen1, SelRect.X, SelRect.Y, SelRect.Width-1, SelRect.Height-1);

            //string s1 = $"{SelRect.Left}, {SelRect.Top}";
            //e.Graphics.DrawString(s1, pic1.Font, Brushes.White, SelRect.Left+2+1, SelRect.Top+2+1);
            //e.Graphics.DrawString(s1, pic1.Font, Brushes.Red, SelRect.Left+2, SelRect.Top+2);
            //string s2 = $"{SelRect.Right}, {SelRect.Bottom}";
            //var m2 = e.Graphics.MeasureString(s2, pic1.Font);
            //e.Graphics.DrawString(s2, pic1.Font, Brushes.White, SelRect.Right-m2.Width-2+1, SelRect.Bottom-m2.Height-2+1);
            //e.Graphics.DrawString(s2, pic1.Font, Brushes.Red, SelRect.Right-m2.Width-2, SelRect.Bottom-m2.Height-2);

            // draw coordinates
            //using var br = new SolidBrush(Color.Red);
            //var s1 = this.PointToScreen(p1);
            //e.Graphics.DrawString($"{s1}", this.Font, br, s1.X+2, s1.Y+2);
            //var s2 = this.PointToScreen(p2);
            //var m = e.Graphics.MeasureString($"{s2}", this.Font);
            //e.Graphics.DrawString($"{s2.X},{s2.Y}", this.Font, br, s2.X-m.Width-2, s2.Y-m.Height-2);
        }

        private void CaptureForm_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"capture KeyDown({e.Modifiers}, {e.KeyCode})");

            switch ((e.KeyCode, e.Modifiers)) {
            case (Keys.Left, Keys.Control):
                p1.X--;
                break;
            case (Keys.Left, Keys.Shift):
                p2.X--;
                break;
            case (Keys.Left, 0):
                p1.X--;
                p2.X--;
                break;

            case (Keys.Right, Keys.Control):
                p1.X++;
                break;
            case (Keys.Right, Keys.Shift):
                p2.X++;
                break;
            case (Keys.Right, 0):
                p1.X++;
                p2.X++;
                break;

            case (Keys.Up, Keys.Control):
                p1.Y--;
                break;
            case (Keys.Up, Keys.Shift):
                p2.Y--;
                break;
            case (Keys.Up, 0):
                p1.Y--;
                p2.Y--;
                break;

            case (Keys.Down, Keys.Control):
                p1.Y++;
                break;
            case (Keys.Down, Keys.Shift):
                p2.Y++;
                break;
            case (Keys.Down, 0):
                p1.Y++;
                p2.Y++;
                break;

            default:
                return;
            }
            pic1.Invalidate();
        }

        void SelectAll()
        {
            //SelRect = ImageBounds;
            p1 = Point.Empty;
            p2 = Point.Empty + image.Size;
            pic1.Invalidate();
        }

        private void CaptureForm_KeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"capture KeyUp({e.Modifiers}, {e.KeyCode})");

            switch ((e.KeyCode, e.Modifiers)) {
            case (Keys.Escape, Keys.None):
                this.Close();
                break;

            case (Keys.Enter, Keys.None):
            case (Keys.Enter, Keys.Shift):
                if (p1.X == p2.X || p1.Y == p2.Y)
                    SelectAll();

                _=new ImageForm(ScreenBounds, SelRect, image);

                if (!e.Shift)
                    this.Close();
                break;

            case (Keys.Tab, Keys.None):
                SelectNextRect();
                break;

            case (Keys.S, Keys.Control):
                // CTRL-S: export to image file
                //saveFileDialog1.FileName = _filename;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                    image.Save(saveFileDialog1.FileName);
                    //_filename = saveFileDialog1.FileName;
                }
                break;

            case (Keys.A, Keys.Control):
                // CTRL-A: select all
                SelectAll();
                break;

            case (Keys.L, Keys.Control):    // for debug
                pic1.Invalidate();
                break;

            case (Keys.M, Keys.Control):    // for debug
                if (this.WindowState == FormWindowState.Maximized)
                    this.WindowState = FormWindowState.Normal;
                else
                    this.WindowState = FormWindowState.Maximized;
                break;

            default:
                switch (e.KeyCode) {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    // finish moving by cursor key
                    if (p1.X != p2.X && p1.Y != p2.Y)
                        Clipboard.SetImage(GetCropped(SelRect));
                    break;
                }
                break;
            }

        }

        Bitmap GetCropped(Rectangle selRect)
        {
            Debug.WriteLine($" crop {selRect}");

            var cropped = new Bitmap(selRect.Width, selRect.Height);
            using var g = Graphics.FromImage(cropped);
            g.DrawImage(image, new Rectangle(Point.Empty, cropped.Size), selRect, GraphicsUnit.Pixel);

            return cropped;
        }

    }
}
