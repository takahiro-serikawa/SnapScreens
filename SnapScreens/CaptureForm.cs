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

            this.ID = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            Debug.WriteLine($"capture new {ID}");

            var bounds = Screen.GetBounds(location);
            //rec.ScreenRect = new xRectangle(bounds);
            Debug.WriteLine($" screen bounds = {bounds}");
            var captured = new Bitmap(bounds.Width, bounds.Height);
            using var g = Graphics.FromImage(captured);
            g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);

            image = captured;
            pic1.Size = captured.Size;
            pic1.Invalidate();

            otherRects = EnumWin.EnumWindowsOnScreen();

            this.Location = bounds.Location;
            this.TopMost = true;
            this.Show();
            this.TopMost = false;
            this.WindowState = FormWindowState.Maximized;
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
            SelRect = this.RectangleToClient(otherRects[otherIndex]);
        }


        bool isSelecting = false;
        Point p1 = Point.Empty;
        Point p2 = Point.Empty;
        Rectangle SelRect
        {
            get {
                return new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Abs(p2.X-p1.X), Math.Abs(p2.Y-p1.Y));
            }
            set {
                p1.X = value.X;
                p1.Y = value.Y;
                p2.X = value.X + value.Width;
                p2.Y = value.Y + value.Height;
                pic1.Invalidate();
            }
        }

        Rectangle ImageBounds => new(Point.Empty, image.Size);

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

        readonly string ID;
        Bitmap image;

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

        private void CaptureForm_KeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"capture KeyUp({e.Modifiers}, {e.KeyCode})");

            switch (e.KeyCode) {
            case Keys.Escape:
                this.Close();
                break;

            case Keys.Enter:
                if (p1.X == p2.X || p1.Y == p2.Y)
                    SelRect = ImageBounds;

                _ = new ImageForm(this.PointToScreen(SelRect.Location), GetCropped(SelRect));

                this.Close();
                break;

            case Keys.Tab:
                SelectNextRect();
                break;

            case Keys.S:
                // CTRL-S: export to image file
                //saveFileDialog1.FileName = _filename;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                    image.Save(saveFileDialog1.FileName);
                    //_filename = saveFileDialog1.FileName;
                }
                break;

            case Keys.A:
                // CTRL-A: select all
                SelRect = ImageBounds;
                break;

            case Keys.Left:
            case Keys.Right:
            case Keys.Up:
            case Keys.Down:
                // finish moving by cursor key
                if (p1.X != p2.X && p1.Y != p2.Y)
                    Clipboard.SetImage(GetCropped(SelRect));
                break;

            case Keys.L:    // for debug
                pic1.Invalidate();
                break;

            case Keys.M:    // for debug
                if (this.WindowState == FormWindowState.Maximized)
                    this.WindowState = FormWindowState.Normal;
                else
                    this.WindowState = FormWindowState.Maximized;
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
