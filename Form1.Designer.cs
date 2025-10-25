namespace TestProject
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            openFileToolStripMenuItem = new ToolStripMenuItem();
            openFolderToolStripMenuItem = new ToolStripMenuItem();
            treeView1 = new TreeView();
            panel1 = new Panel();
            PbFile = new ProgressBar();
            LblCounter = new Label();
            richTextBox1 = new RichTextBox();
            menuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { openFileToolStripMenuItem, openFolderToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(581, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // openFileToolStripMenuItem
            // 
            openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            openFileToolStripMenuItem.Size = new Size(69, 20);
            openFileToolStripMenuItem.Text = "Open File";
            openFileToolStripMenuItem.Click += openFileToolStripMenuItem_Click;
            // 
            // openFolderToolStripMenuItem
            // 
            openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            openFolderToolStripMenuItem.Size = new Size(81, 20);
            openFolderToolStripMenuItem.Text = "OpenFolder";
            openFolderToolStripMenuItem.Click += openFolderToolStripMenuItem_Click;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(0, 24);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(174, 132);
            treeView1.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(PbFile);
            panel1.Controls.Add(LblCounter);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 418);
            panel1.Name = "panel1";
            panel1.Size = new Size(581, 32);
            panel1.TabIndex = 22;
            // 
            // PbFile
            // 
            PbFile.Dock = DockStyle.Fill;
            PbFile.Location = new Point(0, 0);
            PbFile.Name = "PbFile";
            PbFile.Size = new Size(489, 32);
            PbFile.TabIndex = 4;
            // 
            // LblCounter
            // 
            LblCounter.Dock = DockStyle.Right;
            LblCounter.Location = new Point(489, 0);
            LblCounter.Name = "LblCounter";
            LblCounter.Size = new Size(92, 32);
            LblCounter.TabIndex = 0;
            LblCounter.Text = "0/0";
            LblCounter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(238, 28);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(343, 302);
            richTextBox1.TabIndex = 23;
            richTextBox1.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(581, 450);
            Controls.Add(richTextBox1);
            Controls.Add(panel1);
            Controls.Add(treeView1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem openFileToolStripMenuItem;
        private TreeView treeView1;
        private ToolStripMenuItem openFolderToolStripMenuItem;
        private Panel panel1;
        private ProgressBar PbFile;
        private Label LblCounter;
        private RichTextBox richTextBox1;
    }
}
