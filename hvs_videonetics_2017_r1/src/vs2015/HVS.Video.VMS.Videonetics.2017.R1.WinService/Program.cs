namespace HVS.Video.VMS.Videonetics._2017.R1.WinService
{
    using Services;

    static class Program
    {
        #region Consts

        public const string ServiceDisplayName = "HVS Videonetics 2017 R1 Connector";
        public const string ServiceName = "HVSVNS2017R1Connector";

        #endregion
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            // Need to provide the proper factory to run the service            
            VideoConnectorHostFactory.Run(ServiceName, ServiceDisplayName);
        }
    }
}
