// image display form
using System.Diagnostics;
using System.Xml.Serialization;

namespace SnapScreens
{
    public partial class ImageForm : Form
    {
        public ImageForm()
        {
            InitializeComponent();

            instances.Add(this);
        }

        public ImageForm(Rectangle screenBounds, Rectangle selRect, Bitmap screenBitmap)
        {
            InitializeComponent();

            rec.ScreenRect = new xRect(screenBounds);
            rec.SelectRect = new xRect(selRect);
            instances.Add(this);

            pic1.Image = screenBitmap;
            pic1.Size = screenBitmap.Size;

            var rectOnScreen = selRect;
            rectOnScreen.Offset(screenBounds.Location);
            this.Location = rectOnScreen.Location;
            this.ClientSize = rectOnScreen.Size;
            this.Show();

            if (this.AutoScroll)
                this.AutoScrollPosition = selRect.Location;
            else
                pic1.Location = new Point(-selRect.X, -selRect.Y);
        }

        ~ImageForm()
        {
            instances.Remove(this);
        }

        private static List<ImageForm> instances = new List<ImageForm>();

        public void LoadImage(string filename)
        {
            rec.ID = "";
            //_filename = filename;
            Debug.WriteLine($"capture {rec.ID} new");

            //xx image = new Bitmap(filename);

            //this.Location = bounds.Location;
            //this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        // image list
        [XmlRoot("item")]
        public class ImageRec
        {
            [XmlAttribute("id")]
            public string ID;

            [XmlElement("bounds_rect")]
            public xRect ScreenRect;

            [XmlElement("select_rect")]
            public xRect SelectRect;

            //[XmlIgnore]
            //public Bitmap Bitmap;

            //public ImageRec(string ID) { this.ID = ID; }
        }

        readonly ImageRec rec = new();

        public static string SnapPath = "";

        void SaveRec()
        {
            var ser = new XmlSerializer(typeof(ImageRec));

            var id_xml = Path.Combine(SnapPath, rec.ID+".xml");
            using var wr = new StreamWriter(id_xml);
            ser.Serialize(wr, rec);

            var id_png = Path.Combine(SnapPath, rec.ID+".png");
            pic1.Image.Save(id_png);
        }

        private void pic1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ImageForm_Resize(object sender, EventArgs e)
        {
            Debug.WriteLine($"image {rec.ID} Resize({this.WindowState}, {this.Size}, ClientSize={this.ClientSize})");
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
            Debug.WriteLine($"image {rec.ID} KeyDown({e.Modifiers}, {e.KeyCode})");

        }

        private void ImageForm_KeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"image {rec.ID} KeyUp({e.Modifiers}, {e.KeyCode})");

            switch ((e.KeyCode, e.Modifiers)) {
            case (Keys.S, Keys.Control):
                //SnapMain.SaveImageAs(pic1.Image);
                break;

            case (Keys.W, Keys.Control):
                this.Close();
                break;

            case (Keys.C, Keys.Control):
                Debug.WriteLine($" copy ({pic1.Image.Size})");
                Clipboard.SetImage(pic1.Image);
                break;

            case (Keys.Tab, 0):
                ActivateNext(+1);
                break;

            case (Keys.Tab, Keys.Shift):
                ActivateNext(-1);
                break;

            case (Keys.B, Keys.Control):
                var lastSize = this.ClientSize;
                Debug.WriteLine($" ClientSize {lastSize}");
                this.AutoScroll = !this.AutoScroll;
                if (this.AutoScroll) {
                    pic1.Location = Point.Empty;
                    this.AutoScrollPosition = new Point(rec.SelectRect.Left, rec.SelectRect.Top);
                } else {
                    pic1.Location = new Point(-rec.SelectRect.Left, -rec.SelectRect.Top);
                }
                this.ClientSize = lastSize;
                break;

            }
        }

        void ActivateNext(int d)
        {
            int index = instances.IndexOf(this);
            index = (index+d) % instances.Count;
            instances[index].Activate();

            Debug.WriteLine($" activate #{index}: {instances[index].rec.ID}");
        }

        private void ImageForm_Scroll(object sender, ScrollEventArgs e)
        {
            Debug.WriteLine($"image {rec.ID} Scroll({e.Type}, {e.ScrollOrientation}, {e.OldValue}, {e.NewValue})");
        }

    }
}
