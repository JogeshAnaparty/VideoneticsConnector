[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace VideoStreamTester
{
    using System;
    using System.Windows.Forms;
    using log4net;

    public static class Program
    {
        public static readonly ILog Log = LogManager.GetLogger("VideoStreamTester");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainView());
        }
    }
}
