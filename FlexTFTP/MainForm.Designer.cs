using System.Drawing;
namespace FlexTFTP
{
    partial class FlexTftpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlexTftpForm));
            textBoxFilePath = new TextBox();
            buttonDownload = new Button();
            buttonOpenFile = new Button();
            textBoxPath = new TextBox();
            autoPathCheckBox = new CheckBox();
            outputTextBox = new RichTextBox();
            textBoxAddress = new TextBox();
            openFileDialog1 = new OpenFileDialog();
            maskedTextBoxPort = new MaskedTextBox();
            pictureBoxSettings = new PictureBox();
            pictureBox1 = new PictureBox();
            hotInfo = new LinkLabel();
            pictureBox_lockSettings = new PictureBox();
            pictureBoxOnlineState = new PictureBox();
            pictureBoxPreset1 = new PictureBox();
            progressBarDownload = new ProgressBarWithCaption();
            pictureBoxPreset2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSettings).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_lockSettings).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOnlineState).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreset1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreset2).BeginInit();
            SuspendLayout();
            // 
            // textBoxFilePath
            // 
            textBoxFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxFilePath.ForeColor = Color.Silver;
            textBoxFilePath.Location = new Point(4, 14);
            textBoxFilePath.Margin = new Padding(4);
            textBoxFilePath.Name = "textBoxFilePath";
            textBoxFilePath.Size = new Size(292, 23);
            textBoxFilePath.TabIndex = 2;
            textBoxFilePath.Text = "File...";
            textBoxFilePath.Enter += textBoxFilePath_Enter;
            textBoxFilePath.Leave += textBoxFilePath_Leave;
            // 
            // buttonDownload
            // 
            buttonDownload.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonDownload.Location = new Point(270, 361);
            buttonDownload.Margin = new Padding(4);
            buttonDownload.Name = "buttonDownload";
            buttonDownload.Size = new Size(88, 23);
            buttonDownload.TabIndex = 7;
            buttonDownload.Text = "Download";
            buttonDownload.UseVisualStyleBackColor = true;
            buttonDownload.Click += buttonDownload_Click;
            // 
            // buttonOpenFile
            // 
            buttonOpenFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonOpenFile.Location = new Point(302, 14);
            buttonOpenFile.Margin = new Padding(4);
            buttonOpenFile.Name = "buttonOpenFile";
            buttonOpenFile.Size = new Size(57, 23);
            buttonOpenFile.TabIndex = 1;
            buttonOpenFile.Text = "Open";
            buttonOpenFile.UseVisualStyleBackColor = true;
            buttonOpenFile.Click += buttonOpenFile_Click;
            // 
            // textBoxPath
            // 
            textBoxPath.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxPath.ForeColor = Color.Silver;
            textBoxPath.Location = new Point(4, 332);
            textBoxPath.Margin = new Padding(4);
            textBoxPath.Name = "textBoxPath";
            textBoxPath.Size = new Size(264, 23);
            textBoxPath.TabIndex = 3;
            textBoxPath.Text = "Path...";
            textBoxPath.TextChanged += textBoxPath_TextChanged;
            textBoxPath.Enter += textBox1_Enter;
            // 
            // autoPathCheckBox
            // 
            autoPathCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            autoPathCheckBox.AutoSize = true;
            autoPathCheckBox.Location = new Point(279, 334);
            autoPathCheckBox.Margin = new Padding(4);
            autoPathCheckBox.Name = "autoPathCheckBox";
            autoPathCheckBox.Size = new Size(79, 19);
            autoPathCheckBox.TabIndex = 4;
            autoPathCheckBox.Text = "Auto Path";
            autoPathCheckBox.UseVisualStyleBackColor = true;
            autoPathCheckBox.CheckedChanged += autoPathCheckBox_CheckedChanged;
            // 
            // outputTextBox
            // 
            outputTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            outputTextBox.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            outputTextBox.Location = new Point(4, 46);
            outputTextBox.Margin = new Padding(4);
            outputTextBox.Name = "outputTextBox";
            outputTextBox.ReadOnly = true;
            outputTextBox.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            outputTextBox.Size = new Size(355, 278);
            outputTextBox.TabIndex = 10;
            outputTextBox.TabStop = false;
            outputTextBox.Text = "";
            outputTextBox.LinkClicked += outputTextBox_LinkClicked;
            // 
            // textBoxAddress
            // 
            textBoxAddress.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            textBoxAddress.Location = new Point(108, 361);
            textBoxAddress.Margin = new Padding(4);
            textBoxAddress.Name = "textBoxAddress";
            textBoxAddress.Size = new Size(103, 23);
            textBoxAddress.TabIndex = 5;
            textBoxAddress.TextChanged += textBoxAddress_TextChanged;
            // 
            // maskedTextBoxPort
            // 
            maskedTextBoxPort.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            maskedTextBoxPort.Location = new Point(217, 361);
            maskedTextBoxPort.Margin = new Padding(4);
            maskedTextBoxPort.Mask = "00";
            maskedTextBoxPort.Name = "maskedTextBoxPort";
            maskedTextBoxPort.Size = new Size(28, 23);
            maskedTextBoxPort.TabIndex = 6;
            maskedTextBoxPort.ValidatingType = typeof(DateTime);
            maskedTextBoxPort.TypeValidationCompleted += maskedTextBoxPort_TypeValidationCompleted;
            // 
            // pictureBoxSettings
            // 
            pictureBoxSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pictureBoxSettings.BackColor = Color.Transparent;
            pictureBoxSettings.Cursor = Cursors.Hand;
            pictureBoxSettings.Image = (Image)resources.GetObject("pictureBoxSettings.Image");
            pictureBoxSettings.InitialImage = null;
            pictureBoxSettings.Location = new Point(24, 390);
            pictureBoxSettings.Margin = new Padding(4);
            pictureBoxSettings.Name = "pictureBoxSettings";
            pictureBoxSettings.Size = new Size(13, 13);
            pictureBoxSettings.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxSettings.TabIndex = 11;
            pictureBoxSettings.TabStop = false;
            pictureBoxSettings.Click += pictureBoxSettings_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(4, 390);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(13, 13);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // hotInfo
            // 
            hotInfo.AutoSize = true;
            hotInfo.Font = new Font("Microsoft Sans Serif", 6.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            hotInfo.LinkColor = Color.CornflowerBlue;
            hotInfo.Location = new Point(43, 388);
            hotInfo.Margin = new Padding(4, 0, 4, 0);
            hotInfo.Name = "hotInfo";
            hotInfo.Size = new Size(43, 12);
            hotInfo.TabIndex = 14;
            hotInfo.TabStop = true;
            hotInfo.Text = "Hot Infos";
            hotInfo.Visible = false;
            hotInfo.VisitedLinkColor = Color.CornflowerBlue;
            hotInfo.LinkClicked += hotInfo_LinkClicked;
            // 
            // pictureBox_lockSettings
            // 
            pictureBox_lockSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pictureBox_lockSettings.Cursor = Cursors.Hand;
            pictureBox_lockSettings.Image = (Image)resources.GetObject("pictureBox_lockSettings.Image");
            pictureBox_lockSettings.InitialImage = null;
            pictureBox_lockSettings.Location = new Point(251, 363);
            pictureBox_lockSettings.Margin = new Padding(4);
            pictureBox_lockSettings.Name = "pictureBox_lockSettings";
            pictureBox_lockSettings.Size = new Size(18, 19);
            pictureBox_lockSettings.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox_lockSettings.TabIndex = 15;
            pictureBox_lockSettings.TabStop = false;
            pictureBox_lockSettings.Click += pictureBox2_Click;
            // 
            // pictureBoxOnlineState
            // 
            pictureBoxOnlineState.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pictureBoxOnlineState.Image = (Image)resources.GetObject("pictureBoxOnlineState.Image");
            pictureBoxOnlineState.Location = new Point(344, 390);
            pictureBoxOnlineState.Margin = new Padding(4);
            pictureBoxOnlineState.Name = "pictureBoxOnlineState";
            pictureBoxOnlineState.Size = new Size(13, 13);
            pictureBoxOnlineState.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxOnlineState.TabIndex = 16;
            pictureBoxOnlineState.TabStop = false;
            // 
            // pictureBoxPreset1
            // 
            pictureBoxPreset1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pictureBoxPreset1.Cursor = Cursors.Hand;
            pictureBoxPreset1.Image = Properties.Resources.preset1_inactive;
            pictureBoxPreset1.Location = new Point(273, 390);
            pictureBoxPreset1.Margin = new Padding(4);
            pictureBoxPreset1.Name = "pictureBoxPreset1";
            pictureBoxPreset1.Size = new Size(13, 13);
            pictureBoxPreset1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPreset1.TabIndex = 17;
            pictureBoxPreset1.TabStop = false;
            pictureBoxPreset1.Click += pictureBoxPreset1_Click;
            // 
            // progressBarDownload
            // 
            progressBarDownload.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBarDownload.CustomText = "";
            progressBarDownload.DisplayStyle = ProgressBarDisplayText.Percentage;
            progressBarDownload.Location = new Point(4, 361);
            progressBarDownload.Margin = new Padding(4);
            progressBarDownload.MarqueeAnimationSpeed = 0;
            progressBarDownload.Name = "progressBarDownload";
            progressBarDownload.Size = new Size(99, 23);
            progressBarDownload.Step = 1;
            progressBarDownload.TabIndex = 1;
            // 
            // pictureBoxPreset2
            // 
            pictureBoxPreset2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            pictureBoxPreset2.Cursor = Cursors.Hand;
            pictureBoxPreset2.Image = Properties.Resources.preset2_inactive;
            pictureBoxPreset2.Location = new Point(290, 390);
            pictureBoxPreset2.Margin = new Padding(4);
            pictureBoxPreset2.Name = "pictureBoxPreset2";
            pictureBoxPreset2.Size = new Size(13, 13);
            pictureBoxPreset2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPreset2.TabIndex = 18;
            pictureBoxPreset2.TabStop = false;
            pictureBoxPreset2.Click += pictureBox2_Click_1;
            // 
            // FlexTftpForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(364, 409);
            Controls.Add(pictureBoxPreset2);
            Controls.Add(pictureBoxPreset1);
            Controls.Add(pictureBoxOnlineState);
            Controls.Add(pictureBox_lockSettings);
            Controls.Add(hotInfo);
            Controls.Add(pictureBox1);
            Controls.Add(pictureBoxSettings);
            Controls.Add(maskedTextBoxPort);
            Controls.Add(textBoxAddress);
            Controls.Add(autoPathCheckBox);
            Controls.Add(buttonOpenFile);
            Controls.Add(textBoxPath);
            Controls.Add(textBoxFilePath);
            Controls.Add(outputTextBox);
            Controls.Add(progressBarDownload);
            Controls.Add(buttonDownload);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new Padding(4);
            MinimumSize = new Size(347, 337);
            Name = "FlexTftpForm";
            Text = "FlexTFTP";
            Activated += FlexTftpForm_Activated;
            FormClosing += FlexTftpForm_FormClosing;
            FormClosed += FlexTFTPForm_FormClosed;
            Load += Form1_Load;
            Shown += FlexTFTPForm_Shown;
            DragDrop += FlexTFTPForm_DragDrop;
            DragEnter += FlexTFTPForm_DragEnter;
            Enter += FlexTftpForm_Enter;
            KeyDown += ToggleKeyDown;
            ((System.ComponentModel.ISupportInitialize)pictureBoxSettings).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_lockSettings).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOnlineState).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreset1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreset2).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Button buttonOpenFile;
        private ProgressBarWithCaption progressBarDownload;
        private System.Windows.Forms.CheckBox autoPathCheckBox;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.RichTextBox outputTextBox;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxPort;
        private System.Windows.Forms.PictureBox pictureBoxSettings;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel hotInfo;
        private System.Windows.Forms.PictureBox pictureBox_lockSettings;
        private System.Windows.Forms.PictureBox pictureBoxOnlineState;
        private System.Windows.Forms.PictureBox pictureBoxPreset1;
        private System.Windows.Forms.PictureBox pictureBoxPreset2;
    }
}

