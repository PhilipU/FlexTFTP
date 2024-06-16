namespace FlexTFTP
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.deviceTab = new System.Windows.Forms.TabControl();
            this.General = new System.Windows.Forms.TabPage();
            this.labelCurrentVersion = new System.Windows.Forms.Label();
            this.labelNewestVersion = new System.Windows.Forms.Label();
            this.checkBoxUpdateBetaRing = new System.Windows.Forms.CheckBox();
            this.labelSessionStatisticsTime = new System.Windows.Forms.Label();
            this.labelSessionStatisticsBytes = new System.Windows.Forms.Label();
            this.labelSessionStatisticsTitel = new System.Windows.Forms.Label();
            this.checkBoxAutoUpdate = new System.Windows.Forms.CheckBox();
            this.updateCheck = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.maskedTextBoxMultiTargetFileMinSize = new System.Windows.Forms.MaskedTextBox();
            this.Presets = new System.Windows.Forms.TabPage();
            this.groupBoxPreset1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.maskedTextBoxPreset1Port = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxPreset1Address = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPreset1Path = new System.Windows.Forms.TextBox();
            this.checkBoxPreset1AutoPath = new System.Windows.Forms.CheckBox();
            this.checkBoxEnablePresets = new System.Windows.Forms.CheckBox();
            this.Transfer = new System.Windows.Forms.TabPage();
            this.checkBoxAutoForce = new System.Windows.Forms.CheckBox();
            this.maskedTextBoxTransferRetryTimeout = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.maskedTextBoxTransferRetryCount = new System.Windows.Forms.MaskedTextBox();
            this.Misc = new System.Windows.Forms.TabPage();
            this.checkBoxShowFullFilePath = new System.Windows.Forms.CheckBox();
            this.maskedTextBoxOnlineCheckInterval = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxOnlineCheck = new System.Windows.Forms.CheckBox();
            this.Restore = new System.Windows.Forms.TabPage();
            this.checkBoxRestoreAutoPath = new System.Windows.Forms.CheckBox();
            this.checkBoxRestoreHistory = new System.Windows.Forms.CheckBox();
            this.checkBoxRestorePath = new System.Windows.Forms.CheckBox();
            this.checkBoxRestorePort = new System.Windows.Forms.CheckBox();
            this.checkBoxRestoreIP = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxRestoreLastFile = new System.Windows.Forms.CheckBox();
            this.checkBoxRestoreWindowPos = new System.Windows.Forms.CheckBox();
            this.groupBoxPreset2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.maskedTextBoxPreset2Port = new System.Windows.Forms.MaskedTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxPreset2Address = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxPreset2Path = new System.Windows.Forms.TextBox();
            this.checkBoxPreset2AutoPath = new System.Windows.Forms.CheckBox();
            this.deviceTab.SuspendLayout();
            this.General.SuspendLayout();
            this.Presets.SuspendLayout();
            this.groupBoxPreset1.SuspendLayout();
            this.Transfer.SuspendLayout();
            this.Misc.SuspendLayout();
            this.Restore.SuspendLayout();
            this.groupBoxPreset2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(151, 234);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(70, 234);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // deviceTab
            // 
            this.deviceTab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceTab.Controls.Add(this.General);
            this.deviceTab.Controls.Add(this.Presets);
            this.deviceTab.Controls.Add(this.Transfer);
            this.deviceTab.Controls.Add(this.Misc);
            this.deviceTab.Controls.Add(this.Restore);
            this.deviceTab.Location = new System.Drawing.Point(6, 7);
            this.deviceTab.Name = "deviceTab";
            this.deviceTab.SelectedIndex = 0;
            this.deviceTab.Size = new System.Drawing.Size(220, 221);
            this.deviceTab.TabIndex = 2;
            // 
            // General
            // 
            this.General.Controls.Add(this.labelCurrentVersion);
            this.General.Controls.Add(this.labelNewestVersion);
            this.General.Controls.Add(this.checkBoxUpdateBetaRing);
            this.General.Controls.Add(this.labelSessionStatisticsTime);
            this.General.Controls.Add(this.labelSessionStatisticsBytes);
            this.General.Controls.Add(this.labelSessionStatisticsTitel);
            this.General.Controls.Add(this.checkBoxAutoUpdate);
            this.General.Controls.Add(this.updateCheck);
            this.General.Controls.Add(this.label5);
            this.General.Controls.Add(this.label4);
            this.General.Controls.Add(this.maskedTextBoxMultiTargetFileMinSize);
            this.General.Location = new System.Drawing.Point(4, 22);
            this.General.Name = "General";
            this.General.Padding = new System.Windows.Forms.Padding(3);
            this.General.Size = new System.Drawing.Size(212, 195);
            this.General.TabIndex = 2;
            this.General.Text = "General";
            this.General.UseVisualStyleBackColor = true;
            // 
            // labelCurrentVersion
            // 
            this.labelCurrentVersion.AutoSize = true;
            this.labelCurrentVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentVersion.Location = new System.Drawing.Point(6, 100);
            this.labelCurrentVersion.Name = "labelCurrentVersion";
            this.labelCurrentVersion.Size = new System.Drawing.Size(73, 12);
            this.labelCurrentVersion.TabIndex = 10;
            this.labelCurrentVersion.Text = "Current Version:";
            // 
            // labelNewestVersion
            // 
            this.labelNewestVersion.AutoSize = true;
            this.labelNewestVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNewestVersion.Location = new System.Drawing.Point(6, 84);
            this.labelNewestVersion.Name = "labelNewestVersion";
            this.labelNewestVersion.Size = new System.Drawing.Size(75, 12);
            this.labelNewestVersion.TabIndex = 9;
            this.labelNewestVersion.Text = "Newest Version:";
            // 
            // checkBoxUpdateBetaRing
            // 
            this.checkBoxUpdateBetaRing.AutoSize = true;
            this.checkBoxUpdateBetaRing.Location = new System.Drawing.Point(9, 64);
            this.checkBoxUpdateBetaRing.Name = "checkBoxUpdateBetaRing";
            this.checkBoxUpdateBetaRing.Size = new System.Drawing.Size(112, 17);
            this.checkBoxUpdateBetaRing.TabIndex = 8;
            this.checkBoxUpdateBetaRing.Text = "Use Beta versions";
            this.checkBoxUpdateBetaRing.UseVisualStyleBackColor = true;
            // 
            // labelSessionStatisticsTime
            // 
            this.labelSessionStatisticsTime.AutoSize = true;
            this.labelSessionStatisticsTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSessionStatisticsTime.Location = new System.Drawing.Point(6, 178);
            this.labelSessionStatisticsTime.Name = "labelSessionStatisticsTime";
            this.labelSessionStatisticsTime.Size = new System.Drawing.Size(23, 12);
            this.labelSessionStatisticsTime.TabIndex = 7;
            this.labelSessionStatisticsTime.Text = "time";
            // 
            // labelSessionStatisticsBytes
            // 
            this.labelSessionStatisticsBytes.AutoSize = true;
            this.labelSessionStatisticsBytes.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSessionStatisticsBytes.Location = new System.Drawing.Point(6, 161);
            this.labelSessionStatisticsBytes.Name = "labelSessionStatisticsBytes";
            this.labelSessionStatisticsBytes.Size = new System.Drawing.Size(22, 12);
            this.labelSessionStatisticsBytes.TabIndex = 6;
            this.labelSessionStatisticsBytes.Text = "size";
            // 
            // labelSessionStatisticsTitel
            // 
            this.labelSessionStatisticsTitel.AutoSize = true;
            this.labelSessionStatisticsTitel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSessionStatisticsTitel.Location = new System.Drawing.Point(6, 143);
            this.labelSessionStatisticsTitel.Name = "labelSessionStatisticsTitel";
            this.labelSessionStatisticsTitel.Size = new System.Drawing.Size(81, 12);
            this.labelSessionStatisticsTitel.TabIndex = 5;
            this.labelSessionStatisticsTitel.Text = "Session statistics:";
            // 
            // checkBoxAutoUpdate
            // 
            this.checkBoxAutoUpdate.AutoSize = true;
            this.checkBoxAutoUpdate.Location = new System.Drawing.Point(9, 48);
            this.checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
            this.checkBoxAutoUpdate.Size = new System.Drawing.Size(124, 17);
            this.checkBoxAutoUpdate.TabIndex = 4;
            this.checkBoxAutoUpdate.Text = "Automatically update";
            this.checkBoxAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // updateCheck
            // 
            this.updateCheck.AutoSize = true;
            this.updateCheck.Location = new System.Drawing.Point(9, 32);
            this.updateCheck.Name = "updateCheck";
            this.updateCheck.Size = new System.Drawing.Size(179, 17);
            this.updateCheck.TabIndex = 3;
            this.updateCheck.Text = "Check for new version at startup";
            this.updateCheck.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(173, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "MB";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Multi target file minimum size";
            // 
            // maskedTextBoxMultiTargetFileMinSize
            // 
            this.maskedTextBoxMultiTargetFileMinSize.Location = new System.Drawing.Point(151, 6);
            this.maskedTextBoxMultiTargetFileMinSize.Mask = "00";
            this.maskedTextBoxMultiTargetFileMinSize.Name = "maskedTextBoxMultiTargetFileMinSize";
            this.maskedTextBoxMultiTargetFileMinSize.Size = new System.Drawing.Size(20, 20);
            this.maskedTextBoxMultiTargetFileMinSize.TabIndex = 0;
            // 
            // Presets
            // 
            this.Presets.Controls.Add(this.groupBoxPreset2);
            this.Presets.Controls.Add(this.groupBoxPreset1);
            this.Presets.Controls.Add(this.checkBoxEnablePresets);
            this.Presets.Location = new System.Drawing.Point(4, 22);
            this.Presets.Name = "Presets";
            this.Presets.Padding = new System.Windows.Forms.Padding(3);
            this.Presets.Size = new System.Drawing.Size(212, 195);
            this.Presets.TabIndex = 4;
            this.Presets.Text = "Presets";
            this.Presets.UseVisualStyleBackColor = true;
            // 
            // groupBoxPreset1
            // 
            this.groupBoxPreset1.Controls.Add(this.label9);
            this.groupBoxPreset1.Controls.Add(this.maskedTextBoxPreset1Port);
            this.groupBoxPreset1.Controls.Add(this.label8);
            this.groupBoxPreset1.Controls.Add(this.textBoxPreset1Address);
            this.groupBoxPreset1.Controls.Add(this.label7);
            this.groupBoxPreset1.Controls.Add(this.textBoxPreset1Path);
            this.groupBoxPreset1.Controls.Add(this.checkBoxPreset1AutoPath);
            this.groupBoxPreset1.Location = new System.Drawing.Point(6, 21);
            this.groupBoxPreset1.Name = "groupBoxPreset1";
            this.groupBoxPreset1.Size = new System.Drawing.Size(200, 87);
            this.groupBoxPreset1.TabIndex = 1;
            this.groupBoxPreset1.TabStop = false;
            this.groupBoxPreset1.Text = "Preset 1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(138, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Port";
            // 
            // maskedTextBoxPreset1Port
            // 
            this.maskedTextBoxPreset1Port.Location = new System.Drawing.Point(170, 14);
            this.maskedTextBoxPreset1Port.Name = "maskedTextBoxPreset1Port";
            this.maskedTextBoxPreset1Port.Size = new System.Drawing.Size(23, 20);
            this.maskedTextBoxPreset1Port.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Address";
            // 
            // textBoxPreset1Address
            // 
            this.textBoxPreset1Address.Location = new System.Drawing.Point(75, 61);
            this.textBoxPreset1Address.Name = "textBoxPreset1Address";
            this.textBoxPreset1Address.Size = new System.Drawing.Size(118, 20);
            this.textBoxPreset1Address.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Target Path";
            // 
            // textBoxPreset1Path
            // 
            this.textBoxPreset1Path.Location = new System.Drawing.Point(75, 38);
            this.textBoxPreset1Path.Name = "textBoxPreset1Path";
            this.textBoxPreset1Path.Size = new System.Drawing.Size(118, 20);
            this.textBoxPreset1Path.TabIndex = 1;
            // 
            // checkBoxPreset1AutoPath
            // 
            this.checkBoxPreset1AutoPath.AutoSize = true;
            this.checkBoxPreset1AutoPath.Location = new System.Drawing.Point(7, 15);
            this.checkBoxPreset1AutoPath.Name = "checkBoxPreset1AutoPath";
            this.checkBoxPreset1AutoPath.Size = new System.Drawing.Size(73, 17);
            this.checkBoxPreset1AutoPath.TabIndex = 0;
            this.checkBoxPreset1AutoPath.Text = "Auto Path";
            this.checkBoxPreset1AutoPath.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnablePresets
            // 
            this.checkBoxEnablePresets.AutoSize = true;
            this.checkBoxEnablePresets.Location = new System.Drawing.Point(7, 7);
            this.checkBoxEnablePresets.Name = "checkBoxEnablePresets";
            this.checkBoxEnablePresets.Size = new System.Drawing.Size(97, 17);
            this.checkBoxEnablePresets.TabIndex = 0;
            this.checkBoxEnablePresets.Text = "Enable Presets";
            this.checkBoxEnablePresets.UseVisualStyleBackColor = true;
            this.checkBoxEnablePresets.CheckedChanged += new System.EventHandler(this.checkBoxEnablePresets_CheckedChanged);
            // 
            // Transfer
            // 
            this.Transfer.Controls.Add(this.checkBoxAutoForce);
            this.Transfer.Controls.Add(this.maskedTextBoxTransferRetryTimeout);
            this.Transfer.Controls.Add(this.label3);
            this.Transfer.Controls.Add(this.label2);
            this.Transfer.Controls.Add(this.maskedTextBoxTransferRetryCount);
            this.Transfer.Location = new System.Drawing.Point(4, 22);
            this.Transfer.Name = "Transfer";
            this.Transfer.Padding = new System.Windows.Forms.Padding(3);
            this.Transfer.Size = new System.Drawing.Size(212, 195);
            this.Transfer.TabIndex = 1;
            this.Transfer.Text = "Transfer";
            this.Transfer.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoForce
            // 
            this.checkBoxAutoForce.AutoSize = true;
            this.checkBoxAutoForce.Location = new System.Drawing.Point(9, 65);
            this.checkBoxAutoForce.Name = "checkBoxAutoForce";
            this.checkBoxAutoForce.Size = new System.Drawing.Size(143, 17);
            this.checkBoxAutoForce.TabIndex = 4;
            this.checkBoxAutoForce.Text = "Automatically add \'-force\'";
            this.checkBoxAutoForce.UseVisualStyleBackColor = true;
            // 
            // maskedTextBoxTransferRetryTimeout
            // 
            this.maskedTextBoxTransferRetryTimeout.Location = new System.Drawing.Point(88, 39);
            this.maskedTextBoxTransferRetryTimeout.Mask = "00";
            this.maskedTextBoxTransferRetryTimeout.Name = "maskedTextBoxTransferRetryTimeout";
            this.maskedTextBoxTransferRetryTimeout.Size = new System.Drawing.Size(24, 20);
            this.maskedTextBoxTransferRetryTimeout.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Retry timeout";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Retry count";
            // 
            // maskedTextBoxTransferRetryCount
            // 
            this.maskedTextBoxTransferRetryCount.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.maskedTextBoxTransferRetryCount.Location = new System.Drawing.Point(88, 12);
            this.maskedTextBoxTransferRetryCount.Mask = "00";
            this.maskedTextBoxTransferRetryCount.Name = "maskedTextBoxTransferRetryCount";
            this.maskedTextBoxTransferRetryCount.Size = new System.Drawing.Size(24, 20);
            this.maskedTextBoxTransferRetryCount.TabIndex = 0;
            this.maskedTextBoxTransferRetryCount.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // Misc
            // 
            this.Misc.Controls.Add(this.checkBoxShowFullFilePath);
            this.Misc.Controls.Add(this.maskedTextBoxOnlineCheckInterval);
            this.Misc.Controls.Add(this.label6);
            this.Misc.Controls.Add(this.checkBoxOnlineCheck);
            this.Misc.Location = new System.Drawing.Point(4, 22);
            this.Misc.Name = "Misc";
            this.Misc.Padding = new System.Windows.Forms.Padding(3);
            this.Misc.Size = new System.Drawing.Size(212, 195);
            this.Misc.TabIndex = 3;
            this.Misc.Text = "Misc";
            this.Misc.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowFullFilePath
            // 
            this.checkBoxShowFullFilePath.AutoSize = true;
            this.checkBoxShowFullFilePath.Location = new System.Drawing.Point(6, 59);
            this.checkBoxShowFullFilePath.Name = "checkBoxShowFullFilePath";
            this.checkBoxShowFullFilePath.Size = new System.Drawing.Size(109, 17);
            this.checkBoxShowFullFilePath.TabIndex = 3;
            this.checkBoxShowFullFilePath.Text = "Show full file path";
            this.checkBoxShowFullFilePath.UseVisualStyleBackColor = true;
            // 
            // maskedTextBoxOnlineCheckInterval
            // 
            this.maskedTextBoxOnlineCheckInterval.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.maskedTextBoxOnlineCheckInterval.Location = new System.Drawing.Point(141, 23);
            this.maskedTextBoxOnlineCheckInterval.Mask = "0000";
            this.maskedTextBoxOnlineCheckInterval.Name = "maskedTextBoxOnlineCheckInterval";
            this.maskedTextBoxOnlineCheckInterval.Size = new System.Drawing.Size(37, 20);
            this.maskedTextBoxOnlineCheckInterval.TabIndex = 2;
            this.maskedTextBoxOnlineCheckInterval.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.maskedTextBoxOnlineCheckInterval_MaskInputRejected);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Online Check Interval (ms)";
            // 
            // checkBoxOnlineCheck
            // 
            this.checkBoxOnlineCheck.AutoSize = true;
            this.checkBoxOnlineCheck.Location = new System.Drawing.Point(6, 6);
            this.checkBoxOnlineCheck.Name = "checkBoxOnlineCheck";
            this.checkBoxOnlineCheck.Size = new System.Drawing.Size(90, 17);
            this.checkBoxOnlineCheck.TabIndex = 0;
            this.checkBoxOnlineCheck.Text = "Online Check";
            this.checkBoxOnlineCheck.UseVisualStyleBackColor = true;
            // 
            // Restore
            // 
            this.Restore.Controls.Add(this.checkBoxRestoreAutoPath);
            this.Restore.Controls.Add(this.checkBoxRestoreHistory);
            this.Restore.Controls.Add(this.checkBoxRestorePath);
            this.Restore.Controls.Add(this.checkBoxRestorePort);
            this.Restore.Controls.Add(this.checkBoxRestoreIP);
            this.Restore.Controls.Add(this.label1);
            this.Restore.Controls.Add(this.checkBoxRestoreLastFile);
            this.Restore.Controls.Add(this.checkBoxRestoreWindowPos);
            this.Restore.Location = new System.Drawing.Point(4, 22);
            this.Restore.Name = "Restore";
            this.Restore.Padding = new System.Windows.Forms.Padding(3);
            this.Restore.Size = new System.Drawing.Size(212, 195);
            this.Restore.TabIndex = 0;
            this.Restore.Text = "Restore";
            this.Restore.UseVisualStyleBackColor = true;
            // 
            // checkBoxRestoreAutoPath
            // 
            this.checkBoxRestoreAutoPath.AutoSize = true;
            this.checkBoxRestoreAutoPath.Location = new System.Drawing.Point(6, 119);
            this.checkBoxRestoreAutoPath.Name = "checkBoxRestoreAutoPath";
            this.checkBoxRestoreAutoPath.Size = new System.Drawing.Size(157, 17);
            this.checkBoxRestoreAutoPath.TabIndex = 7;
            this.checkBoxRestoreAutoPath.Text = "Type dependend Auto Path";
            this.checkBoxRestoreAutoPath.UseVisualStyleBackColor = true;
            // 
            // checkBoxRestoreHistory
            // 
            this.checkBoxRestoreHistory.AutoSize = true;
            this.checkBoxRestoreHistory.Location = new System.Drawing.Point(6, 103);
            this.checkBoxRestoreHistory.Name = "checkBoxRestoreHistory";
            this.checkBoxRestoreHistory.Size = new System.Drawing.Size(103, 17);
            this.checkBoxRestoreHistory.TabIndex = 6;
            this.checkBoxRestoreHistory.Text = "Complete history";
            this.checkBoxRestoreHistory.UseVisualStyleBackColor = true;
            // 
            // checkBoxRestorePath
            // 
            this.checkBoxRestorePath.AutoSize = true;
            this.checkBoxRestorePath.Location = new System.Drawing.Point(6, 87);
            this.checkBoxRestorePath.Name = "checkBoxRestorePath";
            this.checkBoxRestorePath.Size = new System.Drawing.Size(48, 17);
            this.checkBoxRestorePath.TabIndex = 5;
            this.checkBoxRestorePath.Text = "Path";
            this.checkBoxRestorePath.UseVisualStyleBackColor = true;
            // 
            // checkBoxRestorePort
            // 
            this.checkBoxRestorePort.AutoSize = true;
            this.checkBoxRestorePort.Location = new System.Drawing.Point(6, 71);
            this.checkBoxRestorePort.Name = "checkBoxRestorePort";
            this.checkBoxRestorePort.Size = new System.Drawing.Size(45, 17);
            this.checkBoxRestorePort.TabIndex = 4;
            this.checkBoxRestorePort.Text = "Port";
            this.checkBoxRestorePort.UseVisualStyleBackColor = true;
            // 
            // checkBoxRestoreIP
            // 
            this.checkBoxRestoreIP.AutoSize = true;
            this.checkBoxRestoreIP.Location = new System.Drawing.Point(6, 55);
            this.checkBoxRestoreIP.Name = "checkBoxRestoreIP";
            this.checkBoxRestoreIP.Size = new System.Drawing.Size(76, 17);
            this.checkBoxRestoreIP.TabIndex = 3;
            this.checkBoxRestoreIP.Text = "IP address";
            this.checkBoxRestoreIP.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Restore after start";
            // 
            // checkBoxRestoreLastFile
            // 
            this.checkBoxRestoreLastFile.AutoSize = true;
            this.checkBoxRestoreLastFile.Location = new System.Drawing.Point(6, 39);
            this.checkBoxRestoreLastFile.Name = "checkBoxRestoreLastFile";
            this.checkBoxRestoreLastFile.Size = new System.Drawing.Size(101, 17);
            this.checkBoxRestoreLastFile.TabIndex = 1;
            this.checkBoxRestoreLastFile.Text = "Last opened file";
            this.checkBoxRestoreLastFile.UseVisualStyleBackColor = true;
            // 
            // checkBoxRestoreWindowPos
            // 
            this.checkBoxRestoreWindowPos.AutoSize = true;
            this.checkBoxRestoreWindowPos.Location = new System.Drawing.Point(6, 23);
            this.checkBoxRestoreWindowPos.Name = "checkBoxRestoreWindowPos";
            this.checkBoxRestoreWindowPos.Size = new System.Drawing.Size(104, 17);
            this.checkBoxRestoreWindowPos.TabIndex = 0;
            this.checkBoxRestoreWindowPos.Text = "Window position";
            this.checkBoxRestoreWindowPos.UseVisualStyleBackColor = true;
            // 
            // groupBoxPreset2
            // 
            this.groupBoxPreset2.Controls.Add(this.label10);
            this.groupBoxPreset2.Controls.Add(this.maskedTextBoxPreset2Port);
            this.groupBoxPreset2.Controls.Add(this.label11);
            this.groupBoxPreset2.Controls.Add(this.textBoxPreset2Address);
            this.groupBoxPreset2.Controls.Add(this.label12);
            this.groupBoxPreset2.Controls.Add(this.textBoxPreset2Path);
            this.groupBoxPreset2.Controls.Add(this.checkBoxPreset2AutoPath);
            this.groupBoxPreset2.Location = new System.Drawing.Point(3, 108);
            this.groupBoxPreset2.Name = "groupBoxPreset2";
            this.groupBoxPreset2.Size = new System.Drawing.Size(200, 87);
            this.groupBoxPreset2.TabIndex = 2;
            this.groupBoxPreset2.TabStop = false;
            this.groupBoxPreset2.Text = "Preset 2";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(138, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Port";
            // 
            // maskedTextBoxPreset2Port
            // 
            this.maskedTextBoxPreset2Port.Location = new System.Drawing.Point(170, 14);
            this.maskedTextBoxPreset2Port.Name = "maskedTextBoxPreset2Port";
            this.maskedTextBoxPreset2Port.Size = new System.Drawing.Size(23, 20);
            this.maskedTextBoxPreset2Port.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 65);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Address";
            // 
            // textBoxPreset2Address
            // 
            this.textBoxPreset2Address.Location = new System.Drawing.Point(75, 61);
            this.textBoxPreset2Address.Name = "textBoxPreset2Address";
            this.textBoxPreset2Address.Size = new System.Drawing.Size(118, 20);
            this.textBoxPreset2Address.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 41);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Target Path";
            // 
            // textBoxPreset2Path
            // 
            this.textBoxPreset2Path.Location = new System.Drawing.Point(75, 38);
            this.textBoxPreset2Path.Name = "textBoxPreset2Path";
            this.textBoxPreset2Path.Size = new System.Drawing.Size(118, 20);
            this.textBoxPreset2Path.TabIndex = 1;
            // 
            // checkBoxPreset2AutoPath
            // 
            this.checkBoxPreset2AutoPath.AutoSize = true;
            this.checkBoxPreset2AutoPath.Location = new System.Drawing.Point(7, 15);
            this.checkBoxPreset2AutoPath.Name = "checkBoxPreset2AutoPath";
            this.checkBoxPreset2AutoPath.Size = new System.Drawing.Size(73, 17);
            this.checkBoxPreset2AutoPath.TabIndex = 0;
            this.checkBoxPreset2AutoPath.Text = "Auto Path";
            this.checkBoxPreset2AutoPath.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 261);
            this.Controls.Add(this.deviceTab);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.deviceTab.ResumeLayout(false);
            this.General.ResumeLayout(false);
            this.General.PerformLayout();
            this.Presets.ResumeLayout(false);
            this.Presets.PerformLayout();
            this.groupBoxPreset1.ResumeLayout(false);
            this.groupBoxPreset1.PerformLayout();
            this.Transfer.ResumeLayout(false);
            this.Transfer.PerformLayout();
            this.Misc.ResumeLayout(false);
            this.Misc.PerformLayout();
            this.Restore.ResumeLayout(false);
            this.Restore.PerformLayout();
            this.groupBoxPreset2.ResumeLayout(false);
            this.groupBoxPreset2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabControl deviceTab;
        private System.Windows.Forms.TabPage Restore;
        private System.Windows.Forms.TabPage Transfer;
        private System.Windows.Forms.CheckBox checkBoxRestoreWindowPos;
        private System.Windows.Forms.CheckBox checkBoxRestoreLastFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxRestoreIP;
        private System.Windows.Forms.CheckBox checkBoxRestorePort;
        private System.Windows.Forms.CheckBox checkBoxRestorePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxTransferRetryCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxTransferRetryTimeout;
        private System.Windows.Forms.TabPage General;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxMultiTargetFileMinSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox updateCheck;
        private System.Windows.Forms.CheckBox checkBoxRestoreHistory;
        private System.Windows.Forms.CheckBox checkBoxAutoUpdate;
        private System.Windows.Forms.Label labelSessionStatisticsTime;
        private System.Windows.Forms.Label labelSessionStatisticsBytes;
        private System.Windows.Forms.Label labelSessionStatisticsTitel;
        private System.Windows.Forms.CheckBox checkBoxUpdateBetaRing;
        private System.Windows.Forms.Label labelCurrentVersion;
        private System.Windows.Forms.Label labelNewestVersion;
        private System.Windows.Forms.CheckBox checkBoxAutoForce;
        private System.Windows.Forms.CheckBox checkBoxRestoreAutoPath;
        private System.Windows.Forms.TabPage Misc;
        private System.Windows.Forms.CheckBox checkBoxOnlineCheck;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxOnlineCheckInterval;
        private System.Windows.Forms.CheckBox checkBoxShowFullFilePath;
        private System.Windows.Forms.TabPage Presets;
        private System.Windows.Forms.CheckBox checkBoxEnablePresets;
        private System.Windows.Forms.GroupBox groupBoxPreset1;
        private System.Windows.Forms.CheckBox checkBoxPreset1AutoPath;
        private System.Windows.Forms.TextBox textBoxPreset1Address;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPreset1Path;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxPreset1Port;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBoxPreset2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxPreset2Port;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxPreset2Address;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxPreset2Path;
        private System.Windows.Forms.CheckBox checkBoxPreset2AutoPath;
    }
}