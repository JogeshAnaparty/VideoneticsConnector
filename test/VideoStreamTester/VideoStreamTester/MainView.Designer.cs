namespace VideoStreamTester
{
    partial class MainView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnBrowseFrameFolder = new System.Windows.Forms.Button();
            this.NumInputFps = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtInputFramesFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TxtOutVideoFps = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.TxtOutVideoCodec = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.ChkLoopStream = new System.Windows.Forms.CheckBox();
            this.BtnStopFFmpeg = new System.Windows.Forms.Button();
            this.BtnStartFFmpeg = new System.Windows.Forms.Button();
            this.TxtMediaProtocol = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.TxtOutFormat = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.TxtOutx264Params = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.TxtOutTune = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TxtOutPreset = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtOutVideoSize = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtOutVideoBps = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtMediaFile = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtMediaApplication = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtMediaServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtFFmpegCommandLine = new System.Windows.Forms.TextBox();
            this.FBDFramesFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.PBCurrentFrame = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumInputFps)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBCurrentFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.windowToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(1311, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(89, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 22);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logToolStripMenuItem,
            this.logToolStripMenuItem1});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 22);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
            this.logToolStripMenuItem.Text = "Video";
            // 
            // logToolStripMenuItem1
            // 
            this.logToolStripMenuItem1.Name = "logToolStripMenuItem1";
            this.logToolStripMenuItem1.Size = new System.Drawing.Size(104, 22);
            this.logToolStripMenuItem1.Text = "Log";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnBrowseFrameFolder);
            this.groupBox1.Controls.Add(this.NumInputFps);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.TxtInputFramesFolder);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(8, 23);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(431, 81);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Settings";
            // 
            // BtnBrowseFrameFolder
            // 
            this.BtnBrowseFrameFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnBrowseFrameFolder.Location = new System.Drawing.Point(369, 22);
            this.BtnBrowseFrameFolder.Margin = new System.Windows.Forms.Padding(2);
            this.BtnBrowseFrameFolder.Name = "BtnBrowseFrameFolder";
            this.BtnBrowseFrameFolder.Size = new System.Drawing.Size(32, 22);
            this.BtnBrowseFrameFolder.TabIndex = 6;
            this.BtnBrowseFrameFolder.Text = "...";
            this.BtnBrowseFrameFolder.UseVisualStyleBackColor = true;
            this.BtnBrowseFrameFolder.Click += new System.EventHandler(this.BtnBrowseFrameFolder_Click);
            // 
            // NumInputFps
            // 
            this.NumInputFps.Location = new System.Drawing.Point(91, 49);
            this.NumInputFps.Margin = new System.Windows.Forms.Padding(2);
            this.NumInputFps.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.NumInputFps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumInputFps.Name = "NumInputFps";
            this.NumInputFps.Size = new System.Drawing.Size(80, 20);
            this.NumInputFps.TabIndex = 4;
            this.NumInputFps.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.NumInputFps.ValueChanged += new System.EventHandler(this.StreamerDataChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Fps";
            // 
            // TxtInputFramesFolder
            // 
            this.TxtInputFramesFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtInputFramesFolder.Location = new System.Drawing.Point(91, 23);
            this.TxtInputFramesFolder.Margin = new System.Windows.Forms.Padding(2);
            this.TxtInputFramesFolder.Name = "TxtInputFramesFolder";
            this.TxtInputFramesFolder.Size = new System.Drawing.Size(274, 20);
            this.TxtInputFramesFolder.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Frames folder";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TxtOutVideoFps);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.TxtOutVideoCodec);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.ChkLoopStream);
            this.groupBox2.Controls.Add(this.BtnStopFFmpeg);
            this.groupBox2.Controls.Add(this.BtnStartFFmpeg);
            this.groupBox2.Controls.Add(this.TxtMediaProtocol);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.checkBox6);
            this.groupBox2.Controls.Add(this.checkBox5);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.TxtOutFormat);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.TxtOutx264Params);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.TxtOutTune);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.TxtOutPreset);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.TxtOutVideoSize);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.TxtOutVideoBps);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.TxtMediaFile);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.TxtMediaApplication);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.TxtMediaServer);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(8, 108);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(431, 357);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output Settings";
            // 
            // TxtOutVideoFps
            // 
            this.TxtOutVideoFps.Location = new System.Drawing.Point(90, 171);
            this.TxtOutVideoFps.Margin = new System.Windows.Forms.Padding(2);
            this.TxtOutVideoFps.Name = "TxtOutVideoFps";
            this.TxtOutVideoFps.Size = new System.Drawing.Size(308, 20);
            this.TxtOutVideoFps.TabIndex = 32;
            this.TxtOutVideoFps.Text = "-r 24";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(4, 174);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 13);
            this.label14.TabIndex = 31;
            this.label14.Text = "Output Fps";
            // 
            // TxtOutVideoCodec
            // 
            this.TxtOutVideoCodec.Location = new System.Drawing.Point(90, 111);
            this.TxtOutVideoCodec.Margin = new System.Windows.Forms.Padding(2);
            this.TxtOutVideoCodec.Name = "TxtOutVideoCodec";
            this.TxtOutVideoCodec.Size = new System.Drawing.Size(308, 20);
            this.TxtOutVideoCodec.TabIndex = 30;
            this.TxtOutVideoCodec.Text = "-c:v libx264 -pix_fmt yuv420p";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(2, 121);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 13);
            this.label13.TabIndex = 29;
            this.label13.Text = "Video Codec";
            // 
            // ChkLoopStream
            // 
            this.ChkLoopStream.AutoSize = true;
            this.ChkLoopStream.Checked = true;
            this.ChkLoopStream.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkLoopStream.Location = new System.Drawing.Point(7, 326);
            this.ChkLoopStream.Name = "ChkLoopStream";
            this.ChkLoopStream.Size = new System.Drawing.Size(84, 17);
            this.ChkLoopStream.TabIndex = 28;
            this.ChkLoopStream.Text = "Loop stream";
            this.ChkLoopStream.UseVisualStyleBackColor = true;
            // 
            // BtnStopFFmpeg
            // 
            this.BtnStopFFmpeg.Location = new System.Drawing.Point(250, 309);
            this.BtnStopFFmpeg.Name = "BtnStopFFmpeg";
            this.BtnStopFFmpeg.Size = new System.Drawing.Size(150, 34);
            this.BtnStopFFmpeg.TabIndex = 27;
            this.BtnStopFFmpeg.Text = "Stop FFmpeg";
            this.BtnStopFFmpeg.UseVisualStyleBackColor = true;
            this.BtnStopFFmpeg.Click += new System.EventHandler(this.BtnStopFFmpeg_Click);
            // 
            // BtnStartFFmpeg
            // 
            this.BtnStartFFmpeg.Location = new System.Drawing.Point(91, 309);
            this.BtnStartFFmpeg.Name = "BtnStartFFmpeg";
            this.BtnStartFFmpeg.Size = new System.Drawing.Size(153, 34);
            this.BtnStartFFmpeg.TabIndex = 26;
            this.BtnStartFFmpeg.Text = "Start FFmpeg";
            this.BtnStartFFmpeg.UseVisualStyleBackColor = true;
            this.BtnStartFFmpeg.Click += new System.EventHandler(this.BtnStartFFmpeg_Click);
            // 
            // TxtMediaProtocol
            // 
            this.TxtMediaProtocol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtMediaProtocol.Location = new System.Drawing.Point(92, 24);
            this.TxtMediaProtocol.Margin = new System.Windows.Forms.Padding(2);
            this.TxtMediaProtocol.Name = "TxtMediaProtocol";
            this.TxtMediaProtocol.Size = new System.Drawing.Size(309, 20);
            this.TxtMediaProtocol.TabIndex = 25;
            this.TxtMediaProtocol.Text = "rtsp";
            this.TxtMediaProtocol.TextChanged += new System.EventHandler(this.StreamerDataChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 28);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "Protcol";
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(404, 266);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(15, 14);
            this.checkBox6.TabIndex = 23;
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(404, 248);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(15, 14);
            this.checkBox5.TabIndex = 22;
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(404, 227);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(15, 14);
            this.checkBox4.TabIndex = 21;
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(404, 207);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(15, 14);
            this.checkBox3.TabIndex = 20;
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(405, 154);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 19;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // TxtOutFormat
            // 
            this.TxtOutFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtOutFormat.Location = new System.Drawing.Point(91, 279);
            this.TxtOutFormat.Margin = new System.Windows.Forms.Padding(2);
            this.TxtOutFormat.Name = "TxtOutFormat";
            this.TxtOutFormat.Size = new System.Drawing.Size(309, 20);
            this.TxtOutFormat.TabIndex = 18;
            this.TxtOutFormat.Text = "-rtsp_transport tcp -f rtsp";
            this.TxtOutFormat.TextChanged += new System.EventHandler(this.StreamerDataChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 288);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "Format";
            // 
            // TxtOutx264Params
            // 
            this.TxtOutx264Params.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtOutx264Params.Location = new System.Drawing.Point(91, 258);
            this.TxtOutx264Params.Margin = new System.Windows.Forms.Padding(2);
            this.TxtOutx264Params.Name = "TxtOutx264Params";
            this.TxtOutx264Params.Size = new System.Drawing.Size(309, 20);
            this.TxtOutx264Params.TabIndex = 16;
            this.TxtOutx264Params.Text = "-x264-params keyint=48:min-keyint=48:no-scenecut";
            this.TxtOutx264Params.TextChanged += new System.EventHandler(this.StreamerDataChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 267);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "x264-params";
            // 
            // TxtOutTune
            // 
            this.TxtOutTune.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtOutTune.Location = new System.Drawing.Point(91, 237);
            this.TxtOutTune.Margin = new System.Windows.Forms.Padding(2);
            this.TxtOutTune.Name = "TxtOutTune";
            this.TxtOutTune.Size = new System.Drawing.Size(309, 20);
            this.TxtOutTune.TabIndex = 14;
            this.TxtOutTune.Text = "-tune zerolatency";
            this.TxtOutTune.TextChanged += new System.EventHandler(this.StreamerDataChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 246);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Tune";
            // 
            // TxtOutPreset
            // 
            this.TxtOutPreset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtOutPreset.Location = new System.Drawing.Point(91, 216);
            this.TxtOutPreset.Margin = new System.Windows.Forms.Padding(2);
            this.TxtOutPreset.Name = "TxtOutPreset";
            this.TxtOutPreset.Size = new System.Drawing.Size(309, 20);
            this.TxtOutPreset.TabIndex = 12;
            this.TxtOutPreset.Text = "-v:preset ultrafast";
            this.TxtOutPreset.TextChanged += new System.EventHandler(this.StreamerDataChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 225);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Preset";
            // 
            // TxtOutVideoSize
            // 
            this.TxtOutVideoSize.Location = new System.Drawing.Point(92, 146);
            this.TxtOutVideoSize.Margin = new System.Windows.Forms.Padding(2);
            this.TxtOutVideoSize.Name = "TxtOutVideoSize";
            this.TxtOutVideoSize.Size = new System.Drawing.Size(308, 20);
            this.TxtOutVideoSize.TabIndex = 10;
            this.TxtOutVideoSize.Text = "-s 1280x720";
            this.TxtOutVideoSize.TextChanged += new System.EventHandler(this.StreamerDataChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 156);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Size";
            // 
            // TxtOutVideoBps
            // 
            this.TxtOutVideoBps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtOutVideoBps.Location = new System.Drawing.Point(91, 195);
            this.TxtOutVideoBps.Margin = new System.Windows.Forms.Padding(2);
            this.TxtOutVideoBps.Name = "TxtOutVideoBps";
            this.TxtOutVideoBps.Size = new System.Drawing.Size(309, 20);
            this.TxtOutVideoBps.TabIndex = 8;
            this.TxtOutVideoBps.Text = "-b:v 255k -maxrate 250k -bufsize 250k";
            this.TxtOutVideoBps.TextChanged += new System.EventHandler(this.StreamerDataChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 204);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Bitrate";
            // 
            // TxtMediaFile
            // 
            this.TxtMediaFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtMediaFile.Location = new System.Drawing.Point(92, 87);
            this.TxtMediaFile.Margin = new System.Windows.Forms.Padding(2);
            this.TxtMediaFile.Name = "TxtMediaFile";
            this.TxtMediaFile.Size = new System.Drawing.Size(309, 20);
            this.TxtMediaFile.TabIndex = 6;
            this.TxtMediaFile.Text = "mystream";
            this.TxtMediaFile.TextChanged += new System.EventHandler(this.StreamerDataChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 94);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Media File";
            // 
            // TxtMediaApplication
            // 
            this.TxtMediaApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtMediaApplication.Location = new System.Drawing.Point(92, 66);
            this.TxtMediaApplication.Margin = new System.Windows.Forms.Padding(2);
            this.TxtMediaApplication.Name = "TxtMediaApplication";
            this.TxtMediaApplication.Size = new System.Drawing.Size(309, 20);
            this.TxtMediaApplication.TabIndex = 4;
            this.TxtMediaApplication.Text = "live";
            this.TxtMediaApplication.TextChanged += new System.EventHandler(this.StreamerDataChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 73);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Application";
            // 
            // TxtMediaServer
            // 
            this.TxtMediaServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtMediaServer.Location = new System.Drawing.Point(92, 45);
            this.TxtMediaServer.Margin = new System.Windows.Forms.Padding(2);
            this.TxtMediaServer.Name = "TxtMediaServer";
            this.TxtMediaServer.Size = new System.Drawing.Size(309, 20);
            this.TxtMediaServer.TabIndex = 2;
            this.TxtMediaServer.Text = "127.0.0.1:1935";
            this.TxtMediaServer.TextChanged += new System.EventHandler(this.StreamerDataChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 52);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Server";
            // 
            // TxtFFmpegCommandLine
            // 
            this.TxtFFmpegCommandLine.Location = new System.Drawing.Point(11, 469);
            this.TxtFFmpegCommandLine.Margin = new System.Windows.Forms.Padding(2);
            this.TxtFFmpegCommandLine.Multiline = true;
            this.TxtFFmpegCommandLine.Name = "TxtFFmpegCommandLine";
            this.TxtFFmpegCommandLine.Size = new System.Drawing.Size(428, 163);
            this.TxtFFmpegCommandLine.TabIndex = 4;
            // 
            // PBCurrentFrame
            // 
            this.PBCurrentFrame.BackColor = System.Drawing.Color.Black;
            this.PBCurrentFrame.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PBCurrentFrame.BackgroundImage")));
            this.PBCurrentFrame.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PBCurrentFrame.Location = new System.Drawing.Point(447, 29);
            this.PBCurrentFrame.Margin = new System.Windows.Forms.Padding(2);
            this.PBCurrentFrame.Name = "PBCurrentFrame";
            this.PBCurrentFrame.Size = new System.Drawing.Size(853, 480);
            this.PBCurrentFrame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PBCurrentFrame.TabIndex = 12;
            this.PBCurrentFrame.TabStop = false;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1311, 643);
            this.Controls.Add(this.PBCurrentFrame);
            this.Controls.Add(this.TxtFFmpegCommandLine);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "MainView";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HVS - Video Stream Tester";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumInputFps)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBCurrentFrame)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TxtInputFramesFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NumInputFps;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnBrowseFrameFolder;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox TxtMediaFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtMediaApplication;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtMediaServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtOutVideoBps;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TxtOutVideoSize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TxtOutPreset;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TxtOutTune;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TxtOutFormat;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TxtOutx264Params;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TxtFFmpegCommandLine;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem1;
        private System.Windows.Forms.FolderBrowserDialog FBDFramesFolder;
        private System.Windows.Forms.PictureBox PBCurrentFrame;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox TxtMediaProtocol;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button BtnStopFFmpeg;
        private System.Windows.Forms.Button BtnStartFFmpeg;
        private System.Windows.Forms.CheckBox ChkLoopStream;
        private System.Windows.Forms.TextBox TxtOutVideoCodec;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox TxtOutVideoFps;
        private System.Windows.Forms.Label label14;
    }
}

