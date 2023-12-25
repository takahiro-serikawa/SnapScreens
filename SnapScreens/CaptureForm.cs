// SnapScreens capture image form
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace SnapScreens
{
    public partial class CaptureForm : Form
    {
        public CaptureForm(Point location)
        {
            InitializeComponent();

            Caption = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            Debug.WriteLine($"capture {Caption} new");

            var bounds = Screen.GetBounds(location);
            //rec.ScreenRect = new xRectangle(bounds);
            Debug.WriteLine($" screen bounds = {bounds}");
            var captured = new Bitmap(bounds.Width, bounds.Height);
            using var g = Graphics.FromImage(captured);
            g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);

            Image = captured;

            otherRects = EnumWin.EnumWindowsOnScreen();
            //for (int i = 0; i < otherRects.Count; i++) {
            //    otherRects[i] = this.RectangleToClient(otherRects[i]);
            //}

            this.Location = bounds.Location;
            this.TopMost = true;
            this.Show();
            this.TopMost = false;
            this.BringToFront();
            this.Activate();
            this.WindowState = FormWindowState.Maximized;
        }

        List<Rectangle> otherRects = null;
        int otherIndex = 0;

        void SelectNextRect()
        {
            otherIndex = (otherIndex+1) % otherRects.Count;
            SelRect = this.RectangleToClient(otherRects[otherIndex]);
        }

        Bitmap _image = null;
        string Caption = "";

        Bitmap Image
        {
            set {
                _image = value;
                pic1.Size = value.Size;
                pic1.Invalidate();
            }
            get { return _image; }
        }

        private void CaptureForm_Resize(object sender, EventArgs e)
        {
            Debug.WriteLine($"capture {Caption} Resize({this.WindowState}, {this.Size})");

        }

        private void CaptureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine($"capture {Caption} FormClosing({e.CloseReason})");
        }

        private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Debug.WriteLine($"capture {Caption} FormClosed({e.CloseReason})");
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
                p2.X = value.X+value.Width;
                p2.Y = value.Y+value.Height;
                pic1.Invalidate();
            }
        }

        private void MoveSelRect(int dx, int dy, int dw, int dh)
        {
            p1.X += dx;
            p2.X += dx;
            p1.Y += dy;
            p2.Y += dy;
            if (p1.X > p2.X) p1.X += dw; else p2.X += dw;
            if (p1.Y > p2.Y) p1.Y += dh; else p2.Y += dh;

            // 移動制限

            // サイズ変更

            pic1.Invalidate();
            Debug.WriteLine($" selRect -> {SelRect}");
        }

        private void pic1_MouseDown(object sender, MouseEventArgs e)
        {
            Debug.WriteLine($"capture {Caption} MouseDown({e.Button}, {e.Location})");

            if (e.Button == MouseButtons.Left) {
                isSelecting = true;
                p1 = e.Location;
            }
        }

        private void pic1_MouseMove(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine($"capture({Caption}) MouseMove({e.Button}, {e.Location})");

            if (isSelecting) {
                p2 = e.Location;
                pic1.Invalidate();
            }
        }

        private void pic1_MouseUp(object sender, MouseEventArgs e)
        {
            Debug.WriteLine($"capture {Caption} MouseUp({e.Button}, {e.Location})");

            if (e.Button == MouseButtons.Left) {
                isSelecting = false;
                if (p1.X != p2.X && p1.Y != p2.Y)
                    Clipboard.SetImage(GetCropped(SelRect));
            }
        }

        private void pic1_Paint(object sender, PaintEventArgs e)
        {
            // draw faded screen capture
            var ia = new ImageAttributes();
            ia.SetColorMatrix(new ColorMatrix { Matrix00 = 0.3f, Matrix11 = 0.3f, Matrix22 = 0.3f });
            e.Graphics.DrawImage(Image, new Rectangle(0, 0, pic1.Width, pic1.Height),
                0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, ia);

            e.Graphics.DrawImage(Image, SelRect,
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
            //e.Graphics.DrawLine(pen1, 0, p1.Y, pic1.Width, p1.Y);
            //e.Graphics.DrawLine(pen1, p1.X, 0, p1.X, pic1.Height);
            //e.Graphics.DrawLine(pen1, 0, p2.Y, pic1.Width, p2.Y);
            //e.Graphics.DrawLine(pen1, p2.X, 0, p2.X, pic1.Height);
        }

        private void CaptureForm_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"capture {Caption} KeyDown({e.Modifiers}, {e.KeyCode})");

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
            Debug.WriteLine($"capture {Caption} KeyUp({e.Modifiers}, {e.KeyCode})");

            switch ((e.KeyCode, e.Modifiers)) {
            case (Keys.Escape, Keys.None):
                this.Close();
                break;

            case (Keys.Enter, Keys.None):
                CropForm();
                //rec.SelectRect = new xRectangle(SelRect);
                //this.SaveRec();
                break;

            case (Keys.Tab, Keys.None):
                SelectNextRect();
                break;

            case (Keys.S, Keys.Control):
                // CTRL-S: export to image file
                //saveFileDialog1.FileName = _filename;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                    Image.Save(saveFileDialog1.FileName);
                    //_filename = saveFileDialog1.FileName;
                }
                break;

            case (Keys.A, Keys.Control):
                // CTRL-A: select all
                SelectAll();
                break;

            case (Keys.Left, Keys.None):
            case (Keys.Left, Keys.Shift):
            case (Keys.Left, Keys.Control):
            case (Keys.Right, Keys.None):
            case (Keys.Right, Keys.Shift):
            case (Keys.Right, Keys.Control):
            case (Keys.Up, Keys.None):
            case (Keys.Up, Keys.Shift):
            case (Keys.Up, Keys.Control):
            case (Keys.Down, Keys.None):
            case (Keys.Down, Keys.Shift):
            case (Keys.Down, Keys.Control):
                // finish moving by cursor key
                if (p1.X != p2.X && p1.Y != p2.Y)
                    Clipboard.SetImage(GetCropped(SelRect));
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

            }
        }

        Bitmap GetCropped(Rectangle selRect)
        {
            Debug.WriteLine($" crop {selRect}");

            var cropped = new Bitmap(selRect.Width, selRect.Height);
            using var g = Graphics.FromImage(cropped);
            g.DrawImage(Image,
                new Rectangle(Point.Empty, cropped.Size),
                selRect, GraphicsUnit.Pixel);

            return cropped;
        }

        void CropForm()
        {
            if (p1.X == p2.X || p1.Y == p2.Y)
                SelectAll();

            var image = GetCropped(SelRect);
            var f = new ImageForm(this.PointToScreen(SelRect.Location), image);
            //f.ClientSize = Image.Size;
            //f.WindowState = FormWindowState.Normal;
            //f.Location = this.PointToScreen(SelRect.Location);
            //f.ClientSize = SelRect.Size;

            this.Close();
        }

        void SelectAll()
        {
            p1 = new Point(0, 0);
            p2 = new Point(pic1.Width, pic1.Height);
            pic1.Invalidate();
        }

        private void CaptureForm_Activated(object sender, EventArgs e)
        {
            Debug.WriteLine($"capture {Caption} Activated()");
        }

        private void CaptureForm_Deactivate(object sender, EventArgs e)
        {
            Debug.WriteLine($"capture {Caption} DeActivated()");
        }
    }
}
