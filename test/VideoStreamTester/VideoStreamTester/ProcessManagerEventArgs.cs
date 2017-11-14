namespace VideoStreamTester
{
	using System;
	using System.Diagnostics;

	public class ProcessManagerEventArgs : EventArgs
	{
		#region Properties

		/// <summary>
		/// Gets or sets the process.
		/// </summary>
		/// <value>
		/// The process.
		/// </value>
		public Process Process { get; set; }
		/// <summary>
		/// Gets or sets the exception.
		/// </summary>
		/// <value>
		/// The exception.
		/// </value>
		public Exception Exception { get; set; }
		/// <summary>
		/// Gets or sets the output.
		/// </summary>
		/// <value>
		/// The output.
		/// </value>
		public string Output { get; set; }

		#endregion

	}
}