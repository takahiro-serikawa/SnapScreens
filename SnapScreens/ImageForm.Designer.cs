namespace SnapScreens
{
    partial class ImageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pic1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pic1).BeginInit();
            SuspendLayout();
            // 
            // pic1
            // 
            pic1.Cursor = Cursors.SizeAll;
            pic1.Location = new Point(0, 0);
            pic1.Margin = new Padding(4, 5, 4, 5);
            pic1.Name = "pic1";
            pic1.Size = new Size(315, 435);
            pic1.TabIndex = 1;
            pic1.TabStop = false;
            pic1.Paint += pic1_Paint;
            // 
            // ImageForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.Gray;
            ClientSize = new Size(640, 480);
            Controls.Add(pic1);
            KeyPreview = true;
            Name = "ImageForm";
            StartPosition = FormStartPosition.Manual;
            Text = "ImageForm";
            FormClosing += ImageForm_FormClosing;
            FormClosed += ImageForm_FormClosed;
            Scroll += ImageForm_Scroll;
            KeyDown += ImageForm_KeyDown;
            KeyUp += ImageForm_KeyUp;
            Resize += ImageForm_Resize;
            ((System.ComponentModel.ISupportInitialize)pic1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pic1;
    }
}