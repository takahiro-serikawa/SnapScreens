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

        public ImageForm(Point location, Bitmap bitmap)
        {
            InitializeComponent();

            instances.Add(this);

            //rec = new ImageRec(id);
            //this.Text = id;
            image = bitmap;
            this.Location = location;
            this.ClientSize = bitmap.Size;
            this.Show();
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

            image = new Bitmap(filename);

            //this.Location = bounds.Location;
            //this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        readonly ImageRec rec = new ImageRec();

        Bitmap image
        {
            set {
                pic1.Image = value;
                pic1.Size = value.Size;
                pic1.Invalidate();
            }
           // get { return pic1.Image; }
        }

        //public struct xRectangle
        //{
        //    public int x, y, width, height;
        //}

        // image list
        [XmlRoot("item")]
        public class ImageRec
        {
            [XmlAttribute("id")]
            public string ID;

            [XmlElement("bounds_rect")]
            //public xRectangle ScreenRect;
            public RECT ScreenRect;

            [XmlElement("select_rect")]
            //public xRectangle SelectRect;
            public RECT SelectRect;

            //[XmlIgnore]
            //public Bitmap Bitmap;

            //public ImageRec(string ID) { this.ID = ID; }
        }

        public static string SnapPath = "";


        void SaveRec()
        {
            var ser = new XmlSerializer(typeof(ImageRec));

            var id_xml = Path.Combine(SnapPath, rec.ID+".xml");
            using (var wr = new StreamWriter(id_xml)) {
                ser.Serialize(wr, rec);
            }

            var id_png = Path.Combine(SnapPath, rec.ID+".png");
            pic1.Image.Save(id_png);
        }

        //string _filename = "";

        //string Caption
        //{
        //    get { return rec.ID; }
        //}

        private void pic1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ImageForm_Resize(object sender, EventArgs e)
        {
            Debug.WriteLine($"image {rec.ID} Resize({this.WindowState}, {this.Size})");
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
                ActivateNext();
                break;
            }
        }

        void ActivateNext()
        {
            int index = instances.IndexOf(this);
            index = (index+1) % instances.Count;
            instances[index].Activate();

            Debug.WriteLine($" activate {instances[index].rec.ID}");
        }

    }
}
