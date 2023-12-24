using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static SnapScreens.EnumWin;

namespace SnapScreens
{
    public partial class ImageForm : Form
    {
        public ImageForm()
        {
            InitializeComponent();
        }

        public ImageForm(Point location, Bitmap bitmap)
        {
            InitializeComponent();

            Image = bitmap;
            this.Show();
            this.Location = location;
            this.ClientSize = bitmap.Size;
        }

        public void LoadImage(string filename)
        {
            rec.ID = "";
            //_filename = filename;
            Debug.WriteLine($"capture {Caption} new");

            var bitmap = new Bitmap(filename);
            Debug.WriteLine($" bitmap size = {bitmap.Size}");

            Image = bitmap;

            //this.Location = bounds.Location;
            //this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        Bitmap Image
        {
            set {
                pic1.Image = value;
                pic1.Size = value.Size;
                pic1.Invalidate();
            }
           // get { return pic1.Image; }
        }

        public struct xRectangle
        {
            public int x, y, width, height;
            public xRectangle(Rectangle r)
            {
                x = r.X;
                y = r.Y;
                width = r.Width;
                height = r.Height;
            }
        }

        // image list
        [XmlRoot("item")]
        public class ImageRec
        {
            [XmlAttribute("id")]
            public string ID;

            [XmlElement("bounds_rect")]
            public xRectangle ScreenRect;

            [XmlElement("select_rect")]
            public xRectangle SelectRect;

            //[XmlIgnore]
            //public Bitmap Bitmap;
        }

        public static string SnapPath = "";

        ImageRec rec = new ImageRec();

        void SaveRec()
        {
            XmlSerializer ser = new XmlSerializer(typeof(ImageRec));

            var id_xml = Path.Combine(SnapPath, rec.ID+".xml");
            using (var wr = new StreamWriter(id_xml)) {
                ser.Serialize(wr, rec);
            }

            var id_png = Path.Combine(SnapPath, rec.ID+".png");
            pic1.Image.Save(id_png);
        }

        //string _filename = "";

        string Caption
        {
            get { return rec.ID; }
        }

        private void pic1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ImageForm_Resize(object sender, EventArgs e)
        {
            Debug.WriteLine($"image {rec.ID} Resize({this.WindowState}, {this.Size})");
        }

        private void ImageForm_Activated(object sender, EventArgs e)
        {
            Debug.WriteLine($"image {rec.ID} Activate()");
        }

        private void ImageForm_Deactivate(object sender, EventArgs e)
        {
            Debug.WriteLine($"image {rec.ID} Deactivate()");
        }

        private void ImageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine($"image {rec.ID} FormClosing({e.CloseReason})");

        }

        private void ImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Debug.WriteLine($"image {rec.ID} FormClosed({e.CloseReason})");

        }

        private void ImageForm_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"image {Caption} KeyDown({e.Modifiers}, {e.KeyCode})");

        }

        private void ImageForm_KeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"image {Caption} KeyUp({e.Modifiers}, {e.KeyCode})");

        }

    }
}
