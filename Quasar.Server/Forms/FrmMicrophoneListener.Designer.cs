namespace Quasar.Server.Forms
{
    partial class FrmMicrophoneListener
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMicrophoneListener));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblRealTimeStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabLiveListening = new System.Windows.Forms.TabPage();
            this.groupBoxLiveControls = new System.Windows.Forms.GroupBox();
            this.btnStopListening = new System.Windows.Forms.Button();
            this.btnStartListening = new System.Windows.Forms.Button();
            this.groupBoxLiveMicrophones = new System.Windows.Forms.GroupBox();
            this.btnRefreshLiveMicrophones = new System.Windows.Forms.Button();
            this.cmbLiveMicrophones = new System.Windows.Forms.ComboBox();
            this.lblLiveMicrophones = new System.Windows.Forms.Label();
            this.groupBoxLiveSettings = new System.Windows.Forms.GroupBox();
            this.numLiveQuality = new System.Windows.Forms.NumericUpDown();
            this.lblLiveQuality = new System.Windows.Forms.Label();
            this.tabRecording = new System.Windows.Forms.TabPage();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.numChunkDuration = new System.Windows.Forms.NumericUpDown();
            this.lblChunkDuration = new System.Windows.Forms.Label();
            this.numQuality = new System.Windows.Forms.NumericUpDown();
            this.lblQuality = new System.Windows.Forms.Label();
            this.groupBoxMicrophones = new System.Windows.Forms.GroupBox();
            this.btnRefreshMicrophones = new System.Windows.Forms.Button();
            this.cmbMicrophones = new System.Windows.Forms.ComboBox();
            this.lblMicrophones = new System.Windows.Forms.Label();
            this.groupBoxControls = new System.Windows.Forms.GroupBox();
            this.btnStopRecording = new System.Windows.Forms.Button();
            this.btnStartRecording = new System.Windows.Forms.Button();
            this.tabAudioLogs = new System.Windows.Forms.TabPage();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBoxRemote = new System.Windows.Forms.GroupBox();
            this.btnGetAudioLogs = new System.Windows.Forms.Button();
            this.lstRemoteAudioFiles = new System.Windows.Forms.ListView();
            this.colRemoteFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRemoteFileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRemoteFileDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxLocal = new System.Windows.Forms.GroupBox();
            this.btnRefreshLocal = new System.Windows.Forms.Button();
            this.lstLocalAudioFiles = new System.Windows.Forms.ListView();
            this.colLocalFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLocalFileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLocalFileDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabRecording.SuspendLayout();
            this.groupBoxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numChunkDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuality)).BeginInit();
            this.groupBoxMicrophones.SuspendLayout();
            this.groupBoxControls.SuspendLayout();
            this.tabAudioLogs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBoxRemote.SuspendLayout();
            this.groupBoxLocal.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblRealTimeStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 539);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";
            // 
            // lblRealTimeStatus
            // 
            this.lblRealTimeStatus.Name = "lblRealTimeStatus";
            this.lblRealTimeStatus.Size = new System.Drawing.Size(730, 17);
            this.lblRealTimeStatus.Spring = true;
            this.lblRealTimeStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabLiveListening);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(494, 227);
            this.tabControl.TabIndex = 1;
            // 
            // tabLiveListening
            // 
            this.tabLiveListening.Controls.Add(this.groupBoxLiveMicrophones);
            this.tabLiveListening.Controls.Add(this.groupBoxLiveControls);
            this.tabLiveListening.Location = new System.Drawing.Point(4, 22);
            this.tabLiveListening.Name = "tabLiveListening";
            this.tabLiveListening.Padding = new System.Windows.Forms.Padding(3);
            this.tabLiveListening.Size = new System.Drawing.Size(486, 201);
            this.tabLiveListening.TabIndex = 0;
            this.tabLiveListening.Text = "Microphone Control";
            this.tabLiveListening.UseVisualStyleBackColor = true;
            // 
            // tabRecording
            // 
            this.tabRecording.Controls.Add(this.groupBoxSettings);
            this.tabRecording.Controls.Add(this.groupBoxMicrophones);
            this.tabRecording.Controls.Add(this.groupBoxControls);
            this.tabRecording.Location = new System.Drawing.Point(4, 22);
            this.tabRecording.Name = "tabRecording";
            this.tabRecording.Padding = new System.Windows.Forms.Padding(3);
            this.tabRecording.Size = new System.Drawing.Size(776, 513);
            this.tabRecording.TabIndex = 0;
            this.tabRecording.Text = "Recording";
            this.tabRecording.UseVisualStyleBackColor = true;
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSettings.Controls.Add(this.numChunkDuration);
            this.groupBoxSettings.Controls.Add(this.lblChunkDuration);
            this.groupBoxSettings.Controls.Add(this.numQuality);
            this.groupBoxSettings.Controls.Add(this.lblQuality);
            this.groupBoxSettings.Location = new System.Drawing.Point(8, 125);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(760, 120);
            this.groupBoxSettings.TabIndex = 2;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Recording Settings";
            // 
            // numChunkDuration
            // 
            this.numChunkDuration.Location = new System.Drawing.Point(150, 85);
            this.numChunkDuration.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numChunkDuration.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numChunkDuration.Name = "numChunkDuration";
            this.numChunkDuration.Size = new System.Drawing.Size(120, 20);
            this.numChunkDuration.TabIndex = 4;
            this.numChunkDuration.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // lblChunkDuration
            // 
            this.lblChunkDuration.AutoSize = true;
            this.lblChunkDuration.Location = new System.Drawing.Point(15, 87);
            this.lblChunkDuration.Name = "lblChunkDuration";
            this.lblChunkDuration.Size = new System.Drawing.Size(129, 13);
            this.lblChunkDuration.TabIndex = 3;
            this.lblChunkDuration.Text = "Chunk Duration (seconds):";
            // 
            // numQuality
            // 
            this.numQuality.Increment = new decimal(new int[] {
            11025,
            0,
            0,
            0});
            this.numQuality.Location = new System.Drawing.Point(150, 55);
            this.numQuality.Maximum = new decimal(new int[] {
            48000,
            0,
            0,
            0});
            this.numQuality.Minimum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.numQuality.Name = "numQuality";
            this.numQuality.Size = new System.Drawing.Size(120, 20);
            this.numQuality.TabIndex = 2;
            this.numQuality.Value = new decimal(new int[] {
            44100,
            0,
            0,
            0});
            // 
            // lblQuality
            // 
            this.lblQuality.AutoSize = true;
            this.lblQuality.Location = new System.Drawing.Point(15, 57);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(96, 13);
            this.lblQuality.TabIndex = 1;
            this.lblQuality.Text = "Sample Rate (Hz):";
            // 
            // groupBoxMicrophones
            // 
            this.groupBoxMicrophones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMicrophones.Controls.Add(this.btnRefreshMicrophones);
            this.groupBoxMicrophones.Controls.Add(this.cmbMicrophones);
            this.groupBoxMicrophones.Controls.Add(this.lblMicrophones);
            this.groupBoxMicrophones.Location = new System.Drawing.Point(8, 6);
            this.groupBoxMicrophones.Name = "groupBoxMicrophones";
            this.groupBoxMicrophones.Size = new System.Drawing.Size(760, 113);
            this.groupBoxMicrophones.TabIndex = 1;
            this.groupBoxMicrophones.TabStop = false;
            this.groupBoxMicrophones.Text = "Microphone Selection";
            // 
            // btnRefreshMicrophones
            // 
            this.btnRefreshMicrophones.Location = new System.Drawing.Point(18, 70);
            this.btnRefreshMicrophones.Name = "btnRefreshMicrophones";
            this.btnRefreshMicrophones.Size = new System.Drawing.Size(150, 30);
            this.btnRefreshMicrophones.TabIndex = 2;
            this.btnRefreshMicrophones.Text = "Refresh Microphones";
            this.btnRefreshMicrophones.UseVisualStyleBackColor = true;
            this.btnRefreshMicrophones.Click += new System.EventHandler(this.btnRefreshMicrophones_Click);
            // 
            // cmbMicrophones
            // 
            this.cmbMicrophones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMicrophones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMicrophones.FormattingEnabled = true;
            this.cmbMicrophones.Location = new System.Drawing.Point(18, 40);
            this.cmbMicrophones.Name = "cmbMicrophones";
            this.cmbMicrophones.Size = new System.Drawing.Size(720, 21);
            this.cmbMicrophones.TabIndex = 1;
            // 
            // lblMicrophones
            // 
            this.lblMicrophones.AutoSize = true;
            this.lblMicrophones.Location = new System.Drawing.Point(15, 24);
            this.lblMicrophones.Name = "lblMicrophones";
            this.lblMicrophones.Size = new System.Drawing.Size(117, 13);
            this.lblMicrophones.TabIndex = 0;
            this.lblMicrophones.Text = "Available Microphones:";
            // 
            // groupBoxControls
            // 
            this.groupBoxControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxControls.Controls.Add(this.btnStopRecording);
            this.groupBoxControls.Controls.Add(this.btnStartRecording);
            this.groupBoxControls.Location = new System.Drawing.Point(8, 251);
            this.groupBoxControls.Name = "groupBoxControls";
            this.groupBoxControls.Size = new System.Drawing.Size(760, 80);
            this.groupBoxControls.TabIndex = 0;
            this.groupBoxControls.TabStop = false;
            this.groupBoxControls.Text = "Recording Controls";
            // 
            // btnStopRecording
            // 
            this.btnStopRecording.Enabled = false;
            this.btnStopRecording.Location = new System.Drawing.Point(174, 30);
            this.btnStopRecording.Name = "btnStopRecording";
            this.btnStopRecording.Size = new System.Drawing.Size(150, 35);
            this.btnStopRecording.TabIndex = 1;
            this.btnStopRecording.Text = "Stop Recording";
            this.btnStopRecording.UseVisualStyleBackColor = true;
            // 
            // btnStartRecording
            // 
            this.btnStartRecording.Enabled = false;
            this.btnStartRecording.Location = new System.Drawing.Point(18, 30);
            this.btnStartRecording.Name = "btnStartRecording";
            this.btnStartRecording.Size = new System.Drawing.Size(150, 35);
            this.btnStartRecording.TabIndex = 0;
            this.btnStartRecording.Text = "Start Recording";
            this.btnStartRecording.UseVisualStyleBackColor = true;
            // 
            // tabAudioLogs
            // 
            this.tabAudioLogs.Controls.Add(this.splitContainer);
            this.tabAudioLogs.Location = new System.Drawing.Point(4, 22);
            this.tabAudioLogs.Name = "tabAudioLogs";
            this.tabAudioLogs.Padding = new System.Windows.Forms.Padding(3);
            this.tabAudioLogs.Size = new System.Drawing.Size(776, 513);
            this.tabAudioLogs.TabIndex = 1;
            this.tabAudioLogs.Text = "Audio Logs";
            this.tabAudioLogs.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(3, 3);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.groupBoxRemote);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.groupBoxLocal);
            this.splitContainer.Size = new System.Drawing.Size(770, 507);
            this.splitContainer.SplitterDistance = 385;
            this.splitContainer.TabIndex = 0;
            // 
            // groupBoxRemote
            // 
            this.groupBoxRemote.Controls.Add(this.btnGetAudioLogs);
            this.groupBoxRemote.Controls.Add(this.lstRemoteAudioFiles);
            this.groupBoxRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxRemote.Location = new System.Drawing.Point(0, 0);
            this.groupBoxRemote.Name = "groupBoxRemote";
            this.groupBoxRemote.Size = new System.Drawing.Size(385, 507);
            this.groupBoxRemote.TabIndex = 0;
            this.groupBoxRemote.TabStop = false;
            this.groupBoxRemote.Text = "Remote Audio Files (Double-click to download)";
            // 
            // btnGetAudioLogs
            // 
            this.btnGetAudioLogs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetAudioLogs.Location = new System.Drawing.Point(6, 471);
            this.btnGetAudioLogs.Name = "btnGetAudioLogs";
            this.btnGetAudioLogs.Size = new System.Drawing.Size(373, 30);
            this.btnGetAudioLogs.TabIndex = 1;
            this.btnGetAudioLogs.Text = "Get Audio Logs";
            this.btnGetAudioLogs.UseVisualStyleBackColor = true;
            // 
            // lstRemoteAudioFiles
            // 
            this.lstRemoteAudioFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstRemoteAudioFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRemoteFileName,
            this.colRemoteFileSize,
            this.colRemoteFileDate});
            this.lstRemoteAudioFiles.FullRowSelect = true;
            this.lstRemoteAudioFiles.GridLines = true;
            this.lstRemoteAudioFiles.Location = new System.Drawing.Point(6, 19);
            this.lstRemoteAudioFiles.Name = "lstRemoteAudioFiles";
            this.lstRemoteAudioFiles.Size = new System.Drawing.Size(373, 446);
            this.lstRemoteAudioFiles.TabIndex = 0;
            this.lstRemoteAudioFiles.UseCompatibleStateImageBehavior = false;
            this.lstRemoteAudioFiles.View = System.Windows.Forms.View.Details;
            // 
            // colRemoteFileName
            // 
            this.colRemoteFileName.Text = "File Name";
            this.colRemoteFileName.Width = 180;
            // 
            // colRemoteFileSize
            // 
            this.colRemoteFileSize.Text = "Size";
            this.colRemoteFileSize.Width = 80;
            // 
            // colRemoteFileDate
            // 
            this.colRemoteFileDate.Text = "Date";
            this.colRemoteFileDate.Width = 110;
            // 
            // groupBoxLocal
            // 
            this.groupBoxLocal.Controls.Add(this.btnRefreshLocal);
            this.groupBoxLocal.Controls.Add(this.lstLocalAudioFiles);
            this.groupBoxLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxLocal.Location = new System.Drawing.Point(0, 0);
            this.groupBoxLocal.Name = "groupBoxLocal";
            this.groupBoxLocal.Size = new System.Drawing.Size(381, 507);
            this.groupBoxLocal.TabIndex = 0;
            this.groupBoxLocal.TabStop = false;
            this.groupBoxLocal.Text = "Local Audio Files (Double-click to play)";
            // 
            // btnRefreshLocal
            // 
            this.btnRefreshLocal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshLocal.Location = new System.Drawing.Point(6, 471);
            this.btnRefreshLocal.Name = "btnRefreshLocal";
            this.btnRefreshLocal.Size = new System.Drawing.Size(369, 30);
            this.btnRefreshLocal.TabIndex = 1;
            this.btnRefreshLocal.Text = "Refresh Local Files";
            this.btnRefreshLocal.UseVisualStyleBackColor = true;
            // 
            // lstLocalAudioFiles
            // 
            this.lstLocalAudioFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstLocalAudioFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colLocalFileName,
            this.colLocalFileSize,
            this.colLocalFileDate});
            this.lstLocalAudioFiles.FullRowSelect = true;
            this.lstLocalAudioFiles.GridLines = true;
            this.lstLocalAudioFiles.Location = new System.Drawing.Point(6, 19);
            this.lstLocalAudioFiles.Name = "lstLocalAudioFiles";
            this.lstLocalAudioFiles.Size = new System.Drawing.Size(369, 446);
            this.lstLocalAudioFiles.TabIndex = 0;
            this.lstLocalAudioFiles.UseCompatibleStateImageBehavior = false;
            this.lstLocalAudioFiles.View = System.Windows.Forms.View.Details;
            // 
            // colLocalFileName
            // 
            this.colLocalFileName.Text = "File Name";
            this.colLocalFileName.Width = 180;
            // 
            // colLocalFileSize
            // 
            this.colLocalFileSize.Text = "Size";
            this.colLocalFileSize.Width = 80;
            // 
            // colLocalFileDate
            // 
            this.colLocalFileDate.Text = "Date";
            this.colLocalFileDate.Width = 106;
            // 
            // FrmMicrophoneListener
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 165);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Controls.Add(this.groupBoxLiveMicrophones);
            this.Controls.Add(this.groupBoxLiveControls);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMicrophoneListener";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Microphone Listener []";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMicrophoneListener_FormClosing);
            this.Load += new System.EventHandler(this.FrmMicrophoneListener_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabRecording.ResumeLayout(false);
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numChunkDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuality)).EndInit();
            this.groupBoxMicrophones.ResumeLayout(false);
            this.groupBoxMicrophones.PerformLayout();
            this.groupBoxControls.ResumeLayout(false);
            this.tabAudioLogs.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            // 
            // groupBoxLiveControls
            // 
            this.groupBoxLiveControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLiveControls.Controls.Add(this.btnStartRecording);
            this.groupBoxLiveControls.Controls.Add(this.btnStartListening);
            this.groupBoxLiveControls.Location = new System.Drawing.Point(12, 75);
            this.groupBoxLiveControls.Name = "groupBoxLiveControls";
            this.groupBoxLiveControls.Size = new System.Drawing.Size(276, 60);
            this.groupBoxLiveControls.TabIndex = 3;
            this.groupBoxLiveControls.TabStop = false;
            this.groupBoxLiveControls.Text = "Microphone Controls";
            // 
            // btnStopListening
            // 
            this.btnStopListening.Enabled = false;
            this.btnStopListening.Location = new System.Drawing.Point(110, 19);
            this.btnStopListening.Name = "btnStopListening";
            this.btnStopListening.Size = new System.Drawing.Size(98, 30);
            this.btnStopListening.TabIndex = 1;
            this.btnStopListening.Text = "Stop Listening";
            this.btnStopListening.UseVisualStyleBackColor = true;
            // 
            // btnStartListening
            // 
            this.btnStartListening.Enabled = false;
            this.btnStartListening.Location = new System.Drawing.Point(38, 25);
            this.btnStartListening.Name = "btnStartListening";
            this.btnStartListening.Size = new System.Drawing.Size(100, 30);
            this.btnStartListening.TabIndex = 0;
            this.btnStartListening.Text = "Start Listening";
            this.btnStartListening.UseVisualStyleBackColor = true;
            this.btnStartListening.Click += new System.EventHandler(this.btnToggleListening_Click);
            // 
            // btnStartRecording
            // 
            this.btnStartRecording.Enabled = false;
            this.btnStartRecording.Location = new System.Drawing.Point(148, 25);
            this.btnStartRecording.Name = "btnStartRecording";
            this.btnStartRecording.Size = new System.Drawing.Size(100, 30);
            this.btnStartRecording.TabIndex = 1;
            this.btnStartRecording.Text = "Start Recording";
            this.btnStartRecording.UseVisualStyleBackColor = true;
            this.btnStartRecording.Click += new System.EventHandler(this.btnToggleRecording_Click);
            // 
            // btnStopRecording
            // 
            this.btnStopRecording.Enabled = false;
            this.btnStopRecording.Location = new System.Drawing.Point(110, 55);
            this.btnStopRecording.Name = "btnStopRecording";
            this.btnStopRecording.Size = new System.Drawing.Size(98, 30);
            this.btnStopRecording.TabIndex = 3;
            this.btnStopRecording.Text = "Stop Recording";
            this.btnStopRecording.UseVisualStyleBackColor = true;
            // 
            // groupBoxLiveMicrophones
            // 
            this.groupBoxLiveMicrophones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLiveMicrophones.Controls.Add(this.cmbLiveMicrophones);
            this.groupBoxLiveMicrophones.Location = new System.Drawing.Point(12, 12);
            this.groupBoxLiveMicrophones.Name = "groupBoxLiveMicrophones";
            this.groupBoxLiveMicrophones.Size = new System.Drawing.Size(276, 55);
            this.groupBoxLiveMicrophones.TabIndex = 1;
            this.groupBoxLiveMicrophones.TabStop = false;
            this.groupBoxLiveMicrophones.Text = "Microphone Selection";
            // 
            // btnRefreshLiveMicrophones
            // 
            this.btnRefreshLiveMicrophones.Location = new System.Drawing.Point(320, 40);
            this.btnRefreshLiveMicrophones.Name = "btnRefreshLiveMicrophones";
            this.btnRefreshLiveMicrophones.Size = new System.Drawing.Size(120, 25);
            this.btnRefreshLiveMicrophones.TabIndex = 2;
            this.btnRefreshLiveMicrophones.Text = "Refresh Microphones";
            this.btnRefreshLiveMicrophones.UseVisualStyleBackColor = true;
            // 
            // cmbLiveMicrophones
            // 
            this.cmbLiveMicrophones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLiveMicrophones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLiveMicrophones.FormattingEnabled = true;
            this.cmbLiveMicrophones.Location = new System.Drawing.Point(6, 25);
            this.cmbLiveMicrophones.Name = "cmbLiveMicrophones";
            this.cmbLiveMicrophones.Size = new System.Drawing.Size(264, 21);
            this.cmbLiveMicrophones.TabIndex = 1;
            // 
            // lblLiveMicrophones
            // 
            this.lblLiveMicrophones.AutoSize = true;
            this.lblLiveMicrophones.Location = new System.Drawing.Point(6, 25);
            this.lblLiveMicrophones.Name = "lblLiveMicrophones";
            this.lblLiveMicrophones.Size = new System.Drawing.Size(125, 13);
            this.lblLiveMicrophones.TabIndex = 0;
            this.lblLiveMicrophones.Text = "Available Microphones:";
            // 
            // groupBoxLiveSettings
            // 
            this.groupBoxLiveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLiveSettings.Controls.Add(this.numLiveQuality);
            this.groupBoxLiveSettings.Controls.Add(this.lblLiveQuality);
            this.groupBoxLiveSettings.Location = new System.Drawing.Point(8, 92);
            this.groupBoxLiveSettings.Name = "groupBoxLiveSettings";
            this.groupBoxLiveSettings.Size = new System.Drawing.Size(470, 70);
            this.groupBoxLiveSettings.TabIndex = 2;
            this.groupBoxLiveSettings.TabStop = false;
            this.groupBoxLiveSettings.Text = "Audio Settings";
            // 
            // numLiveQuality
            // 
            this.numLiveQuality.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numLiveQuality.Location = new System.Drawing.Point(6, 45);
            this.numLiveQuality.Maximum = new decimal(new int[] {
            48000,
            0,
            0,
            0});
            this.numLiveQuality.Minimum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.numLiveQuality.Name = "numLiveQuality";
            this.numLiveQuality.Size = new System.Drawing.Size(120, 22);
            this.numLiveQuality.TabIndex = 1;
            this.numLiveQuality.Value = new decimal(new int[] {
            44100,
            0,
            0,
            0});
            // 
            // lblLiveQuality
            // 
            this.lblLiveQuality.AutoSize = true;
            this.lblLiveQuality.Location = new System.Drawing.Point(6, 25);
            this.lblLiveQuality.Name = "lblLiveQuality";
            this.lblLiveQuality.Size = new System.Drawing.Size(89, 13);
            this.lblLiveQuality.TabIndex = 0;
            this.lblLiveQuality.Text = "Sample Rate (Hz):";
            // 
            // lblChunkDuration
            // 
            this.lblChunkDuration.AutoSize = true;
            this.lblChunkDuration.Location = new System.Drawing.Point(150, 25);
            this.lblChunkDuration.Name = "lblChunkDuration";
            this.lblChunkDuration.Size = new System.Drawing.Size(125, 13);
            this.lblChunkDuration.TabIndex = 2;
            this.lblChunkDuration.Text = "Chunk Duration (seconds):";
            // 
            // numChunkDuration
            // 
            this.numChunkDuration.Location = new System.Drawing.Point(150, 45);
            this.numChunkDuration.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numChunkDuration.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numChunkDuration.Name = "numChunkDuration";
            this.numChunkDuration.Size = new System.Drawing.Size(120, 22);
            this.numChunkDuration.TabIndex = 3;
            this.numChunkDuration.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.groupBoxRemote.ResumeLayout(false);
            this.groupBoxLocal.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblRealTimeStatus;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabLiveListening;
        private System.Windows.Forms.TabPage tabRecording;
        private System.Windows.Forms.TabPage tabAudioLogs;
        private System.Windows.Forms.GroupBox groupBoxControls;
        private System.Windows.Forms.Button btnStopRecording;
        private System.Windows.Forms.Button btnStartRecording;
        private System.Windows.Forms.GroupBox groupBoxMicrophones;
        private System.Windows.Forms.ComboBox cmbMicrophones;
        private System.Windows.Forms.Label lblMicrophones;
        private System.Windows.Forms.Button btnRefreshMicrophones;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.NumericUpDown numQuality;
        private System.Windows.Forms.Label lblQuality;
        private System.Windows.Forms.NumericUpDown numChunkDuration;
        private System.Windows.Forms.Label lblChunkDuration;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox groupBoxRemote;
        private System.Windows.Forms.ListView lstRemoteAudioFiles;
        private System.Windows.Forms.ColumnHeader colRemoteFileName;
        private System.Windows.Forms.ColumnHeader colRemoteFileSize;
        private System.Windows.Forms.ColumnHeader colRemoteFileDate;
        private System.Windows.Forms.Button btnGetAudioLogs;
        private System.Windows.Forms.GroupBox groupBoxLocal;
        private System.Windows.Forms.ListView lstLocalAudioFiles;
        private System.Windows.Forms.ColumnHeader colLocalFileName;
        private System.Windows.Forms.ColumnHeader colLocalFileSize;
        private System.Windows.Forms.ColumnHeader colLocalFileDate;
        private System.Windows.Forms.Button btnRefreshLocal;
        private System.Windows.Forms.GroupBox groupBoxLiveControls;
        private System.Windows.Forms.Button btnStopListening;
        private System.Windows.Forms.Button btnStartListening;
        private System.Windows.Forms.GroupBox groupBoxLiveMicrophones;
        private System.Windows.Forms.Button btnRefreshLiveMicrophones;
        private System.Windows.Forms.ComboBox cmbLiveMicrophones;
        private System.Windows.Forms.Label lblLiveMicrophones;
        private System.Windows.Forms.GroupBox groupBoxLiveSettings;
        private System.Windows.Forms.NumericUpDown numLiveQuality;
        private System.Windows.Forms.Label lblLiveQuality;
    }
}
