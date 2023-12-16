namespace SnapScreens
{
    partial class CaptureForm
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
            this.pic1 = new System.Windows.Forms.PictureBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).BeginInit();
            this.SuspendLayout();
            // 
            // pic1
            // 
            this.pic1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pic1.Location = new System.Drawing.Point(0, 0);
            this.pic1.Margin = new System.Windows.Forms.Padding(2);
            this.pic1.Name = "pic1";
            this.pic1.Size = new System.Drawing.Size(236, 209);
            this.pic1.TabIndex = 0;
            this.pic1.TabStop = false;
            this.pic1.Paint += new System.Windows.Forms.PaintEventHandler(this.pic1_Paint);
            this.pic1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pic1_MouseDown);
            this.pic1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pic1_MouseMove);
            this.pic1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pic1_MouseUp);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "png";
            // 
            // CaptureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(400, 234);
            this.ControlBox = false;
            this.Controls.Add(this.pic1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CaptureForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CaptureForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptureForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CaptureForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CaptureForm_KeyUp);
            this.Resize += new System.EventHandler(this.CaptureForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}