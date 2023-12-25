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
            pic1 = new PictureBox();
            saveFileDialog1 = new SaveFileDialog();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pic1).BeginInit();
            SuspendLayout();
            // 
            // pic1
            // 
            pic1.Cursor = Cursors.Cross;
            pic1.Location = new Point(0, 0);
            pic1.Margin = new Padding(4, 5, 4, 5);
            pic1.Name = "pic1";
            pic1.Size = new Size(511, 515);
            pic1.TabIndex = 0;
            pic1.TabStop = false;
            pic1.Paint += pic1_Paint;
            pic1.MouseDown += pic1_MouseDown;
            pic1.MouseMove += pic1_MouseMove;
            pic1.MouseUp += pic1_MouseUp;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "png";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(778, 29);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(0, 32);
            label2.TabIndex = 2;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(758, 520);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(0, 32);
            label3.TabIndex = 3;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(30, 520);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(0, 32);
            label4.TabIndex = 4;
            // 
            // CaptureForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.Gray;
            ClientSize = new Size(867, 576);
            ControlBox = false;
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(pic1);
            KeyPreview = true;
            Margin = new Padding(4, 5, 4, 5);
            Name = "CaptureForm";
            StartPosition = FormStartPosition.Manual;
            Activated += CaptureForm_Activated;
            Deactivate += CaptureForm_Deactivate;
            FormClosing += CaptureForm_FormClosing;
            FormClosed += CaptureForm_FormClosed;
            KeyDown += CaptureForm_KeyDown;
            KeyUp += CaptureForm_KeyUp;
            Resize += CaptureForm_Resize;
            ((System.ComponentModel.ISupportInitialize)pic1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox pic1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}