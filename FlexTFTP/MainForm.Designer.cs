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
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.autoPathCheckBox = new System.Windows.Forms.CheckBox();
            this.outputTextBox = new System.Windows.Forms.RichTextBox();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.maskedTextBoxPort = new System.Windows.Forms.MaskedTextBox();
            this.pictureBoxSettings = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.hotInfo = new System.Windows.Forms.LinkLabel();
            this.pictureBox_lockSettings = new System.Windows.Forms.PictureBox();
            this.pictureBoxOnlineState = new System.Windows.Forms.PictureBox();
            this.pictureBoxPreset1 = new System.Windows.Forms.PictureBox();
            this.progressBarDownload = new FlexTFTP.ProgressBarWithCaption();
            this.pictureBoxPreset2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_lockSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOnlineState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreset1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreset2)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilePath.ForeColor = System.Drawing.Color.Silver;
            this.textBoxFilePath.Location = new System.Drawing.Point(4, 12);
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(251, 20);
            this.textBoxFilePath.TabIndex = 2;
            this.textBoxFilePath.Text = "File...";
            this.textBoxFilePath.Enter += new System.EventHandler(this.textBoxFilePath_Enter);
            this.textBoxFilePath.Leave += new System.EventHandler(this.textBoxFilePath_Leave);
            // 
            // buttonDownload
            // 
            this.buttonDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDownload.Location = new System.Drawing.Point(234, 310);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(75, 23);
            this.buttonDownload.TabIndex = 7;
            this.buttonDownload.Text = "Download";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenFile.Location = new System.Drawing.Point(259, 9);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(47, 25);
            this.buttonOpenFile.TabIndex = 1;
            this.buttonOpenFile.Text = "Open";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.ForeColor = System.Drawing.Color.Silver;
            this.textBoxPath.Location = new System.Drawing.Point(4, 287);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(227, 20);
            this.textBoxPath.TabIndex = 3;
            this.textBoxPath.Text = "Path...";
            this.textBoxPath.TextChanged += new System.EventHandler(this.textBoxPath_TextChanged);
            this.textBoxPath.Enter += new System.EventHandler(this.textBox1_Enter);
            // 
            // autoPathCheckBox
            // 
            this.autoPathCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.autoPathCheckBox.AutoSize = true;
            this.autoPathCheckBox.Location = new System.Drawing.Point(234, 289);
            this.autoPathCheckBox.Name = "autoPathCheckBox";
            this.autoPathCheckBox.Size = new System.Drawing.Size(73, 17);
            this.autoPathCheckBox.TabIndex = 4;
            this.autoPathCheckBox.Text = "Auto Path";
            this.autoPathCheckBox.UseVisualStyleBackColor = true;
            this.autoPathCheckBox.CheckedChanged += new System.EventHandler(this.autoPathCheckBox_CheckedChanged);
            // 
            // outputTextBox
            // 
            this.outputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.outputTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputTextBox.Location = new System.Drawing.Point(4, 40);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.outputTextBox.Size = new System.Drawing.Size(305, 241);
            this.outputTextBox.TabIndex = 10;
            this.outputTextBox.TabStop = false;
            this.outputTextBox.Text = "";
            this.outputTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.outputTextBox_LinkClicked);
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAddress.Location = new System.Drawing.Point(93, 312);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(89, 20);
            this.textBoxAddress.TabIndex = 5;
            this.textBoxAddress.TextChanged += new System.EventHandler(this.textBoxAddress_TextChanged);
            // 
            // maskedTextBoxPort
            // 
            this.maskedTextBoxPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.maskedTextBoxPort.Location = new System.Drawing.Point(186, 312);
            this.maskedTextBoxPort.Mask = "00";
            this.maskedTextBoxPort.Name = "maskedTextBoxPort";
            this.maskedTextBoxPort.Size = new System.Drawing.Size(25, 20);
            this.maskedTextBoxPort.TabIndex = 6;
            this.maskedTextBoxPort.ValidatingType = typeof(System.DateTime);
            this.maskedTextBoxPort.TypeValidationCompleted += new System.Windows.Forms.TypeValidationEventHandler(this.maskedTextBoxPort_TypeValidationCompleted);
            // 
            // pictureBoxSettings
            // 
            this.pictureBoxSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxSettings.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxSettings.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxSettings.Image")));
            this.pictureBoxSettings.InitialImage = null;
            this.pictureBoxSettings.Location = new System.Drawing.Point(21, 338);
            this.pictureBoxSettings.Name = "pictureBoxSettings";
            this.pictureBoxSettings.Size = new System.Drawing.Size(11, 11);
            this.pictureBoxSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxSettings.TabIndex = 11;
            this.pictureBoxSettings.TabStop = false;
            this.pictureBoxSettings.Click += new System.EventHandler(this.pictureBoxSettings_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(4, 338);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(11, 11);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // hotInfo
            // 
            this.hotInfo.AutoSize = true;
            this.hotInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hotInfo.LinkColor = System.Drawing.Color.CornflowerBlue;
            this.hotInfo.Location = new System.Drawing.Point(37, 337);
            this.hotInfo.Name = "hotInfo";
            this.hotInfo.Size = new System.Drawing.Size(43, 12);
            this.hotInfo.TabIndex = 14;
            this.hotInfo.TabStop = true;
            this.hotInfo.Text = "Hot Infos";
            this.hotInfo.Visible = false;
            this.hotInfo.VisitedLinkColor = System.Drawing.Color.CornflowerBlue;
            this.hotInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.hotInfo_LinkClicked);
            // 
            // pictureBox_lockSettings
            // 
            this.pictureBox_lockSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_lockSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_lockSettings.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_lockSettings.Image")));
            this.pictureBox_lockSettings.InitialImage = null;
            this.pictureBox_lockSettings.Location = new System.Drawing.Point(215, 314);
            this.pictureBox_lockSettings.Name = "pictureBox_lockSettings";
            this.pictureBox_lockSettings.Size = new System.Drawing.Size(16, 16);
            this.pictureBox_lockSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_lockSettings.TabIndex = 15;
            this.pictureBox_lockSettings.TabStop = false;
            this.pictureBox_lockSettings.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBoxOnlineState
            // 
            this.pictureBoxOnlineState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxOnlineState.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxOnlineState.Image")));
            this.pictureBoxOnlineState.Location = new System.Drawing.Point(295, 338);
            this.pictureBoxOnlineState.Name = "pictureBoxOnlineState";
            this.pictureBoxOnlineState.Size = new System.Drawing.Size(11, 11);
            this.pictureBoxOnlineState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxOnlineState.TabIndex = 16;
            this.pictureBoxOnlineState.TabStop = false;
            // 
            // pictureBoxPreset1
            // 
            this.pictureBoxPreset1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxPreset1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxPreset1.Image = global::FlexTFTP.Properties.Resources.preset1_inactive;
            this.pictureBoxPreset1.Location = new System.Drawing.Point(234, 338);
            this.pictureBoxPreset1.Name = "pictureBoxPreset1";
            this.pictureBoxPreset1.Size = new System.Drawing.Size(11, 11);
            this.pictureBoxPreset1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxPreset1.TabIndex = 17;
            this.pictureBoxPreset1.TabStop = false;
            this.pictureBoxPreset1.Click += new System.EventHandler(this.pictureBoxPreset1_Click);
            // 
            // progressBarDownload
            // 
            this.progressBarDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarDownload.CustomText = "";
            this.progressBarDownload.DisplayStyle = FlexTFTP.ProgressBarDisplayText.Percentage;
            this.progressBarDownload.Location = new System.Drawing.Point(4, 312);
            this.progressBarDownload.MarqueeAnimationSpeed = 0;
            this.progressBarDownload.Name = "progressBarDownload";
            this.progressBarDownload.Size = new System.Drawing.Size(85, 20);
            this.progressBarDownload.Step = 1;
            this.progressBarDownload.TabIndex = 1;
            // 
            // pictureBoxPreset2
            // 
            this.pictureBoxPreset2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxPreset2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxPreset2.Image = global::FlexTFTP.Properties.Resources.preset2_inactive;
            this.pictureBoxPreset2.Location = new System.Drawing.Point(248, 338);
            this.pictureBoxPreset2.Name = "pictureBoxPreset2";
            this.pictureBoxPreset2.Size = new System.Drawing.Size(11, 11);
            this.pictureBoxPreset2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxPreset2.TabIndex = 18;
            this.pictureBoxPreset2.TabStop = false;
            this.pictureBoxPreset2.Click += new System.EventHandler(this.pictureBox2_Click_1);
            // 
            // FlexTftpForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 354);
            this.Controls.Add(this.pictureBoxPreset2);
            this.Controls.Add(this.pictureBoxPreset1);
            this.Controls.Add(this.pictureBoxOnlineState);
            this.Controls.Add(this.pictureBox_lockSettings);
            this.Controls.Add(this.hotInfo);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBoxSettings);
            this.Controls.Add(this.maskedTextBoxPort);
            this.Controls.Add(this.textBoxAddress);
            this.Controls.Add(this.autoPathCheckBox);
            this.Controls.Add(this.buttonOpenFile);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.textBoxFilePath);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.progressBarDownload);
            this.Controls.Add(this.buttonDownload);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "FlexTftpForm";
            this.Text = "FlexTFTP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FlexTftpForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FlexTFTPForm_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.FlexTFTPForm_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FlexTFTPForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FlexTFTPForm_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ToggleKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_lockSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOnlineState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreset1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreset2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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

