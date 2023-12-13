namespace SnapCS
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            // カーソルのある方のスクリーンを画像としてキャプチャ
            var bounds = Screen.GetBounds(Cursor.Position);
            var captured = new Bitmap(bounds.Width, bounds.Height);
            using var g = Graphics.FromImage(captured);
            g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);

            // 最前面で最大化表示
            this.Text = "";
            this.ControlBox = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = bounds.Location;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.Show();
            this.TopMost = false;

            // なにかキーが押されるととりあえず閉じる
            this.KeyPress += (sender, e) => this.Close();

            // 画像表示、マウスによる矩形選択
            var isSelecting = false;
            var p1 = Point.Empty;
            var selRect = Rectangle.Empty;
            var picturebox1 = new PictureBox {
                Parent = this,
                Size = captured.Size,
                Image = captured,
                Cursor = Cursors.Cross
            };

            picturebox1.MouseDown += (sender, e) => {
                // 選択はじめ
                if (e.Button == MouseButtons.Left) {
                    isSelecting = true;
                    p1 = e.Location;
                }
            };

            picturebox1.MouseMove += (sender, e) => {
                // 選択中
                if (isSelecting) {
                    var p2 = e.Location;
                    selRect = new Rectangle(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y),
                                            Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
                    picturebox1.Invalidate();
                }
            };

            picturebox1.MouseUp += (sender, e) => {
                // 選択おわり
                if (e.Button == MouseButtons.Left) {
                    isSelecting = false;

                    if (selRect.Width > 0 && selRect.Height > 0) {
                        // 画像を切り抜いてクリップボードへコピー
                        var cropped = new Bitmap(selRect.Width, selRect.Height);
                        using var g = Graphics.FromImage(cropped);
                        g.DrawImage(picturebox1.Image, new Rectangle(0, 0, cropped.Width, cropped.Height), selRect, GraphicsUnit.Pixel);
                        Clipboard.SetImage(cropped);
                    }
                }
            };

            picturebox1.Paint += (sender, e) => {
                // 選択中の矩形を描画
                using var pen1 = new Pen(Color.Red, 1);
                e.Graphics.DrawRectangle(pen1, selRect);
            };

        }
    }
}
