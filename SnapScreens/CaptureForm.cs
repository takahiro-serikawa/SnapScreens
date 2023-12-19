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

namespace SnapScreens
{
    public partial class CaptureForm : Form
    {
        public CaptureForm(Point location)
        {
            InitializeComponent();

            _filename = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            Debug.WriteLine($"capture {Caption} new");

            ScreenRect = Screen.GetBounds(location);
            Debug.WriteLine($" screen bounds = {ScreenRect}");
            var captured = new Bitmap(ScreenRect.Width, ScreenRect.Height);
            using (var g = Graphics.FromImage(captured)) {
                g.CopyFromScreen(ScreenRect.Location, Point.Empty, ScreenRect.Size);
            }

            Image = captured;

            this.Location = ScreenRect.Location;
            //this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.Show();
            this.TopMost = false;
            this.BringToFront();
            this.Activate();
        }

        Image _image;

        Image Image
        {
            set {
                _image = value;
                pic1.Size = _image.Size;
                pic1.Invalidate();
            }
            get { return _image; }
        }
         
        public CaptureForm(string filename)
        {
            InitializeComponent();

            _filename = filename;
            Debug.WriteLine($"capture {Caption} new");

            var bitmap = new Bitmap(filename);
            Debug.WriteLine($" bitmap size = {bitmap.Size}");

            Image = bitmap;

            //this.Location = bounds.Location;
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        string _filename = "";

        string Caption
        {
            get { return System.IO.Path.GetFileName(_filename); }
            //set { _filename = value; }
        }

        private void CaptureForm_Resize(object sender, EventArgs e)
        {
            Debug.WriteLine($"capture {Caption} Resize({this.WindowState}, {this.Size})");

            if (this.WindowState == FormWindowState.Maximized) {
                this.Text = "";
                this.ControlBox = false;
            } else {
                this.Text = Caption;
                this.ControlBox = true;
            }
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
        Rectangle SelRect {
            get { return new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Abs(p2.X-p1.X), Math.Abs(p2.Y-p1.Y)); }
        }
        Rectangle ScreenRect = Rectangle.Empty;

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
            if (p1.X == p2.X && p1.Y == p2.Y) {
                e.Graphics.DrawImage(Image, new Rectangle(0, 0, pic1.Width, pic1.Height),
                    0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel);
            } else {
                var ia = new ImageAttributes();
                ia.SetColorMatrix(new ColorMatrix { Matrix00 = 0.3f, Matrix11 = 0.3f, Matrix22 = 0.3f });
                e.Graphics.DrawImage(Image, new Rectangle(0, 0, pic1.Width, pic1.Height),
                    0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, ia);

                // draw selecting rectangle
                e.Graphics.DrawImage(Image, SelRect,
                    SelRect.X, SelRect.Y, SelRect.Width, SelRect.Height, GraphicsUnit.Pixel);

                using (var pen1 = new Pen(Color.Red, 1))
                using (var pen2 = new Pen(Color.Black, 1)) {
                    pen2.DashStyle = DashStyle.Dash;
                    e.Graphics.DrawRectangle(pen1, SelRect.X, SelRect.Y, SelRect.Width-1, SelRect.Height-1);
                    e.Graphics.DrawRectangle(pen2, SelRect.X, SelRect.Y, SelRect.Width-1, SelRect.Height-1);
                    //e.Graphics.DrawLine(pen1, 0, p1.Y, pic1.Width, p1.Y);
                    //e.Graphics.DrawLine(pen1, p1.X, 0, p1.X, pic1.Height);
                    //e.Graphics.DrawLine(pen1, 0, p2.Y, pic1.Width, p2.Y);
                    //e.Graphics.DrawLine(pen1, p2.X, 0, p2.X, pic1.Height);
                }
            }
        }

        private void CaptureForm_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"capture {Caption} KeyDown({e.Modifiers}, {e.KeyCode})");

            switch (e.KeyCode) {
            case Keys.Left:
                if (e.Shift)
                    MoveSelRect(0, 0, -1, 0);
                else
                    MoveSelRect(-1, 0, 0, 0);
                break;

            case Keys.Right:
                if (e.Shift)
                    MoveSelRect(0, 0, +1, 0);
                else
                    MoveSelRect(+1, 0, 0, 0);
                break;

            case Keys.Up:
                if (e.Shift)
                    MoveSelRect(0, 0, 0, -1);
                else
                    MoveSelRect(0, -1, 0, 0);
                break;

            case Keys.Down:
                if (e.Shift)
                    MoveSelRect(0, 0, 0, +1);
                else
                    MoveSelRect(0, +1, 0, 0);
                break;

            }
        }

        private void CaptureForm_KeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"capture {Caption} KeyUp({e.Modifiers}, {e.KeyCode})");

            switch (e.KeyCode) {
            case Keys.Escape:
                this.Close();
                break;

            case Keys.Enter:
                CropForm();
                break;

            case Keys.S:
                if (e.Control) {
                    // CTRL-S: export to image file
                    saveFileDialog1.FileName = _filename;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                        Image.Save(saveFileDialog1.FileName);
                        _filename = saveFileDialog1.FileName;
                    }
                }
                break;

            case Keys.A:
                if (e.Control) {
                    // CTRL-A: select all
                    SelectAll();
                }
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
                if (e.Control) {
                    pic1.Invalidate();
                }
                break;

            case Keys.M:    // for debug
                if (e.Control) {
                    if (this.WindowState == FormWindowState.Maximized)
                        this.WindowState = FormWindowState.Normal;
                    else
                        this.WindowState = FormWindowState.Maximized;
                }
                break;

            }
        }

        private void MoveSelRect(int dx, int dy, int dw, int dh)
        {
            p1.X += dx;
            p2.X += dx;
            p1.Y += dy;
            p2.Y += dy;

            // 移動制限

            // サイズ変更

            pic1.Invalidate();
            Debug.WriteLine($" selRect -> {SelRect}");
        }

        Bitmap GetCropped(Rectangle selRect)
        {
            Debug.WriteLine($" crop {selRect}");

            var cropped = new Bitmap(selRect.Width, selRect.Height);
            using (var g = Graphics.FromImage(cropped)) {
                g.DrawImage(Image,
                    //new Rectangle(0, 0, cropped.Width, cropped.Height),
                    new Rectangle(Point.Empty, cropped.Size),
                    selRect, GraphicsUnit.Pixel);
            }

            return cropped;
        }

        void CropForm()
        {
            if (p1.X != p2.X && p1.Y != p2.Y) {
                Image = GetCropped(SelRect);
                this.ClientSize = Image.Size;
                this.WindowState = FormWindowState.Normal;
                this.Location = new Point(ScreenRect.X + SelRect.X, ScreenRect.Y + SelRect.Y);
                this.ClientSize = SelRect.Size;

                SelectAll();
            }
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
