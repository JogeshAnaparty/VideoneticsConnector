namespace HVS.Video.VMS.Tester
{
    using Application;
    using Data.Entities;
    using Data.Entities.Video;
    using ExtensionMethods;
    using MessageBrokers.RabbitMq;
    using Protocol;
    using Protocol.Video;
    using RabbitMQ.Client.Framing;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    public partial class Main : Form
    {
        #region Constants

        public const string VIDEO_CONNECTORS = "VideoConnectors";
        public const string CONFIGURATIONS = "Configurations";

        #region FFMPEG commands

        //public const string FFMPEGVideoLiveImage2PipeCommand = "ffmpeg -f image2pipe -vcodec mjpeg -i - -an -ss 1 -c:v libx264 -b:v 255k -maxrate 250k -bufsize 250k -tune zerolatency -x264opts keyint=120:min-keyint=1 -f flv rtmp://localhost:1935/live/{0}";
        //public const string FFMPEGVideoLiveImage2PipeCommand = "ffmpeg -f image2pipe -vcodec mjpeg -i - -an -c:v libx264  -tune zerolatency -rtsp_transport tcp -f rtsp rtsp://127.0.0.1:1935/live/{0}";
        public const string FFMPEGVideoLiveImage2PipeCommand = "ffmpeg -f image2pipe -vcodec mjpeg -i - -an -c:v libx264  -tune zerolatency -rtsp_transport tcp -f rtsp rtsp://{0}:{1}/live/{2}";
        //public const string FFMPEGVideoLiveImage2PipeCommand = "ffmpeg -f image2pipe -vcodec mjpeg -i - -an -ss 1 -c:v libx264 -preset ultrafast -b:v 400k -maxrate 800k -bufsize 1600k -tune zerolatency -x264opts keyint=120:min-keyint=30 -rtsp_transport tcp -f rtsp rtsp://192.168.1.154:1935/live/{0}";
        //public const string FFMPEGRTSPStreamingUrlCommand = "ffmpeg -i {0} -vcodec mjpeg -i - -an -c:v libx264 -maxrate 400k -bufsize 400k -f flv rtmp://localhost:1935/live/{1}";
        //public const string FFMPEGRTSPStreamingUrlCommand = "ffmpeg -rtsp_transport tcp -i rtsp://{0}:{1}@{2}:{3}/Streaming/Channels/101 -an -c:v libx264 -b:v 1000k -maxrate 1500k -bufsize 3000k -tune zerolatency -x264opts keyint=120:min-keyint=30  -f flv rtmp://localhost:1935/live/{4}";
        public const string FFMPEGRTSPStreamingUrlCommand = "ffmpeg -rtsp_transport tcp -i {0} -an -c:v libx264 -b:v 1000k -maxrate 1500k -bufsize 3000k -tune zerolatency -x264opts keyint=120:min-keyint=30  -f flv rtmp://localhost:1935/live/{1}";
        public const string FFMPEGVideoPlaybackImage2PipeCommand = "ffmpeg -f image2pipe -vcodec mjpeg -i - -an -c:v libx264 -maxrate 400k -bufsize 400k -f flv rtmp://localhost:1935/live/{0}";
        public const string FFMPEGVideoRecordImage2PipeCommand = "ffmpeg.exe -f image2pipe -vcodec mjpeg -i - -an -map 0 -vcodec libx264  -maxrate 400k -bufsize 400k -r 20 -f segment  -segment_list c:\\recorded\\{0}\\{1}\\{2}\\{3}\\playlist.m3u8 -segment_time 60  c:\\recorded\\{0}\\{1}\\{2}\\{3}\\%03d.mp4";

        #endregion

        #endregion

        #region Fields

        private VmsEntity vms;
        private TranscoderInputFormat transcoderInputFormat;
        private List<ComboBoxItem> currentLiveStreams;

        #endregion

        #region Properties

        public RabbitMQServiceBus ServiceBus
        {
            get { return serviceBus; }
        }

        public string DefaultExchangeName
        {
            get { return "hvs.automation"; }
        }
        public string ExchangeName
        {
            get { return string.Format("hvs.automation.domainid:{0}", vms.DomainId); }
        }

        public RabbitMQQueue Queue
        {
            get
            {
                return new RabbitMQQueue()
                {
                    Arguments = null,
                    AutoDelete = false,
                    Durable = false,
                    Exclusive = false,
                    Name = "HVS.Video.VMS.Tester"
                };
            }
        }

        public string RoutingKey
        {
            get { return string.Format("siteid:{0}.brandid:{1}", vms.SiteId, ((VideoConnector)LBTypes.SelectedItem).Value); }
        }

        public static RabbitMQServiceBus serviceBus;

        protected string liveTaskId = Guid.NewGuid().ToString("N");
        protected string recordTaskId = Guid.NewGuid().ToString("N");
        protected string playbackTaskId = Guid.NewGuid().ToString("N");
        protected string exportTaskId = Guid.NewGuid().ToString("N");

        #endregion

        public Main()
        {
            InitializeComponent();
            InitializeControls();
            InitializeServiceBus();

            currentLiveStreams = new List<ComboBoxItem>();
        }

        #region Event Handlers

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAddDevice_Click(object sender, EventArgs e)
        {
            TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.AddOrUpdateVMS, vms.DomainId, vms.SiteId, new VMSManagementArgs() { VMS = GetVMS() }));
            ServiceBus.BasicPublish(ExchangeName, RoutingKey, message, GetBasicProperties(message));
        }

        private void btnRemoveDevice_Click(object sender, EventArgs e)
        {
            TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.DeleteVMS, vms.DomainId, vms.SiteId, new VMSManagementArgs() { VMS = GetVMS() }));
            ServiceBus.BasicPublish(ExchangeName, RoutingKey, message, GetBasicProperties(message));
        }

        private void btnGetCameras_Click(object sender, EventArgs e)
        {
            LBCameras.Items.Clear();

            switch (transcoderInputFormat)
            {
                case TranscoderInputFormat.Jpg:
                    TxtIPCamStreamCommand.Text = FFMPEGVideoLiveImage2PipeCommand;
                    break;
                case TranscoderInputFormat.Rtsp:
                    TxtIPCamStreamCommand.Text = FFMPEGRTSPStreamingUrlCommand;
                    break;
            }

            TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.GetVMSCameras, vms.DomainId, new VMSManagementArgs() { VMS = GetVMS() }));
            ServiceBus.BasicPublish(DefaultExchangeName, RoutingKey, message, GetBasicProperties(message));
        }

        private void LBTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clean camera list
            LBCameras.Items.Clear();
            // Get transcoder input format
            transcoderInputFormat = ((VideoConnector)LBTypes.SelectedItem).TranscoderInputFormat;
            // Read the Vms json configuration
            ReadVMS(((VideoConnector)LBTypes.SelectedItem).Value);
        }

        private void LBCameras_SelectedValueChanged(object sender, EventArgs e)
        {
            if (LBCameras.SelectedItem != null)
            {
                var camera = ((CameraEntity)LBCameras.SelectedItem);

                switch (transcoderInputFormat)
                {
                    case TranscoderInputFormat.Jpg:
                        TxtIPCamStreamCommand.Text = String.Format(FFMPEGVideoLiveImage2PipeCommand, txtMediaServerIPAddress.Text, txtMediaServerPort.Text, ((CameraEntity)LBCameras.SelectedItem).SourceId);
                        break;
                    case TranscoderInputFormat.Rtsp:
                        var ipCamera = (IpCameraEntity)camera;
                        TxtIPCamStreamCommand.Text = String.Format(
                            FFMPEGRTSPStreamingUrlCommand,
                            /* TO FIX */Path.Combine(UriExtensionMethods.New("rtsp", ipCamera.RtspAddress, ipCamera.Port, ipCamera.Username, ipCamera.Password).ToString(), ipCamera.RtspAddress),
                            camera.Id);
                        break;
                }

                BtnStartIPCam.Enabled = camera.Active;
                BtnStopIPCam.Enabled = camera.Active;
                TxtIPCamStreamCommand.Enabled = camera.Active;
                BtnStartCameraRec.Enabled = camera.Active;
                BtnStopCameraRec.Enabled = camera.Active;
                TxtCamRecordCommand.Enabled = camera.Active;
                cmbCurrentLiveStreams.Enabled = camera.Active;

                // Refresh streams list
                cmbCurrentLiveStreams.Items.Clear();
                cmbCurrentLiveStreams.Text = "";
                foreach (var item in currentLiveStreams.Where(i => i.Ref.ToString() == camera.SourceId))
                {
                    cmbCurrentLiveStreams.Items.Add(item);
                    cmbCurrentLiveStreams.SelectedIndex = cmbCurrentLiveStreams.Items.Count - 1;
                }
            }
        }

        #endregion

        #region Live

        private void BtnStartIPCam_Click(object sender, EventArgs e)
        {
            StartLive(TxtIPCamStreamCommand.Text);
        }

        protected void StartLive(string transcoderCommand = null)
        {
            if (transcoderCommand == null)
                transcoderCommand = String.Format(FFMPEGVideoLiveImage2PipeCommand, ((CameraEntity)LBCameras.SelectedItem).SourceId);
            var id = Guid.NewGuid().ToString("N");
            var camera = (CameraEntity)LBCameras.SelectedItem;
            TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.StartLiveStream, vms.DomainId,
                    new StartLiveStreamArgs() { Id = id, Camera = camera, TranscoderCommand = transcoderCommand, StreamUrl = string.Format($"localhost:1935/live/{camera.SourceId}"), TranscoderInputFormat = transcoderInputFormat, TranscodingPurpose = TranscodingPurpose.Live }));
            ServiceBus.BasicPublish(DefaultExchangeName, RoutingKey, message, GetBasicProperties(message));
            var item = new ComboBoxItem() { Value = id, Ref = camera.SourceId };
            currentLiveStreams.Add(item);
            // Refresh streams list
            cmbCurrentLiveStreams.Items.Clear();
            cmbCurrentLiveStreams.Text = "";
            foreach (var i in currentLiveStreams.Where(i => i.Ref.ToString() == camera.SourceId))
            {
                cmbCurrentLiveStreams.Items.Add(i);
                cmbCurrentLiveStreams.SelectedIndex = cmbCurrentLiveStreams.Items.Count - 1;
            }
        }

        protected void StopLive()
        {
            TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.StopLiveStream, vms.DomainId,
                new StopLiveStreamArgs { Id = ((ComboBoxItem)cmbCurrentLiveStreams.SelectedItem).Value.ToString(), Camera = (CameraEntity)LBCameras.SelectedItem, TranscoderCommand = String.Empty }));
            ServiceBus.BasicPublish(DefaultExchangeName, RoutingKey, message, GetBasicProperties(message));
            ComboBoxItem item = currentLiveStreams.Where(i => i.Value.ToString() == cmbCurrentLiveStreams.SelectedItem.ToString()).FirstOrDefault();
            currentLiveStreams.Remove(item);
            // Refresh streams list
            cmbCurrentLiveStreams.Items.Clear();
            cmbCurrentLiveStreams.Text = "";
            foreach (var i in currentLiveStreams.Where(i => i.Ref.ToString() == item.Ref.ToString()))
            {
                cmbCurrentLiveStreams.Items.Add(i);
                cmbCurrentLiveStreams.SelectedIndex = cmbCurrentLiveStreams.Items.Count - 1;
            }
        }

        #endregion

        #region Playback

        protected void SendPlaybackMessage(PlaybackVerb verb, string transcoderCommand = null)
        {
            if (transcoderCommand == null)
                transcoderCommand = String.Format(FFMPEGVideoPlaybackImage2PipeCommand, ((CameraEntity)LBCameras.SelectedItem).Id);

            TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.CameraPlayback, vms.DomainId,
                   new CameraPlaybackArgs
                   {
                       Id = playbackTaskId,
                       DateTime = dateTimePicker1.Value,
                       Camera = (CameraEntity)LBCameras.SelectedItem,
                       TranscoderCommand = transcoderCommand,
                       Speed = 1,
                       TranscodingPurpose = TranscodingPurpose.Playback,
                       MediaStreamerLocation = MediaStreamerLocation.OnPremises,
                       PlaybackVerb = verb
                   }));
            ServiceBus.BasicPublish(ExchangeName, RoutingKey, message, GetBasicProperties(message));
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            SendPlaybackMessage(PlaybackVerb.Play);
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            SendPlaybackMessage(PlaybackVerb.Stop);
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            SendPlaybackMessage(PlaybackVerb.Pause);
        }

        private void BtnFF_Click(object sender, EventArgs e)
        {
            SendPlaybackMessage(PlaybackVerb.FastForward);
        }

        private void BtnRew_Click(object sender, EventArgs e)
        {
            SendPlaybackMessage(PlaybackVerb.Rewind);
        }

        private void BtnFramePrev_Click(object sender, EventArgs e)
        {
            SendPlaybackMessage(PlaybackVerb.PreviousFrame);
        }

        private void BtnFrameNext_Click(object sender, EventArgs e)
        {
            SendPlaybackMessage(PlaybackVerb.NextFrame);
        }

        private void BtnSeek_Click(object sender, EventArgs e)
        {
            SendPlaybackMessage(PlaybackVerb.Seek);
        }

        #endregion

        #region Export

        private void BtnExportClip_Click(object sender, EventArgs e)
        {
            if (LBCameras.SelectedItem != null)
            {
                TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.ExportClip, vms.DomainId,
                    new ExportClipArgs { Id = exportTaskId, Camera = ((CameraEntity)LBCameras.SelectedItem), AbsoluteUri = TxtClipExportUNCPath.Text, Filename = TxtClipExportFilename.Text, ClipStartTime = ExportStartTime.Value, ClipEndTime = ExportEndTime.Value, Format = vms.Attributes != null && vms.Attributes.ContainsKey("exportedVideoFormat") ? vms.Attributes["exportedVideoFormat"].ToString() : null }));
                ServiceBus.BasicPublish(ExchangeName, RoutingKey, message, GetBasicProperties(message));
            }
            else
            {
                DialogResult dlgRes = MessageBox.Show("No camera selected.", "Video Clip Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Camera Move

        private void BtnMoveLeft_Click(object sender, EventArgs e)
        {
            MoveCamera(null, null, 1, null);
        }

        private void BtnMoveUp_Click(object sender, EventArgs e)
        {
            MoveCamera(1, null, null, null);
        }

        private void BtnMoveRight_Click(object sender, EventArgs e)
        {
            MoveCamera(null, null, null, 1);
        }

        private void BtnMoveDown_Click(object sender, EventArgs e)
        {
            MoveCamera(null, 1, null, null);
        }

        private void MoveCamera(uint? up, uint? down, uint? left, uint? right)
        {
            if (LBCameras.SelectedItem != null)
            {
                TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.MoveCamera, vms.DomainId,
                    new MoveCameraArgs { Camera = ((CameraEntity)LBCameras.SelectedItem), CameraMoveType = CameraMoveType.PTZ, PTZ = new PTZArgs() { Type = RBAbsolute.Checked ? PTZArgs.PTZType.Absolute : PTZArgs.PTZType.Relative, Up = up, Down = down, Left = left, Right = right } }));
                ServiceBus.BasicPublish(ExchangeName, RoutingKey, message, GetBasicProperties(message));
            }
            else
            {
                DialogResult dlgRes = MessageBox.Show("No camera selected.", "PTZ Command Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnZoomIn_Click(object sender, EventArgs e)
        {
            if (LBCameras.SelectedItem != null)
            {
                TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.MoveCamera, vms.DomainId,
                    new MoveCameraArgs { Camera = ((CameraEntity)LBCameras.SelectedItem), CameraMoveType = CameraMoveType.PTZ, PTZ = new PTZArgs() { Type = PTZArgs.PTZType.Relative, ZoomIn = 1 } }));
                ServiceBus.BasicPublish(ExchangeName, RoutingKey, message, GetBasicProperties(message));
            }
            else
            {
                DialogResult dlgRes = MessageBox.Show("No camera selected.", "PTZ Command Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnZoomOut_Click(object sender, EventArgs e)
        {
            if (LBCameras.SelectedItem != null)
            {
                TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.MoveCamera, vms.DomainId,
                    new MoveCameraArgs { Camera = ((CameraEntity)LBCameras.SelectedItem), CameraMoveType = CameraMoveType.PTZ, PTZ = new PTZArgs() { Type = PTZArgs.PTZType.Relative, ZoomOut = 1 } }));
                ServiceBus.BasicPublish(ExchangeName, RoutingKey, message, GetBasicProperties(message));
            }
            else
            {
                DialogResult dlgRes = MessageBox.Show("No camera selected.", "PTZ Command Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPreset1_Click(object sender, EventArgs e)
        {
            if (LBCameras.SelectedItem != null)
            {
                TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.MoveCamera, vms.DomainId,
                    new MoveCameraArgs
                    {
                        Camera = ((CameraEntity)LBCameras.SelectedItem),
                        CameraMoveType = CameraMoveType.Preset,
                        PTZ = new PTZArgs()
                        {
                            Type = PTZArgs.PTZType.Relative,
                            PresetId = "4"
                        }
                    }));
                ServiceBus.BasicPublish(ExchangeName, RoutingKey, message, GetBasicProperties(message));
            }
            else
            {
                DialogResult dlgRes = MessageBox.Show("No camera selected.", "PTZ Command Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTour1_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Recording

        protected void StartCameraRecording(string transcoderCommand, string uncPath)
        {
            TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.StartCameraRecord, vms.DomainId,
                    new StartCameraRecordArgs
                    {
                        Id = recordTaskId,
                        Camera = (CameraEntity)LBCameras.SelectedItem,
                        TranscoderCommand = transcoderCommand,
                        UncPath = uncPath
                    }));

            ServiceBus.BasicPublish(DefaultExchangeName, RoutingKey, message, GetBasicProperties(message));
        }

        protected void StopCameraRecording(string transcoderCommand, string uncPath)
        {
            TaskMessage message = new TaskMessage(new WorkflowTask(VideoProtocol.MessageType.StopCameraRecord, vms.DomainId,
                    new StartCameraRecordArgs
                    {
                        Id = recordTaskId,
                        Camera = (CameraEntity)LBCameras.SelectedItem
                    }));

            ServiceBus.BasicPublish(DefaultExchangeName, RoutingKey, message, GetBasicProperties(message));
        }

        #endregion

        #region Rabbit Event handlers

        private void ConsumerNewMessage(object sender, EventArgs e)
        {
            RabbitMQConsumerEventArgs<TaskMessage> eArgs = e as RabbitMQConsumerEventArgs<TaskMessage>;
            if (eArgs != null)
            {
                switch (eArgs.Message.Type)
                {
                    case "AddOrUpdateVMS":
                        SetBtnGetCamerasEnable(true);
                        break;
                    case "GetVMSCameras":
                        if (eArgs.Message.Body.Result != null)
                        {
                            List<CameraEntity> cameras = new List<CameraEntity>(eArgs.Message.Body.Result);
                            foreach (var cam in cameras)
                            {
                                AddLBCamerasItem(cam);
                                SetLBCamerasEnable(true);
                            }
                        }
                        break;
                }
            }
        }

        private void ConsumerError(object sender, EventArgs e)
        {
            throw new Exception("ServiceBus Consumer Error exception.");
        }

        void Error(object sender, UnhandledExceptionEventArgs e)
        {
            throw new Exception("ServiceBus Error exception.");
        }

        #endregion

        #region Helpers

        private void InitializeControls()
        {
            PopulateTypesCombo();
        }

        public void PopulateTypesCombo()
        {
            var path = Path.Combine(ApplicationHelper.GetEntryAssemblyPath(), VIDEO_CONNECTORS, "VideoConnectors.json");
            using (StreamReader sr = new StreamReader(path))
            {
                var json = sr.ReadToEnd();
                var list = json.ParseJson<VideoConnectors>();

                foreach (var c in list.Connectors)
                    LBTypes.Items.Add(c);
            }
        }

        protected void ReadVMS(string id)
        {
            var path = Path.Combine(ApplicationHelper.GetEntryAssemblyPath(), CONFIGURATIONS, id + ".json");
            using (StreamReader sr = new StreamReader(path))
            {
                var json = sr.ReadToEnd();
                vms = json.ParseJson<VmsEntity>();

                // Populate textboxes
                txtDomainId.Text = vms.DomainId;
                txtSiteId.Text = vms.SiteId;
                chkIsEnabled.Checked = vms.Enabled;
                txtHost.Text = vms.IPAddress;
                chkIsPingable.Checked = vms.IsPingSupported;
                chkIsVirtual.Checked = vms.IsVirtual;
                txtPassword.Text = vms.Password;
                txtPort.Text = vms.Port.ToString();
                txtUsername.Text = vms.Username;
            }
        }

        private VmsEntity GetVMS()
        {
            int port = 0;
            if (!int.TryParse(txtPort.Text, out port))
                port = 80;

            vms = new VmsEntity(vms)
            {
                DomainId = txtDomainId.Text,
                Enabled = chkIsEnabled.Checked,
                IPAddress = txtHost.Text,
                IsPingSupported = chkIsPingable.Checked,
                IsVirtual = chkIsVirtual.Checked,
                Password = txtPassword.Text,
                Port = port,
                SiteId = txtSiteId.Text,
                Username = txtUsername.Text
            };

            return vms;
        }

        private void ClearControls()
        {
            SetLBConnectorsEnable(true);
            SetLBVersionsEnable(true);
            SetLBCamerasEnable(false);
            ClearLBCamerasItems();
            SetBtnRunServiceEnable(true);
            SetBtnStopServiceEnable(false);
            SetTxtHostText(String.Empty);
            SetTxtHostEnable(false);
            SetTxtPortText(String.Empty);
            SetTxtPortEnable(false);
            SetTxtUsernameText(String.Empty);
            SetTxtUsernameEnable(false);
            SetTxtPasswordText(String.Empty);
            SetTxtPasswordEnable(false);
            SetBtnAddDeviceEnable(false);
            SetBtnRemoveDeviceEnable(false);
            SetBtnGetCamerasEnable(false);
            SetChkIsEnabledEnable(false);
            SetChkIsEnabledChecked(false);
            SetChkIsPingableEnable(false);
            SetChkIsPingableChecked(false);
            SetChkIsVirtualEnable(false);
            SetChkIsVirtualChecked(false);
            SetTxtIPCamStreamCommandText(FFMPEGVideoLiveImage2PipeCommand);
        }

        #region Set Custom Controls

        delegate void SetBoolCallback(bool enabled);
        delegate void SetTextCallback(string text);
        delegate void SetCameraCallback(CameraEntity cam);
        delegate void SetClearCallback();

        private void SetLBConnectorsEnable(bool enabled)
        {
            //// InvokeRequired required compares the thread ID of the
            //// calling thread to the thread ID of the creating thread.
            //// If these threads are different, it returns true.
            //if (this.LBConnectors.InvokeRequired)
            //{
            //    SetBoolCallback d = new SetBoolCallback(SetLBConnectorsEnable);
            //    this.Invoke(d, new object[] { enabled });
            //}
            //else
            //{
            //    this.LBConnectors.Enabled = enabled;
            //}
        }
        private void SetLBVersionsEnable(bool enabled)
        {
            //// InvokeRequired required compares the thread ID of the
            //// calling thread to the thread ID of the creating thread.
            //// If these threads are different, it returns true.
            //if (this.LBVersions.InvokeRequired)
            //{
            //    SetBoolCallback d = new SetBoolCallback(SetLBVersionsEnable);
            //    this.Invoke(d, new object[] { enabled });
            //}
            //else
            //{
            //    this.LBVersions.Enabled = enabled;
            //}
        }
        private void SetLBCamerasEnable(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.LBCameras.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetLBCamerasEnable);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.LBCameras.Enabled = enabled;
            }
        }
        private void SetBtnRunServiceEnable(bool enabled)
        {
            //// InvokeRequired required compares the thread ID of the
            //// calling thread to the thread ID of the creating thread.
            //// If these threads are different, it returns true.
            //if (this.btnRunService.InvokeRequired)
            //{
            //    SetBoolCallback d = new SetBoolCallback(SetBtnRunServiceEnable);
            //    this.Invoke(d, new object[] { enabled });
            //}
            //else
            //{
            //    this.btnRunService.Enabled = enabled;
            //}
        }
        private void ClearLBCamerasItems()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.LBCameras.InvokeRequired)
            {
                SetClearCallback d = new SetClearCallback(ClearLBCamerasItems);
                this.Invoke(d, new object[] { });
            }
            else
            {
                this.LBCameras.Items.Clear();
            }
        }
        private void SetBtnStopServiceEnable(bool enabled)
        {
            //// InvokeRequired required compares the thread ID of the
            //// calling thread to the thread ID of the creating thread.
            //// If these threads are different, it returns true.
            //if (this.btnStopService.InvokeRequired)
            //{
            //    SetBoolCallback d = new SetBoolCallback(SetBtnStopServiceEnable);
            //    this.Invoke(d, new object[] { enabled });
            //}
            //else
            //{
            //    this.btnStopService.Enabled = enabled;
            //}
        }
        private void SetTxtHostText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtHost.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTxtHostText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtHost.Text = text;
            }
        }
        private void SetTxtHostEnable(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtHost.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetTxtHostEnable);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.txtHost.Enabled = enabled;
            }
        }
        private void SetTxtPortText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtPort.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTxtPortText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtPort.Text = text;
            }
        }
        private void SetTxtPortEnable(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtPort.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetTxtPortEnable);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.txtPort.Enabled = enabled;
            }
        }
        private void SetTxtUsernameText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtUsername.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTxtUsernameText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtUsername.Text = text;
            }
        }
        private void SetTxtUsernameEnable(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtUsername.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetTxtUsernameEnable);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.txtUsername.Enabled = enabled;
            }
        }
        private void SetTxtPasswordText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtPassword.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTxtPasswordText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtPassword.Text = text;
            }
        }
        private void SetTxtPasswordEnable(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtPassword.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetTxtPasswordEnable);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.txtPassword.Enabled = enabled;
            }
        }
        private void SetBtnAddDeviceEnable(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnAddDevice.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetBtnAddDeviceEnable);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.btnAddDevice.Enabled = enabled;
            }
        }
        private void SetBtnRemoveDeviceEnable(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnRemoveDevice.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetBtnRemoveDeviceEnable);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.btnRemoveDevice.Enabled = enabled;
            }
        }
        private void SetBtnGetCamerasEnable(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnGetCameras.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetBtnGetCamerasEnable);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.btnGetCameras.Enabled = enabled;
            }
        }
        private void SetChkIsEnabledEnable(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.chkIsEnabled.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetChkIsEnabledEnable);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.chkIsEnabled.Enabled = enabled;
            }
        }
        private void SetChkIsEnabledChecked(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.chkIsEnabled.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetChkIsEnabledChecked);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.chkIsEnabled.Checked = enabled;
            }
        }
        private void SetChkIsPingableEnable(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.chkIsPingable.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetChkIsPingableEnable);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.chkIsPingable.Enabled = enabled;
            }
        }
        private void SetChkIsPingableChecked(bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.chkIsPingable.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetChkIsPingableChecked);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.chkIsPingable.Checked = enabled;
            }
        }
        private void SetChkIsVirtualEnable(bool enabled)
        {
            //// InvokeRequired required compares the thread ID of the
            //// calling thread to the thread ID of the creating thread.
            //// If these threads are different, it returns true.
            //if (this.chkIsVirtual.InvokeRequired)
            //{
            //    SetBoolCallback d = new SetBoolCallback(SetChkIsVirtualEnable);
            //    this.Invoke(d, new object[] { enabled });
            //}
            //else
            //{
            //    this.chkIsVirtual.Enabled = enabled;
            //}
        }
        private void SetChkIsVirtualChecked(bool enabled)
        {
            //// InvokeRequired required compares the thread ID of the
            //// calling thread to the thread ID of the creating thread.
            //// If these threads are different, it returns true.
            //if (this.chkIsVirtual.InvokeRequired)
            //{
            //    SetBoolCallback d = new SetBoolCallback(SetChkIsVirtualChecked);
            //    this.Invoke(d, new object[] { enabled });
            //}
            //else
            //{
            //    this.chkIsVirtual.Checked = enabled;
            //}
        }
        private void AddLBCamerasItem(CameraEntity cam)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (LBCameras.InvokeRequired)
            {
                SetCameraCallback d = new SetCameraCallback(AddLBCamerasItem);
                Invoke(d, new object[] { cam });
            }
            else
            {
                LBCameras.Items.Add(cam);
            }
        }
        private void SetTxtIPCamStreamCommandText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (TxtIPCamStreamCommand.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTxtIPCamStreamCommandText);
                Invoke(d, new object[] { text });
            }
            else
            {
                TxtIPCamStreamCommand.Text = text;
            }
        }

        #endregion

        private void InitializeServiceBus()
        {
            if (serviceBus != null)
            {
                serviceBus.StopConsumers();
                serviceBus.ConsumerNewMessage -= ConsumerNewMessage;
                serviceBus.ConsumerError -= ConsumerError;
                serviceBus = null;
            }

            ushort port = 5672;
            ushort.TryParse(txtServiceBusPort.Text, out port);

            serviceBus = new RabbitMQServiceBus(txtServiceBusIPAddress.Text, txtServiceBusUsername.Text, txtServiceBusPassword.Text, port);

            serviceBus.RegisterConsumer(() =>
            {
                // the consumer of the RPC queue
                var consumer = new RabbitMQMessageConsumer<TaskMessage>(new RabbitMQClient(serviceBus));
                //consumer.AutoAck = true;
                consumer.Exchange = new RabbitMQExchange
                {
                    Name = String.Empty,
                    AutoDelete = true,
                    Durable = true,
                    Type = RabbitMqExchangeType.direct
                };
                consumer.Queue = Queue;
                consumer.RoutingKey = Queue.Name;
                consumer.AutoAck = true;
                return consumer;
            });

            serviceBus.StartConsumers();

            serviceBus.ConsumerNewMessage += ConsumerNewMessage;
            serviceBus.ConsumerError += ConsumerError;
        }

        protected BasicProperties GetBasicProperties(TaskMessage message)
        {
            return new BasicProperties { ReplyTo = Queue.Name, CorrelationId = message.Id, Expiration = "2000" };
        }

        private void BtnStopIPCam_Click(object sender, EventArgs e)
        {
            StopLive();
        }

        #endregion

        public class ComboBoxItem
        {
            public object Ref { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InitializeServiceBus();
        }
    }
}
