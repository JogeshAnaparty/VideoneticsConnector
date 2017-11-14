namespace HVS.Video.VMS.Tester
{
    using HVS.Protocol.Video;
    using System.Collections.Generic;

    public class VideoConnectors
    {
        public IEnumerable<VideoConnector> Connectors { get; set; }
    }

    public class VideoConnector
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public TranscoderInputFormat TranscoderInputFormat { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
