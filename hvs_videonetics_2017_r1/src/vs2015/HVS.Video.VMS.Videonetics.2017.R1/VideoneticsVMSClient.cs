namespace HVS.Video.VMS.Videonetics._2017.R1
{
    using Data.Entities.Video;
    using Logging;
    using System.Collections.Generic;
    using System.Linq;
    using VMS;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

	class VideoneticsVMSClient : VMSClient
    {
        #region Fields

        public static long sessionId { get; set; }
        private VideoneticsHttpClient client;

        #endregion

        #region Properties
        
        public VideoneticsHttpClient VideoneticsHttpClient => client;

        #endregion

        #region Constructor(s)


        #endregion

        #region Method overrides

        public override bool Connect()
        {
            Log.Info("VideoneticsVMSClient::Connect");
            // Kabir
            // Don't really require this
            // Commenting, test and delete
            // GetRequestHttp("/REST/local/channel");

            client = new VideoneticsHttpClient(VMS);
            IsConnected = true;
            return true;
        }

        public override bool Disconnect()
        {
            IsConnected = false;
            client = null;
            return true;
        }

        public override IEnumerable<CameraEntity> GetCameras()
        {
            List<VideoneticsCamera> cameras = new List<VideoneticsCamera>();
            VideoneticsChannels channelsinfo = client.GetChannelsList();
	        object sync = new object();
            if (channelsinfo?.result != null && channelsinfo.code == 200)
            {
	            Status[] channelsStatus = client.GetChannelsStatus();
	            Parallel.ForEach(channelsinfo.result, channel =>
	            {
		            var status = channelsStatus.FirstOrDefault(x => x.channelId == channel.id);
		            var VNSCamera = new VideoneticsCamera(channel, VMS) {Active = status != null ? !status.channelStatus: false};
		            lock(sync) { cameras.Add(VNSCamera); }
					if(status == null)
						Log.Warn($"Status for camera {channel.id} ({channel.name}) is not available");
	            });
            }
            else
            {
                Log.Debug($"VMS {VMS.IPAddress}-{VMS.Name}: Error: {channelsinfo?.message}");
            }

            VMS.Active = true;
            return cameras;
        }

        protected override void RefreshCameraStatus(out List<CameraEntity> cameras)
        {
            cameras = GetCameras().ToList();
        }

        #endregion

        #region Utilities

        public string GetRequestHttp(string uri)
        {
            Log.Info("VideoneticsVMSClient::GetRequestHttp");
            HttpClient httpClient = new HttpClient();
            if (!VMS.IPAddress.Contains("http"))
            {
                httpClient.BaseAddress = new Uri("http://" + VMS.IPAddress + ":" + VMS.Port);
            }
            else
            {
                httpClient.BaseAddress = new Uri(VMS.IPAddress + ":" + VMS.Port);
            }

            // Add an Accept header for JSON format.
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
            //System.Text.Encoding.ASCII.GetBytes(
            //    string.Format("{0}:{1}", VMS.Username, VMS.Password))));
            HttpResponseMessage response = httpClient.GetAsync(uri).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }

			Log.Debug($"VMS { VMS.Name } : {(int)response.StatusCode} ({response.ReasonPhrase})");
            throw new UnauthorizedAccessException($"VMS { VMS.Name } : { response.ReasonPhrase}");
        }

        #endregion
    }
}
