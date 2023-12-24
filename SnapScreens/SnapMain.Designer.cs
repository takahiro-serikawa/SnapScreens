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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SnapMain));
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            importItem = new ToolStripMenuItem();
            SettingsItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            QuitItem = new ToolStripMenuItem();
            applyButton = new Button();
            openFileDialog1 = new OpenFileDialog();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "snap screens";
            notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(32, 32);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { importItem, SettingsItem, toolStripMenuItem1, QuitItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(255, 124);
            // 
            // importItem
            // 
            importItem.Name = "importItem";
            importItem.Size = new Size(254, 38);
            importItem.Text = "Import Image ...";
            importItem.Click += importItem_Click;
            // 
            // SettingsItem
            // 
            SettingsItem.Name = "SettingsItem";
            SettingsItem.Size = new Size(254, 38);
            SettingsItem.Text = "Settings ...";
            SettingsItem.Click += SettingsItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(251, 6);
            // 
            // QuitItem
            // 
            QuitItem.Name = "QuitItem";
            QuitItem.Size = new Size(254, 38);
            QuitItem.Text = "Quit";
            QuitItem.Click += QuitItem_Click;
            // 
            // applyButton
            // 
            applyButton.Location = new Point(656, 490);
            applyButton.Margin = new Padding(6, 8, 6, 8);
            applyButton.Name = "applyButton";
            applyButton.Size = new Size(162, 56);
            applyButton.TabIndex = 1;
            applyButton.Text = "apply";
            applyButton.UseVisualStyleBackColor = true;
            applyButton.Click += applyButton_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.DefaultExt = "png";
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // SnapMain
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(867, 576);
            Controls.Add(applyButton);
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SnapMain";
            ShowInTaskbar = false;
            Text = "SnapScreens";
            FormClosing += SnapSettings_FormClosing;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
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

