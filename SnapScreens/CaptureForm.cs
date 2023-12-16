// SnapScreens capture image form
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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

            var bounds = Screen.GetBounds(location);
            Debug.WriteLine($" screen bounds = {bounds}");
            var captured = new Bitmap(bounds.Width, bounds.Height);
            using (var g = Graphics.FromImage(captured)) {
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            }

            Image = captured;

            //this.Location = bounds.Location;
            //this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.Show();
            this.TopMost = false;
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
        Rectangle selRect = Rectangle.Empty;

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
                selRect = new Rectangle(Math.Min(p1.X, e.X), Math.Min(p1.Y, e.Y),
                    Math.Abs(p1.X-e.X), Math.Abs(p1.Y-e.Y));

                pic1.Invalidate();
            }
        }

        private void pic1_MouseUp(object sender, MouseEventArgs e)
        {
            Debug.WriteLine($"capture {Caption} MouseUp({e.Button}, {e.Location})");

            if (e.Button == MouseButtons.Left) {
                isSelecting = false;
                if (selRect.Width > 0 && selRect.Height > 0)
                    Clipboard.SetImage(GetCropped(selRect));
            }
        }

        private void pic1_Paint(object sender, PaintEventArgs e)
        {
            using (var pen1 = new Pen(Color.Red, 1)) {
                var ia = new ImageAttributes();
                ia.SetColorMatrix(new ColorMatrix { Matrix00 = 0.5f, Matrix11 = 0.5f, Matrix22 = 0.5f });
                e.Graphics.DrawImage(Image, pic1.Bounds,
                    0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, ia);

                // draw selecting rectangle
                e.Graphics.DrawImage(Image, selRect,
                    selRect.X, selRect.Y, selRect.Width, selRect.Height, GraphicsUnit.Pixel);

                e.Graphics.DrawRectangle(pen1, selRect);
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
                    // export to image file
                    saveFileDialog1.FileName = _filename;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                        Image.Save(saveFileDialog1.FileName);
                        _filename = saveFileDialog1.FileName;
                    }
                }
                break;

            case Keys.A:
                if (e.Control) {
                    // select all
                    selRect = new Rectangle(0, 0, pic1.Width, pic1.Height);
                    pic1.Invalidate();
                }
                break;

            case Keys.Left:
            case Keys.Right:
            case Keys.Up:
            case Keys.Down:
                // finish moving by cursor key
                if (selRect.Width > 0 && selRect.Height > 0)
                    Clipboard.SetImage(GetCropped(selRect));
                break;

            }
        }

        private void MoveSelRect(int dx, int dy, int dw, int dh)
        {
            selRect.X += dx;
            if (selRect.X > pic1.Width - selRect.Width)
                selRect.X = pic1.Width - selRect.Width;
            if (selRect.X < 0)
                selRect.X = 0;

            selRect.Y += dy;
            if (selRect.Y > pic1.Height - selRect.Height)
                selRect.Y = pic1.Height - selRect.Height;
            if (selRect.Y < 0)
                selRect.Y = 0;

            pic1.Invalidate();
            Debug.WriteLine($" selRect -> {selRect}");
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
            if (selRect.Width > 0 && selRect.Height > 0) {
                Image = GetCropped(selRect);
                this.ClientSize = Image.Size;
                this.Location = selRect.Location;
                this.WindowState = FormWindowState.Normal;

                // select all
                selRect.Location = Point.Empty;
            }
        }

    }
}
