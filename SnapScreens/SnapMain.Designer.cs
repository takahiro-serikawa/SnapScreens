namespace SnapScreens
{
    partial class SnapMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SnapMain));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SettingsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.QuitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyButton = new System.Windows.Forms.Button();
            this.importItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "snap screens";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importItem,
            this.SettingsItem,
            this.toolStripMenuItem1,
            this.QuitItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(255, 124);
            // 
            // SettingsItem
            // 
            this.SettingsItem.Name = "SettingsItem";
            this.SettingsItem.Size = new System.Drawing.Size(300, 38);
            this.SettingsItem.Text = "Settings ...";
            this.SettingsItem.Click += new System.EventHandler(this.SettingsItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(297, 6);
            // 
            // QuitItem
            // 
            this.QuitItem.Name = "QuitItem";
            this.QuitItem.Size = new System.Drawing.Size(300, 38);
            this.QuitItem.Text = "Quit";
            this.QuitItem.Click += new System.EventHandler(this.QuitItem_Click);
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(606, 383);
            this.applyButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(150, 44);
            this.applyButton.TabIndex = 1;
            this.applyButton.Text = "apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // importItem
            // 
            this.importItem.Name = "importItem";
            this.importItem.Size = new System.Drawing.Size(300, 38);
            this.importItem.Text = "Import Image ...";
            this.importItem.Click += new System.EventHandler(this.importItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "png";
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SnapMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.applyButton);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SnapMain";
            this.ShowInTaskbar = false;
            this.Text = "SnapScreens";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SnapSettings_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem SettingsItem;
        private System.Windows.Forms.ToolStripMenuItem QuitItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.ToolStripMenuItem importItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

