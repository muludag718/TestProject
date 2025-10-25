namespace ProtoType2.Tools.UI
{
    partial class P3DForm
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
            if (disposing && (components != null))
            {
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
            logTextBox = new TextBox();
            groupBox1 = new GroupBox();
            BtnOutputFolder = new Button();
            TxtOutputFolder = new TextBox();
            label2 = new Label();
            BtnInputFolder = new Button();
            TxtInputFolder = new TextBox();
            label1 = new Label();
            BtnStart = new Button();
            panel1 = new Panel();
            PbFile = new ProgressBar();
            LblCounter = new Label();
            BtnStop = new Button();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // logTextBox
            // 
            logTextBox.BackColor = SystemColors.InfoText;
            logTextBox.Dock = DockStyle.Fill;
            logTextBox.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            logTextBox.ForeColor = Color.Lime;
            logTextBox.Location = new Point(10, 26);
            logTextBox.Margin = new Padding(4, 3, 4, 3);
            logTextBox.Multiline = true;
            logTextBox.Name = "logTextBox";
            logTextBox.ReadOnly = true;
            logTextBox.ScrollBars = ScrollBars.Vertical;
            logTextBox.Size = new Size(815, 250);
            logTextBox.TabIndex = 11;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(logTextBox);
            groupBox1.Dock = DockStyle.Bottom;
            groupBox1.Location = new Point(0, 132);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(10);
            groupBox1.Size = new Size(835, 286);
            groupBox1.TabIndex = 12;
            groupBox1.TabStop = false;
            groupBox1.Text = "Transaction Logs";
            // 
            // BtnOutputFolder
            // 
            BtnOutputFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnOutputFolder.Location = new Point(628, 59);
            BtnOutputFolder.Margin = new Padding(4, 3, 4, 3);
            BtnOutputFolder.Name = "BtnOutputFolder";
            BtnOutputFolder.Size = new Size(88, 27);
            BtnOutputFolder.TabIndex = 19;
            BtnOutputFolder.Text = "Browse...";
            BtnOutputFolder.UseVisualStyleBackColor = true;
            BtnOutputFolder.Click += BtnOutputFolder_Click;
            // 
            // TxtOutputFolder
            // 
            TxtOutputFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TxtOutputFolder.Location = new Point(104, 61);
            TxtOutputFolder.Margin = new Padding(4, 3, 4, 3);
            TxtOutputFolder.Name = "TxtOutputFolder";
            TxtOutputFolder.Size = new Size(516, 23);
            TxtOutputFolder.TabIndex = 18;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 65);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(84, 15);
            label2.TabIndex = 17;
            label2.Text = "Output Folder:";
            // 
            // BtnInputFolder
            // 
            BtnInputFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnInputFolder.Location = new Point(628, 26);
            BtnInputFolder.Margin = new Padding(4, 3, 4, 3);
            BtnInputFolder.Name = "BtnInputFolder";
            BtnInputFolder.Size = new Size(88, 27);
            BtnInputFolder.TabIndex = 16;
            BtnInputFolder.Text = "Browse...";
            BtnInputFolder.UseVisualStyleBackColor = true;
            BtnInputFolder.Click += BtnInputFolder_Click;
            // 
            // TxtInputFolder
            // 
            TxtInputFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TxtInputFolder.Location = new Point(104, 28);
            TxtInputFolder.Margin = new Padding(4, 3, 4, 3);
            TxtInputFolder.Name = "TxtInputFolder";
            TxtInputFolder.Size = new Size(516, 23);
            TxtInputFolder.TabIndex = 15;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 31);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(74, 15);
            label1.TabIndex = 14;
            label1.Text = "Input Folder:";
            // 
            // BtnStart
            // 
            BtnStart.Location = new Point(532, 114);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(88, 27);
            BtnStart.TabIndex = 20;
            BtnStart.Text = "Start";
            BtnStart.UseVisualStyleBackColor = true;
            BtnStart.Click += Start_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(PbFile);
            panel1.Controls.Add(LblCounter);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 418);
            panel1.Name = "panel1";
            panel1.Size = new Size(835, 32);
            panel1.TabIndex = 21;
            // 
            // PbFile
            // 
            PbFile.Dock = DockStyle.Fill;
            PbFile.Location = new Point(0, 0);
            PbFile.Name = "PbFile";
            PbFile.Size = new Size(743, 32);
            PbFile.TabIndex = 4;
            // 
            // LblCounter
            // 
            LblCounter.Dock = DockStyle.Right;
            LblCounter.Location = new Point(743, 0);
            LblCounter.Name = "LblCounter";
            LblCounter.Size = new Size(92, 32);
            LblCounter.TabIndex = 0;
            LblCounter.Text = "0/0";
            LblCounter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // BtnStop
            // 
            BtnStop.Location = new Point(628, 114);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new Size(88, 27);
            BtnStop.TabIndex = 22;
            BtnStop.Text = "Stop";
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Click += BtnStop_Click;
            // 
            // P3DForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(835, 450);
            Controls.Add(BtnStop);
            Controls.Add(BtnStart);
            Controls.Add(BtnOutputFolder);
            Controls.Add(TxtOutputFolder);
            Controls.Add(label2);
            Controls.Add(BtnInputFolder);
            Controls.Add(TxtInputFolder);
            Controls.Add(label1);
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            Name = "P3DForm";
            Text = "P3DForm";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox logTextBox;
        private GroupBox groupBox1;
        private Button BtnOutputFolder;
        private TextBox TxtOutputFolder;
        private Label label2;
        private Button BtnInputFolder;
        private TextBox TxtInputFolder;
        private Label label1;
        private Button BtnStart;
        private Panel panel1;
        private ProgressBar PbFile;
        private Label LblCounter;
        private Button BtnStop;
    }
}