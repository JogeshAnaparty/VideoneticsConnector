
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using log4net.Config;

namespace VideoStreamTester
{
    using System;
    using System.Text;
    using System.Windows.Forms;
    using JsonConfig;

    public partial class MainView : Form
    {


        const string FFmpegParamsSeparator = "\r\n";

        enum FFmpegInputFormat { Jpg, Rtsp };

        private ProcessManager processManager;
        protected CancellationTokenSource cancellationTokenSource;
        private Task streamTask;

        #region Entry Point

        /// <summary>
        /// Initializes a new instance of the <see cref="MainView"/> class.
        /// </summary>
        public MainView()
        {
            InitializeComponent();
            Program.Log.Debug("Starting Video Stream Tester Application...");
            Initialize();
        }


        #endregion

        #region UI Event handlers


        /// <summary>
        /// This method is called everytime a setting changes
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void StreamerDataChanged(object sender, EventArgs e)
        {

            //const string paramsSeparator = " ";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"-framerate {NumInputFps.Value}" + FFmpegParamsSeparator);
            stringBuilder.Append("-f image2pipe -vcodec mjpeg -i - -an"+ FFmpegParamsSeparator);
            stringBuilder.Append(TxtOutVideoCodec.Text + FFmpegParamsSeparator);
            stringBuilder.Append(TxtOutVideoSize.Text + FFmpegParamsSeparator);
            stringBuilder.Append(TxtOutVideoFps.Text + FFmpegParamsSeparator);
            stringBuilder.Append(TxtOutVideoBps.Text + FFmpegParamsSeparator);
            stringBuilder.Append(TxtOutPreset.Text + FFmpegParamsSeparator);
            stringBuilder.Append(TxtOutTune.Text + FFmpegParamsSeparator);
            stringBuilder.Append(TxtOutx264Params.Text + FFmpegParamsSeparator);
            stringBuilder.Append(TxtOutFormat.Text + FFmpegParamsSeparator);
            stringBuilder.Append($"{TxtMediaProtocol.Text}://{TxtMediaServer.Text}/{TxtMediaApplication.Text}/{TxtMediaFile.Text}" + FFmpegParamsSeparator);
            TxtFFmpegCommandLine.Text = stringBuilder.ToString();
        }


        private void BtnBrowseFrameFolder_Click(object sender, EventArgs e)
        {
            if (FBDFramesFolder.ShowDialog(this) == DialogResult.OK)
            {
                TxtInputFramesFolder.Text = FBDFramesFolder.SelectedPath;
            }
        }

        private void BtnStartFFmpeg_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            streamTask = Task.Factory.StartNew(DoStream, cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void DoStream()
        {
            //TODO: allow streaming from camera also...
            var inputType = FFmpegInputFormat.Jpg;
            processManager = new ProcessManager(inputType == FFmpegInputFormat.Jpg);
            processManager.Exit += ProcessManagerExit;

            switch (inputType)
            {
                case FFmpegInputFormat.Jpg:

                    #region Jpg


                    if (string.IsNullOrEmpty(TxtInputFramesFolder.Text))
                    {
                        //TODO: show error
                        Program.Log.Warn("TxtInputFramesFolder not set...");
                        return;
                    }

                    //Files are sorted by name
                    string[] imagesFileNames = Directory.GetFiles(TxtInputFramesFolder.Text);

                    if (imagesFileNames.Length == 0)
                    {
                        //TODO: show error
                        Program.Log.Warn("Images directory empty...");
                        return;
                    }
					
					processManager.StartProcess("ffmpeg.exe", TxtFFmpegCommandLine.Text.Replace(FFmpegParamsSeparator, " "));

					StartMjpegStream(imagesFileNames);
					
					processManager.StopProcess();

                    #endregion

                    break;
                case FFmpegInputFormat.Rtsp:
                    throw new NotImplementedException();
            }
            processManager.Exit -= ProcessManagerExit;
        }

		private void StartMjpegStream(string[] imagesFileNames)
		{
			Stopwatch sw = new Stopwatch();
			foreach (string image in imagesFileNames)
			{

				if (cancellationTokenSource.IsCancellationRequested)
				{
					processManager.StopProcess();
					return;
				}

				sw.Start();

				//Write jpg image into the pipe
				using (Bitmap img = (Bitmap)Image.FromFile(image))
				{
					var width = img.Width;
					var height = img.Height;
					Invoke((Action)(() =>
					{
						PBCurrentFrame.Image = Image.FromFile(image);

					}));
					using (MemoryStream ms = new MemoryStream())
					{
						img.Save(ms, ImageFormat.Jpeg);
						int writtenDataSize = processManager.WriteDataIntoPipe(ms.ToArray(), (int)ms.Length);
						if (writtenDataSize != ms.Length)
						{
							Program.Log.Error("Warning: data write failed!");
							processManager.StopProcess();
							return;
						}

					}
					uint framePeriod = (uint)(1000 / (double)NumInputFps.Value);
					var rest = framePeriod - sw.ElapsedMilliseconds;
					if (rest > 0)
						Thread.Sleep((int)rest);

					sw.Reset();
				}
			}
			if (ChkLoopStream.Checked)
				StartMjpegStream(imagesFileNames);
		}
			private void BtnStopFFmpeg_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
            //processManager.StopProcess();
        }

        private void ProcessManagerExit(object sender, ProcessManagerEventArgs e)
        {
            Program.Log.Debug($"FFmpeg Process {e.Process.Id} Exit:\r\n{processManager.Output}");
            ((ProcessManager)sender).Pipe?.Close();
        }

        #endregion

        #region Initialization


        private void Initialize()
        {
            //Load Settings
            TxtInputFramesFolder.Text = Config.Global.InputFramesFolder;
            NumInputFps.Value = (int)Config.Global.InputFps;
            TxtMediaProtocol.Text = Config.Global.MediaProtocol;
            TxtMediaServer.Text = Config.Global.MediaServer;
            TxtMediaApplication.Text = Config.Global.MediaApplication;
            TxtMediaFile.Text = Config.Global.MediaFile;
            TxtOutVideoCodec.Text = Config.Global.OutVideoCodec;
            TxtOutVideoFps.Text = Config.Global.OutVideoFps;
            TxtOutVideoSize.Text = Config.Global.OutVideoSize;
            TxtOutVideoBps.Text = Config.Global.OutVideoBps;
            TxtOutPreset.Text = Config.Global.OutPreset;
            TxtOutTune.Text = Config.Global.OutTune;
            TxtOutx264Params.Text = Config.Global.Outx264Params;
            TxtOutFormat.Text = Config.Global.OutFormat;
        }




        #endregion


    }
}
