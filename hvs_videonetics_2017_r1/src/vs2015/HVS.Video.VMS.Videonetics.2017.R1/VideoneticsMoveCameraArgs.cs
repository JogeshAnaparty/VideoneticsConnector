namespace HVS.Video.VMS.Videonetics._2017.R1
{
    using System;
    using Protocol.Video;

    /// <summary>
    /// Hasn't been tested.
    /// </summary>
    public class VideoneticsMoveCameraArgs : MoveCameraArgs
    {
        #region Constants

        public const string PTZLEFT = "13";
        public const string PTZRIGHT = "14";
        public const string PTZUP = "15";
        public const string PTZDOWN = "16";
        public const string PTZIN = "21";
        public const string PTZOUT = "22";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the PTZ command.
        /// </summary>
        /// <value>
        /// The PTZ command.
        /// </value>
        public string PTZCommand { get; set; }
        #endregion

        #region Static Constructor

        public static VideoneticsMoveCameraArgs New(MoveCameraArgs args)
        {
            VideoneticsMoveCameraArgs moveCameraArgs = new VideoneticsMoveCameraArgs
            {
                Id = args.Id,
                Camera = args.Camera,
                CameraMoveType = args.CameraMoveType,
                PTZ = args.PTZ
            };

            moveCameraArgs.PTZCommand = moveCameraArgs.ToPTZCommand(args.PTZ);

            return moveCameraArgs;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// To the PTZ command.
        /// </summary>
        /// <param name="ptz">The camera PTZ.</param>
        /// <returns></returns>
        private string ToPTZCommand(PTZArgs ptz)
        {
            var action = ptz.GetPTZAction();
            switch (action)
            {
                case PTZArgs.Action.None:
                return null;
                case PTZArgs.Action.Up:
                    return PTZUP;
                case PTZArgs.Action.Down:
                    return PTZDOWN;
                case PTZArgs.Action.Left:
                    return PTZLEFT;
                case PTZArgs.Action.Right:
                    return PTZRIGHT;
                case PTZArgs.Action.ZoomIn:
                    return PTZIN;
                case PTZArgs.Action.ZoomOut:
                    return PTZOUT;
            }

            throw new InvalidOperationException();
        }

        #endregion
    }
}
