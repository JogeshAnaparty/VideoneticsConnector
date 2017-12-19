namespace HVS.Video.VMS.Videonetics._2017.R1
{
	using System;
	using Protocol.Video;
    using VMS;
    using System.Linq;
    using Logging;

	class VideoneticsVMSManager : VirtualVMSManager
    {
        #region Fields

        private VideoneticsHttpClient httpClient;
        const string PTZ_SPEED = "5";

        #endregion

        #region Properties

        public VideoneticsHttpClient HttpClient => httpClient ?? (httpClient = new VideoneticsHttpClient(VMS));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoneticsVMSManager"/> class.
        /// </summary>
        /// <param name="vmsClient">The VMS client.</param>
        public VideoneticsVMSManager(IVMSClient vmsClient) : base(vmsClient)
        {
        }

        #endregion

        #region LiveStream

        /// <summary>
        /// StartLiveStream
        /// </summary>
        /// <param name="commandArgs"></param>
        /// <returns></returns>
        public override string StartLiveStream(CameraTaskArgs commandArgs)
        {
	        switch (configuration.TranscoderInputFormat)
	        {
		        case TranscoderInputFormat.Rtsp:

					//Get the camera from the list
					VideoneticsCamera camera = (VideoneticsCamera)Cameras.FirstOrDefault(x => x.SourceId == commandArgs.Camera.SourceId);
					if (camera == null)
						throw new InvalidOperationException($"Cannot find camera sourceId {commandArgs.Camera.SourceId}");

			        if (!string.IsNullOrEmpty(camera.RtspUrl))
			        {
						// force system to use rtsp stream from the camera
				        string orginalCommand = ((TranscoderTaskArgs)commandArgs).TranscoderCommand;
				        var mediaServerIndex = orginalCommand.LastIndexOf("rtsp", StringComparison.Ordinal);
				        string mediaServer = orginalCommand.Substring(mediaServerIndex, orginalCommand.Length - mediaServerIndex);
				        ((TranscoderTaskArgs)commandArgs).TranscoderInputFormat = TranscoderInputFormat.Rtsp;
						//TODO fix this moving in the configuration if needed, to apply transcoding, in case.
				        ((TranscoderTaskArgs)commandArgs).TranscoderCommand = $"ffmpeg -i &quot;{camera.RtspUrl}&quot; -codec copy -an -rtsp_transport tcp -f rtsp {mediaServer}";
				        return base.StartLiveStream(commandArgs);
			        }

			        throw new InvalidOperationException($"Rtsp url not set for camera sourceId {commandArgs.Camera.SourceId}");
		        default:
			        //Base behavior makes use of Videonetics V-Connect HLS media server
					StartLiveResponse response = HttpClient.StartLiveVideo(commandArgs.Id, commandArgs.Camera.Attributes["channelId"].Value.ToString());
			        Log.Debug($"Camera {commandArgs.Camera.Name} - Live video started and available at: {response.result.First().hlsURL}");
			        return response.result.First().hlsURL;
			}
        }

        /// <summary>
        /// StopLiveStream
        /// </summary>
        /// <param name="commandArgs"></param>
        public override void StopLiveStream(CameraTaskArgs commandArgs)
        {
	        if (configuration.TranscoderInputFormat == TranscoderInputFormat.Rtsp)
				RemoveCamerasTranscoder(commandArgs);

	        HttpClient.StopLiveVideo(commandArgs.Id);
	        Log.Debug($"Camera {commandArgs.Camera.Name} - Live video stopped.");
		}


		#endregion

	    #region Playback

	    public override object CameraPlayback(CameraTaskArgs args)
	    {
		    var cameraPlaybackArg = (CameraPlaybackArgs) args;
		    switch (cameraPlaybackArg.PlaybackVerb)
		    {
               
			    case PlaybackVerb.Play:
                    //long unixTime = (int)(cameraPlaybackArg.DateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
                    long unixTime = Convert.ToInt64((cameraPlaybackArg.DateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalMilliseconds);
                    StartArchiveVideoResponse response = HttpClient.StartArchiveVideo(args.Id, args.Camera.Attributes["channelId"].Value.ToString(), unixTime);
				    Log.Debug($"Camera {cameraPlaybackArg.Camera.Name} - Live video started and available at: {response.result.First().hlsURL}");
				    return response.result.First().hlsURL;
					//cameraPlaybackArg.StreamUrl = "http://localhost:1935/vod/03.mp4/playlist.m3u8";
			    case PlaybackVerb.Stop:
					HttpClient.StopArchiveVideo(cameraPlaybackArg.Id);
				    Log.Debug($"Camera {cameraPlaybackArg.Camera.Name} - Archive video stopped.");
					break;
		    }

		    return null;
	    }

        #endregion

        #region ExportClip

        public override void ExportClip(CameraTaskArgs args)
        {
            base.ExportClip(args);
            // TODO
            // REST implementation for exporting the clip
        }

        #endregion

        #region MoveCamera

        public override void MoveCamera(CameraTaskArgs args)
        {            
            base.MoveCamera(args);
            if (args.Camera.IsPtz)
            {
                // TODO
                // PTZ Implementation 
                VideoneticsMoveCameraArgs moveArgs = VideoneticsMoveCameraArgs.New((MoveCameraArgs)args);
                //VideoneticsCamera camera = ((VideoneticsCamera)Cameras.FirstOrDefault(x => x.Id == moveArgs.Camera.Id));
                //if (camera == null)
                //    throw new InvalidOperationException($"Cannot find camera id {moveArgs.Camera.Id}");
                VideoneticsCamera camera = (VideoneticsCamera)Cameras.FirstOrDefault(x => x.SourceId == args.Camera.SourceId);
                if (camera == null)
                    throw new InvalidOperationException($"Cannot find camera sourceId {args.Camera.SourceId}");

                string cameraId = args.Camera.Attributes["channelId"].Value.ToString();
                if (moveArgs.CameraMoveType == CameraMoveType.PTZ)
                {
                    switch (moveArgs.PTZCommand)
                    {
                        case VideoneticsMoveCameraArgs.PTZUP:
                            httpClient.MoveCamera(cameraId, VideoneticsMoveCameraArgs.PTZUP, PTZ_SPEED);
                            break;
                        case VideoneticsMoveCameraArgs.PTZDOWN:
                            httpClient.MoveCamera(cameraId, VideoneticsMoveCameraArgs.PTZDOWN, PTZ_SPEED);
                            break;
                        case VideoneticsMoveCameraArgs.PTZLEFT:
                            httpClient.MoveCamera(cameraId, VideoneticsMoveCameraArgs.PTZLEFT, PTZ_SPEED);
                            break;
                        case VideoneticsMoveCameraArgs.PTZRIGHT:
                            httpClient.MoveCamera(cameraId, VideoneticsMoveCameraArgs.PTZRIGHT, PTZ_SPEED);
                            break;
                        case VideoneticsMoveCameraArgs.PTZIN:
                            httpClient.MoveCamera(cameraId, VideoneticsMoveCameraArgs.PTZIN, PTZ_SPEED);
                            break;
                        case VideoneticsMoveCameraArgs.PTZOUT:
                            httpClient.MoveCamera(cameraId, VideoneticsMoveCameraArgs.PTZOUT, PTZ_SPEED);
                            break;
                    }
                }
            }
        }
        #endregion
    }
}
