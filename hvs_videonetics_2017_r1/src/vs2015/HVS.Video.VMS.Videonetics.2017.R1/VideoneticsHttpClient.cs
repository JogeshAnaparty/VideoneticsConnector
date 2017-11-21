namespace HVS.Video.VMS.Videonetics._2017.R1
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

        public const string ChannelsRequestUri = "/REST/local/channel/";
        public const string ChannelStatusRequestUri = "/REST/local/channel/status";
        public const string StartLiveRequestUri = "/REST/local/startlive/{0}/{1}/{2}/{3}"; // channelId/widht/height/audioEnabled
        public const string StopLiveRequestUri = "/REST/local/stoplive/{0}"; // sessionid
        public const string LiveVideoKeepAliveRequestUri = "/REST/local/live/keepalive/{0}"; //sessionId;
        public const string StartArchiveRequestUri = "/REST/local/startarchive/{0}/{1}/{2}/{3}/{4}"; // channelId/widht/height/starttimestamp/audioEnabled
        public const string StopArchiveRequestUri = "/REST/local/stoparchive/{0}"; // sessionid
        public const string ArchiveVideoKeepArchiveAliveRequestUri = "/REST/local/archive/keepalive/{0}"; //sessionId;

        public const int DefaultRetryDelay = 5000; //ms
        public const int DefaultKeepAliveTimeout = 30000; //ms
        public const int RequestTimeout = 2000; //ms

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

        public VideoneticsHttpClient(VmsEntity vms)
        {
            this.vms = vms;
            InitializeClient();
        }

        #endregion

        #region Methods

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

        public StartLiveResponse StartLiveVideo(string clientId, string channelId, uint videoWidth = DefaultCameraVideoWidth, uint videoHeight = DefaultCameraVideoHeight, uint enableAudio = DefaultCameraAudioEnabled)
        {
	        var requestString = string.Format(StartLiveRequestUri, channelId, videoWidth, videoHeight, enableAudio);
            //var response = DoRequest(requestString);
            //var response = Task.Run(() => DoRequest(requestString)).Result;
            //string response = DoRequest(requestString).GetAwaiter().GetResult();
            Task<string> response = Task.Run(async () =>
            {
                string msg = await DoRequest(requestString).ConfigureAwait(false);
                return msg;
            });

            //TODO check the response back from the server in case of error
            // i.e. response.code

            StartLiveResponse startliveResponse = JsonConvert.DeserializeObject<StartLiveResponse>(response.Result);
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

        public void StopLiveVideo(string clientId)
        {
            if (liveSessions.ContainsKey(clientId))
            {
                //Dispose keep alive and then send stop live
                liveSessions[clientId].cancellationToken.Cancel(false);
                //Stop live
                //var response = DoRequest(string.Format(StopLiveRequestUri, liveSessions[clientId].sesssionId));
                Task<string> response = Task.Run(async () =>
                {
                    string msg = await DoRequest(string.Format(StopLiveRequestUri, liveSessions[clientId].sesssionId)).ConfigureAwait(false);
                    return msg;
                });
                KeepLive stoplive = JsonConvert.DeserializeObject<KeepLive>(response.Result);
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

        public StartArchiveVideoResponse StartArchiveVideo(string clientId, string channelId, long startTimeStamp, uint videoWidth = DefaultCameraVideoWidth,
												   uint videoHeight = DefaultCameraVideoHeight, uint enableAudio = DefaultCameraAudioEnabled)
        {
            //var response = DoRequest(string.Format(StartArchiveRequestUri, channelId, videoWidth, videoHeight, startTimeStamp, enableAudio));
            Task<string> response = Task.Run(async () =>
            {
                string msg = await DoRequest(string.Format(StartArchiveRequestUri, channelId, videoWidth,
                    videoHeight, startTimeStamp, enableAudio)).ConfigureAwait(false);
                return msg;
            });
            //TODO check the response back from the server in case of error
            // i.e. response.code

            StartArchiveVideoResponse startArchiveResponse = JsonConvert.DeserializeObject<StartArchiveVideoResponse>(response.Result);
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

			Log.Error($"Request {httpClient.BaseAddress} ({StartArchiveRequestUri}) ");
            throw new InvalidOperationException($"An error occurred starting live for Camera {channelId}:\r\nError:\r\n{startArchiveResponse.message}");
        }

        public void StopArchiveVideo(string clientId)
        {
            if (archiveSessions.ContainsKey(clientId))
            {
                //Dispose keep alive and then send stop live
                archiveSessions[clientId].cancellationToken.Cancel(false);
                //Stop live
                //var response = DoRequest(string.Format(StopArchiveRequestUri, archiveSessions[clientId].sesssionId));
                Task<string> response = Task.Run(async () =>
                {
                    string msg = await DoRequest(string.Format(StopArchiveRequestUri, 
                        archiveSessions[clientId].sesssionId)).ConfigureAwait(false);
                    return msg;
                });
                KeepLive stoparchive = JsonConvert.DeserializeObject<KeepLive>(response.Result);
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

        #endregion

        #region Helpers

        //private string DoRequest(string requestUri, bool post = false)
        //{
        //    try
        //    {
        //        HttpResponseMessage response = !post ? httpClient.GetAsync(requestUri).Result : httpClient.PostAsync(requestUri, null).Result;
        //        return response.Content.ReadAsStringAsync().Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"Request {httpClient.BaseAddress} ({requestUri}) Error: { ex.Message}");
        //        return "{ 'message': '" + ex.Message + "'}";
        //    }
        //}

        private async Task<string> DoRequest(string requestUri, bool post = false)
        {

            string result = string.Empty;
            var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(RequestTimeout)); //RequestTimeout = 2000

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
                Log.Error($"Request {httpClient.BaseAddress} ({requestUri}) Error: { ex.Message}");
                result = "{ 'message': '" + ex.Message + "'}";
            }

            return result;
        }

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
