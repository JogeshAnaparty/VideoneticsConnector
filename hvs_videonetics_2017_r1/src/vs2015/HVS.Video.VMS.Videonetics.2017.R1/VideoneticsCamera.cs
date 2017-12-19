namespace HVS.Video.VMS.Videonetics._2017.R1
{
	using System;
	using System.Threading;
	using Data.Entities.Video;
    using Newtonsoft.Json;
    using VMS;

    /// <summary>
    /// Concrete Class implementing Videonetics Camera
    /// Will be initialized based on camera information as received from API
    /// </summary>
    [JsonConverter(typeof(BaseClassJsonConverter<CameraEntity>))]
    class VideoneticsCamera : CameraEntity
    {

		#region Properties

		public string RtspUrl => GetAttribute<string>("rtspUrl");
		public string ChannelId => GetAttribute<string>("channelId");

		#endregion

		#region Constructor(s)

		public VideoneticsCamera() { }

        public VideoneticsCamera(channelResult camera, VmsEntity vms)
        {
			// Set default attributed. Active will be updated out of the constructor by calling status service
            SourceId = $"{vms.Id}_{camera.id}";
	        SetAttribute("channelId", camera.id.ToString(),"","string");
			HostId = vms.Id.ToString();
            DomainId = vms.DomainId;
            SiteId = vms.SiteId;
            Name = camera.name;
	        Type = CameraType.Vms;
            Latitude = camera.latitude;
            Longitude = camera.longitude;
			Description = !string.IsNullOrEmpty(camera.ip) ?  $"Videonetics Camera [{SourceId} - {camera.ip}]" : $"Videonetics Camera [{SourceId}]";
			// PTZ not supported yet?
	        IsPtz = Convert.ToBoolean(camera.configurationType);
	        RecordingEnabled = camera.recordingId > 0;

			// Set additional attributes
			
			if(!string.IsNullOrEmpty(camera.model))
				SetAttribute("model", camera.model);

	        if (!string.IsNullOrEmpty(camera.snapUrl))
		        SetAttribute("snapUrl", camera.snapUrl);

	        if (!string.IsNullOrEmpty(camera.analyticUrl))
		        SetAttribute("analyticUrl", camera.analyticUrl);

	        if (!string.IsNullOrEmpty(camera.minorUrl))
		        SetAttribute("minorUrl", camera.minorUrl);

	        if (!string.IsNullOrEmpty(camera.majorUrl))
		        SetAttribute("majorUrl", camera.majorUrl);

	        if (!string.IsNullOrEmpty(camera.username))
		        SetAttribute("username", camera.username);

	        if (!string.IsNullOrEmpty(camera.password))
		        SetAttribute("password", camera.password);

			//Force system to use Videonetics HLS server
			SetAttribute("useExternalHLSMediaServer", true);

			if(!string.IsNullOrEmpty(GetAttribute<string>("majorUrl")))
			{
				string rtspUrl = GetAttribute<string>("majorUrl");
				if (!string.IsNullOrEmpty(GetAttribute<string>("username")) && !string.IsNullOrEmpty(GetAttribute<string>("password")))
				{
					Uri uri = new Uri(rtspUrl);
					if (string.IsNullOrEmpty(uri.UserInfo))
					{
						// the uri does not contain username and password
						string username = GetAttribute<string>("username");
						string password = GetAttribute<string>("password");
						rtspUrl = rtspUrl.Insert(rtspUrl.IndexOf("//", StringComparison.Ordinal) + 2, $"{username}:{password}@");
					}
				}
				SetAttribute("rtspUrl", rtspUrl);
			}


			//if (!string.IsNullOrEmpty(camera.model) && camera.model.ToUpperInvariant().Equals("RTSP"))
			//{
			//    // RTSP is not supported
			//    // Storing the Major URL details for sample
			//    Attributes.Add("ipAddress", new GenericEntityAttribute(camera.majorUrl));
			//    IsPtz = false;
			//}
			//else
			//{
			//    // Kabir
			//    Attributes.Add("ipAddress", new GenericEntityAttribute(camera.ip));                
			//    // Kabir
			//    // Addded the below to send the RTSP URL directly
			//    ////root:root@
			//    //string majorURL = $"rtsp://root:root@{camera.ip}/live2.sdp";
			//    //Attributes.Add("majorURL", new GenericEntityAttribute(majorURL));

			//    // Current camera doesn't support PTZ
			//    //IsPtz = (bool)camera["capabilities"]["camera"]["PTZ"]["tilt"] || 
			//    //    (bool)camera["capabilities"]["camera"]["PTZ"]["pan"] || 
			//    //    (bool)camera["capabilities"]["camera"]["PTZ"]["zoom"];

			//    // Hardcoing to 25
			//    Fps = 25;
			//    IsPtz = true;
			//}

			//Attributes.Add("cameraStreamId", new GenericEntityAttribute(camera.number.ToString()));

			//Attributes.Add("isVNS", new GenericEntityAttribute(true));

			//if (!vms.IPAddress.Contains("http"))
			//{
			//    Attributes.Add("VNSAddress", new GenericEntityAttribute("http://" + vms.IPAddress + ":" + vms.Port));
			//}
			//else
			//{
			//    Attributes.Add("VNSAddress", new GenericEntityAttribute(vms.IPAddress + ":" + vms.Port));
			//}

			//// This comes from the HVS
			//Attributes.Add("VNSUser", new GenericEntityAttribute(vms.Username));
			//Attributes.Add("VNSPassword", new GenericEntityAttribute(vms.Password));

			//// Kabir
			//// As camera type is unknown, setting it to VMS
			//Type = CameraType.Vms;
		}


        #endregion
    }

    #region VideoNetics specific entities

    // Based on the JSON data from VMS
    class VMSCamera : CameraEntity
    {
        public int status { get; set; }
        public int code { get; set; }
        public string description { get; set; }
        public string message { get; set; }
        public string uri { get; set; }
        public channelResult[] result { get; set; }
    }

    public class VideoneticsChannels 
    {
        public int status { get; set; }
        public int code { get; set; }
        public string description { get; set; }
        public string message { get; set; }
        public string uri { get; set; }
        public channelResult[] result { get; set; }
    }


    public class channelResult
    {
        public int id { get; set; }
        public string model { get; set; }
        public string ip { get; set; }
        public string name { get; set; }
        public int configurationType { get; set; }
        public string snapUrl { get; set; }
        public string analyticUrl { get; set; }
        public string minorUrl { get; set; }
        public string majorUrl { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public int recordingId { get; set; }
        public int locationId { get; set; }
        public int junctionId { get; set; }
        public int groupId { get; set; }
        public int recordingStream { get; set; }
        public int oldestClipTime { get; set; }
        public int number { get; set; }
        public int channelType { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public int commandPort { get; set; }
        public int param1 { get; set; }
        public int param2 { get; set; }
        public int vocRecordingFlag { get; set; }
        public int noOfJobs { get; set; }
    }
    public class StartLiveResponse
    {
        public int status { get; set; }
        public int code { get; set; }
        public string description { get; set; }
        public string message { get; set; }
        public string uri { get; set; }
        public Session[] result { get; set; }
    }
	public class StartArchiveVideoResponse
	{
		public int status { get; set; }
		public int code { get; set; }
		public string description { get; set; }
		public string message { get; set; }
		public string uri { get; set; }
		public Session[] result { get; set; }
	}
	public class KeepLive
    {
        public int status { get; set; }
        public int code { get; set; }
        public string description { get; set; }
        public string message { get; set; }
        public string uri { get; set; }
        public object[] result { get; set; }
    }
    public class Session
    {
        public long sesssionId { get; set; }
        public string hlsURL { get; set; }
        public CancellationTokenSource cancellationToken;
    }
    public class ChannelStatus
    {
        public int status { get; set; }
        public int code { get; set; }
        public string description { get; set; }
        public string message { get; set; }
        public string uri { get; set; }
        public Status[] result { get; set; }
    }
    public class Status
    {
        public int channelId { get; set; }
        public bool channelStatus { get; set; }
    }

    #endregion
}