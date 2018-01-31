namespace HVS.Video.VMS.Videonetics._2017.R1
{
    using Logging;
    using VMS;
    using Application;
    using System;
    using Protocol.Video;

	/// <summary>
	/// A class to manage many Videonetics VMSs and the able to stream many cameras along different VMSs
	/// </summary>
	public class VideoneticsVMSClusterManager : VMSClusterManager
    {
	    #region Consts

	    public const string VideoneticSDKNames = "Videonetics 2017 R1";

        #endregion

        #region Constructor(s)

        /// <summary>
        /// VideoneticsVMSClusterManager
        /// </summary>
        public VideoneticsVMSClusterManager()
        {
            Log.Info($"Videonetics Connector: {VideoneticSDKNames}");
        }

        #endregion

        #region Overrides

        /// <summary>
        /// StartLiveStream
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override VideoStreamingArgs StartLiveStream(CameraTaskArgs args)
		{
			//If in Rtsp, just perform base code
			if (Configuration.TranscoderInputFormat == TranscoderInputFormat.Rtsp)
				return base.StartLiveStream(args);

			// We perform the same operation as in the base class but we customize the VideoStreamingArgs resposne
			// since we need to force the stream Url
			CheckVMS(args.Camera.HostId, true);
			lock (syncObject)
			{
				// Load balancing
				CheckLoadBalancing();

				string hlsUrl = VMSManagers[args.Camera.HostId].StartLiveStream(args);
				var videoStreamingArgs = new VideoStreamingArgs(hlsUrl, Configuration.Id, Configuration.IsExternalStreamEnabled) { Hls = hlsUrl };

				// OK serve the request
				return videoStreamingArgs;
			}
		}

        /// <summary>
        /// CameraPlayback
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
	    public override object CameraPlayback(CameraTaskArgs args)
	    {
			CheckVMS(args.Camera.HostId, true);
		    var cameraPlaybackArg = (CameraPlaybackArgs)args;
			lock (syncObject)
		    {
			    switch (cameraPlaybackArg.PlaybackVerb)
			    {
				    case PlaybackVerb.Play:
					    string hlsUrl = (string)VMSManagers[args.Camera.HostId].CameraPlayback(args);
					    return new VideoStreamingArgs(hlsUrl, Configuration.Id, Configuration.IsExternalStreamEnabled) { Hls = hlsUrl };
					case PlaybackVerb.Stop:
					    VMSManagers[args.Camera.HostId].CameraPlayback(args);
						return null;
				    default:
					    throw new ArgumentOutOfRangeException($"Playback verb {cameraPlaybackArg.PlaybackVerb} not implemented.");
			    }
			}
		}

        /// <summary>
        /// MoveCamera
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override object MoveCamera(CameraTaskArgs args)
        {
            CheckVMS(args.Camera.HostId, true);
            lock (syncObject)
            {
                if (args.Camera.IsPtz)
                {
                    VMSManagers[args.Camera.HostId].MoveCamera(args);
                    return null;
                }

                throw new ArgumentOutOfRangeException($"Camera PTZ is {args.Camera.IsPtz}.");
            }
        }
        #endregion

        #region Helpers

        /// <summary>
        /// GetVersion
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
	    {
		    return ApplicationHelper.GetProductVersion(VideoneticSDKNames);
	    }

	    #endregion
	}
}
