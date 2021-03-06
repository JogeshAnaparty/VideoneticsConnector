﻿namespace HVS.Video.VMS.Videonetics._2017.R1
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Data.Entities.Video;
    using Newtonsoft.Json;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Logging;

    public class VideoneticsHttpClient
    {
        #region Consts

        //public const string ChannelsRequestUri = "/REST/local/channel/";
        //public const string ChannelStatusRequestUri = "/REST/local/channel/status";
        //public const string StartLiveRequestUri = "/REST/local/startlive/{0}/{1}/{2}/{3}"; // channelId/widht/height/audioEnabled
        //public const string StopLiveRequestUri = "/REST/local/stoplive/{0}"; // sessionid
        //public const string LiveVideoKeepAliveRequestUri = "/REST/local/live/keepalive/{0}"; //sessionId;
        //public const string StartArchiveRequestUri = "/REST/local/startarchive/{0}/{1}/{2}/{3}/{4}"; // channelId/widht/height/starttimestamp/audioEnabled
        //public const string StopArchiveRequestUri = "/REST/local/stoparchive/{0}"; // sessionid
        //public const string ArchiveVideoKeepArchiveAliveRequestUri = "/REST/local/archive/keepalive/{0}"; //sessionId;
        //public const string PTZRequestUri = "/REST/local/ptz/channel/{0}/{1}/{2}"; //channelId/Ptz-Action/speed param;

        //BELOW API STRING'S(adding "i-apiserver") ARE BASED ON UPDATED PACKAGES FROM VIDEONETICS VMS TEAM

        public const string ChannelsRequestUri = "/i-apiserver/REST/local/channel/";
        public const string ChannelStatusRequestUri = "/i-apiserver/REST/local/channel/status";
        public const string StartLiveRequestUri = "/i-apiserver/REST/local/startlive/{0}/{1}/{2}/{3}"; // channelId/widht/height/audioEnabled
        public const string StopLiveRequestUri = "/i-apiserver/REST/local/stoplive/{0}"; // sessionid
        public const string LiveVideoKeepAliveRequestUri = "/i-apiserver/REST/local/live/keepalive/{0}"; //sessionId;
        public const string StartArchiveRequestUri = "/i-apiserver/REST/local/startarchive/{0}/{1}/{2}/{3}/{4}"; // channelId/widht/height/starttimestamp/audioEnabled
        public const string StopArchiveRequestUri = "/i-apiserver/REST/local/stoparchive/{0}"; // sessionid
        public const string ArchiveVideoKeepArchiveAliveRequestUri = "/i-apiserver/REST/local/archive/keepalive/{0}"; //sessionId;
        public const string PTZRequestUri = "/i-apiserver/REST/local/ptz/channel/{0}/{1}/{2}"; //channelId/Ptz-Action/speed param;

        public const int DefaultRetryDelay = 5000; //ms
        public const int DefaultKeepAliveTimeout = 30000; //ms
        public const int RequestTimeout = 2000; //ms
        public const int ArchiveTimeout = 10000;

        protected const int DefaultCameraFps = 25;
        protected const int DefaultCameraVideoWidth = 1920;
        protected const int DefaultCameraVideoHeight = 1080;
        protected const int DefaultCameraAudioEnabled = 1;
        protected const int ResponseStatusCode = 200;

        #endregion

        #region Fields

        private HttpClient httpClient;
        private readonly VmsEntity vms;

        private readonly ConcurrentDictionary<string, Session> liveSessions = new ConcurrentDictionary<string, Session>(); //<sessionId, session>
        private readonly ConcurrentDictionary<string, Session> archiveSessions = new ConcurrentDictionary<string, Session>();

        #endregion

        #region Constructor(s)

        /// <summary>
        /// VideoneticsHttpClient
        /// </summary>
        /// <param name="vms"></param>
        public VideoneticsHttpClient(VmsEntity vms)
        {
            this.vms = vms;
            InitializeClient();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Channels List
        /// </summary>
        /// <returns></returns>
        public VideoneticsChannels GetChannelsList()
        {
            //string response = DoRequest(ChannelsRequestUri);
            Task<string> response = Task.Run(async () =>
            {
                string msg = await DoRequest(ChannelsRequestUri).ConfigureAwait(false);
                return msg;
            });

            //TODO check the response back from the server in case of error
            // i.e. response.code

            return JsonConvert.DeserializeObject<VideoneticsChannels>(response.Result);
        }

        /// <summary>
        /// Gets Channels Status
        /// </summary>
        /// <returns></returns>
	    public Status[] GetChannelsStatus()
	    {
            //string response = DoRequest(ChannelStatusRequestUri);
            Task<string> response = Task.Run(async () =>
            {
                string msg = await DoRequest(ChannelStatusRequestUri).ConfigureAwait(false);
                return msg;
            });
            ChannelStatus statusInfo = JsonConvert.DeserializeObject<ChannelStatus>(response.Result);
		    return statusInfo.result;
	    }

        /// <summary>
        /// Gets Channel Status By ChannelId
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public bool GetChannelStatus(string channelId)
        {
            //string response = DoRequest(ChannelStatusRequestUri);

            Task<string> response = Task.Run(async () =>
            {
                string msg = await DoRequest(ChannelStatusRequestUri).ConfigureAwait(false);
                return msg;
            });

            //TODO check the response back from the server in case of error
            // i.e. response.code

            ChannelStatus statusInfo = JsonConvert.DeserializeObject<ChannelStatus>(response.Result);
            if (statusInfo?.result != null && statusInfo.code == ResponseStatusCode)
            {
                foreach (Status cs in statusInfo.result)
                {
                    if (cs.channelId.ToString() == channelId)
                    {
                        // 0 is active and 1 is inactive
                        return !cs.channelStatus;
                    }
                }
            }
            else
            {
				if(statusInfo != null)
					Log.Error($"Request {httpClient.BaseAddress} ({ChannelStatusRequestUri}) Error: { statusInfo.message}");
            }
            return false;
        }

        /// <summary>
        /// Starts Live Video
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="channelId"></param>
        /// <param name="videoWidth"></param>
        /// <param name="videoHeight"></param>
        /// <param name="enableAudio"></param>
        /// <returns></returns>
        public StartLiveResponse StartLiveVideo(string clientId, string channelId, uint videoWidth = DefaultCameraVideoWidth, uint videoHeight = DefaultCameraVideoHeight, uint enableAudio = DefaultCameraAudioEnabled)
        {
	        var requestString = string.Format(StartLiveRequestUri, channelId, videoWidth, videoHeight, enableAudio);
            //var response = DoRequest(requestString);
            //var response = Task.Run(() => DoRequest(requestString)).Result;
            //string response = DoRequest(requestString).GetAwaiter().GetResult();
            Task<string> liveResponse = Task.Run(async () =>
            {
                string msg = await DoRequest(requestString).ConfigureAwait(false);
                return msg;
            });

            //TODO check the response back from the server in case of error
            // i.e. response.code
            var response = liveResponse.Result;
            StartLiveResponse startliveResponse = JsonConvert.DeserializeObject<StartLiveResponse>(response);
            var result = startliveResponse.result;
            if (result != null && startliveResponse.code == ResponseStatusCode)
            {
                startliveResponse.result.First().cancellationToken = new CancellationTokenSource();
                // clientId is not Unique, it is same for every camera(channelId).
                bool sessionAdded = liveSessions.TryAdd(clientId, startliveResponse.result.First());
                if (!sessionAdded)
                    throw new InvalidOperationException($"An error occurred starting live for Camera {channelId}.");

                KeepAliveAsync(startliveResponse.result.First().sesssionId, startliveResponse.result.First().cancellationToken, LiveVideoKeepAliveRequestUri);
                return startliveResponse;                
            }

			Log.Error($"Request {httpClient.BaseAddress} ({requestString}) Error: { startliveResponse.message}");
            
            throw new InvalidOperationException($"An error occurred starting live for Camera {channelId}.");
        }

        /// <summary>
        /// Stops Live Video
        /// </summary>
        /// <param name="clientId"></param>
        public void StopLiveVideo(string clientId)
        {
            if (liveSessions.ContainsKey(clientId))
            {
                //Dispose keep alive and then send stop live
                liveSessions[clientId].cancellationToken.Cancel(false);
                //Stop live
                //var response = DoRequest(string.Format(StopLiveRequestUri, liveSessions[clientId].sesssionId));
                Task<string> stopResponse = Task.Run(async () =>
                {
                    string msg = await DoRequest(string.Format(StopLiveRequestUri, liveSessions[clientId].sesssionId)).ConfigureAwait(false);
                    return msg;
                });

                var response = stopResponse.Result;
                KeepLive stoplive = JsonConvert.DeserializeObject<KeepLive>(response);
                //TODO check the response back from the server in case of error
                // i.e. response.code
                if(stoplive.code == ResponseStatusCode)
                {
                    Session value;
                    liveSessions.TryRemove(clientId, out value);
                }
                else
                {
                    Log.Error($"Request {httpClient.BaseAddress} ({StopLiveRequestUri}) Error: { stoplive.message}");
                }             
            }
        }

        /// <summary>
        /// Starts Archive Video
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="channelId"></param>
        /// <param name="startTimeStamp"></param>
        /// <param name="videoWidth"></param>
        /// <param name="videoHeight"></param>
        /// <param name="enableAudio"></param>
        /// <returns></returns>
        public StartArchiveVideoResponse StartArchiveVideo(string clientId, string channelId, long startTimeStamp, uint videoWidth = DefaultCameraVideoWidth,
												   uint videoHeight = DefaultCameraVideoHeight, uint enableAudio = DefaultCameraAudioEnabled)
        {
            //var response = DoRequest(string.Format(StartArchiveRequestUri, channelId, videoWidth, videoHeight, startTimeStamp, enableAudio));
            var requestString = string.Format(StartArchiveRequestUri, channelId, videoWidth, videoHeight, startTimeStamp, enableAudio);
            Task<string> archiveResponse = Task.Run(async () =>
            {
                string msg = await DoRequest(requestString, ArchiveTimeout).ConfigureAwait(false);
                return msg;
            });
            //TODO check the response back from the server in case of error
            // i.e. response.code
            var response = archiveResponse.Result;
            StartArchiveVideoResponse startArchiveResponse = JsonConvert.DeserializeObject<StartArchiveVideoResponse>(response);
            var result = startArchiveResponse.result;
            if (result != null && startArchiveResponse.code == ResponseStatusCode)
            {
                startArchiveResponse.result.First().cancellationToken = new CancellationTokenSource();
                bool sessionAdded = archiveSessions.TryAdd(clientId, startArchiveResponse.result.First());
                if (!sessionAdded)
                    throw new InvalidOperationException($"An error occurred starting live for Camera {channelId}.");

                KeepAliveAsync(startArchiveResponse.result.First().sesssionId, startArchiveResponse.result.First().cancellationToken, ArchiveVideoKeepArchiveAliveRequestUri);
                return startArchiveResponse;
            }

			//Log.Error($"Request {httpClient.BaseAddress} ({StartArchiveRequestUri}) ");
            Log.Error($"Request {httpClient.BaseAddress} ({requestString}) Error: { startArchiveResponse.message}");
            throw new InvalidOperationException($"An error occurred starting live for Camera {channelId}:\r\nError:\r\n{startArchiveResponse.message}");
        }

        /// <summary>
        /// Stops Archive Video
        /// </summary>
        /// <param name="clientId"></param>
        public void StopArchiveVideo(string clientId)
        {
            if (archiveSessions.ContainsKey(clientId))
            {
                //Dispose keep alive and then send stop live
                archiveSessions[clientId].cancellationToken.Cancel(false);
                //Stop live
                //var response = DoRequest(string.Format(StopArchiveRequestUri, archiveSessions[clientId].sesssionId));
                Task<string> stopResponse = Task.Run(async () =>
                {
                    string msg = await DoRequest(string.Format(StopArchiveRequestUri, 
                        archiveSessions[clientId].sesssionId)).ConfigureAwait(false);
                    return msg;
                });

                var response = stopResponse.Result;
                KeepLive stoparchive = JsonConvert.DeserializeObject<KeepLive>(response);
                //TODO check the response back from the server in case of error
                // i.e. response.code

                if (stoparchive.code == ResponseStatusCode)
                {
	                Session value;
                    archiveSessions.TryRemove(clientId, out value);
                }
                else
                {
                    Log.Error($"Request {httpClient.BaseAddress} ({StopArchiveRequestUri}) Error: { stoparchive.message}");
                }
            }
        }

        /// <summary>
        /// Move Camera
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="commandId"></param>
        /// <param name="speed"></param>
		public void MoveCamera(string channelId, string commandId, string speed = "0")
        {
            var requestString = string.Format(PTZRequestUri, channelId, commandId, speed);
            Task<string> moveResponse = Task.Run(async () =>
            {
                string msg = await DoRequest(requestString).ConfigureAwait(false);
                return msg;
            });

            var response = moveResponse.Result;
            KeepLive moveCamera = JsonConvert.DeserializeObject<KeepLive>(response);
            if (moveCamera.code != ResponseStatusCode)            
                Log.Error($"Request {httpClient.BaseAddress} ({requestString}) Error: { moveCamera.message}");
            else
                Log.Debug($"Request {httpClient.BaseAddress} ({requestString}) Message: PTZ command- { commandId}");
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Do http Request
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="timeout"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        private async Task<string> DoRequest(string requestUri, uint timeout = RequestTimeout, bool post = false)
        {

            string result = string.Empty;
            var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeout));

            try
            {
                HttpResponseMessage response = !post ? await httpClient.GetAsync(requestUri, cts.Token).ConfigureAwait(false) : await httpClient.PostAsync(requestUri, null, cts.Token).ConfigureAwait(false);
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (TaskCanceledException ex)
            {
                Log.Warn($"Request {httpClient.BaseAddress} ({requestUri}) Error: { ex.Message}");
                result = "{ 'message': '" + ex.Message + "'}";
            }
            catch (Exception ex)
            {
                Log.Error($"Exception:Request {httpClient.BaseAddress} ({requestUri}) Error: { ex.Message}");
                result = "{ 'message': '" + ex.Message + "'}";
            }

            return result;
        }

        /// <summary>
        /// Initialize Client
        /// </summary>
        private void InitializeClient()
        {
	        Log.Debug($"Initializing http client to {vms.Name}...");

	        if (string.IsNullOrEmpty(vms.IPAddress))
		        throw new InvalidOperationException($"VMS {vms.Name}: ip address not set!");

	        if (vms.Port == 0)
		        vms.Port = 80;


			httpClient = new HttpClient
            {
                BaseAddress = !vms.IPAddress.Contains("http")
                    ? new Uri("http://" + vms.IPAddress + ":" + vms.Port)
                    : new Uri(vms.IPAddress + ":" + vms.Port)
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //httpClient.Timeout = TimeSpan.FromMilliseconds(1000);
        }

        /// <summary>
        /// Keep session Alive in Async
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="cancellationTokenSource"></param>
        /// <param name="keepAliveUri"></param>
        protected void KeepAliveAsync(long sessionId, CancellationTokenSource cancellationTokenSource, string keepAliveUri)
        {
            Task.Factory.StartNew(async() =>
            {
                try
                {
                    // Will perform keep alive request every DefaultKeepAliveTimeout ms
                    while (!cancellationTokenSource.IsCancellationRequested)
                    {
                        await DoRequest(string.Format(keepAliveUri, sessionId)).ConfigureAwait(false);
                        await Task.Delay(DefaultKeepAliveTimeout, cancellationTokenSource.Token);
                    }
                }
                catch (Exception e)
                {
                    //In case of error just wait a while and perform again the request
                    Log.Warn($"Cannot sent keep alive message to {vms.IPAddress} due to:\r\r{e}");
                    try
                    {
                        await Task.Delay(DefaultRetryDelay, cancellationTokenSource.Token);
                    }                    
                    catch (TaskCanceledException ex)
                    {
                        Log.Warn(ex.Message);
                    }               
                }

                Log.Debug($"Number of Cameras of Live Stream {liveSessions.Count} {Environment.NewLine} " +
                    $"Number of Cameras of Archive Stream {archiveSessions.Count}");

            }, cancellationTokenSource.Token);
        }

        #endregion
    }
}
