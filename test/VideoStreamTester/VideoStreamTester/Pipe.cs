namespace VideoStreamTester
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Runtime.InteropServices;

	public class Pipe : IDisposable
	{
		#region Consts

		private const string NativeDllName = "HVSApplicationPipe.dll";

		#endregion

		#region Fields

		private IntPtr pipe;

		#endregion

		#region Properties

		public string Command { get; set; }
		public bool KillProcess { get; set; } = true;
		public int? ProcessId { get; set; }
		public bool InError { get; private set; }

		#endregion

		#region Constructor/Destructor

		public Pipe()
		{
			pipe = BuildPipe();
		}
		~Pipe()
		{
			Dispose(false);
		}

		#endregion

		#region Methods

		public int Initialize(string command)
		{
			Command = command;

			int result = InitializePipe(command, pipe);
			InError = result == -1;
			return result;
		}
		public string GetVersion()
		{
			return GetPipeVersion(pipe);
		}
		public int Write(byte[] data, int size)
		{
			return WritePipe(data, size, pipe);
		}
		public int Close()
		{
			// close child process otherwise the parent process will not die
			if (KillProcess && ProcessId.HasValue)
			{
				var processes = Process.GetProcesses();
				if (processes.Any(x => x.Id == ProcessId.Value))
				{
				    try
				    {
				        var process = Process.GetProcessById(ProcessId.Value);
				        process.KillChildren();
                    }
				    catch (Exception)
				    {
                        //Already closed
				        return -1;
				    }
					
				}
			}
			// close the pipe, this will return -1 if the process was already killed
			var result = ClosePipe(pipe);
			pipe = IntPtr.Zero;

			return result;
		}

		#endregion

		#region Dispose pattern

		public void Dispose()
		{
			Dispose(true);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (pipe != IntPtr.Zero)
			{
				// Call pipe/process and related pointer
				Close();
			}

			if (disposing)
			{
				// No need to call the finalizer since we've now cleaned
				// up the unmanaged memory
				GC.SuppressFinalize(this);
			}
		}

		#endregion

		#region PInvokes

		[DllImport(NativeDllName)]
		private static extern IntPtr BuildPipe();

		[DllImport(NativeDllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern string GetPipeVersion(IntPtr pPipe);

		[DllImport(NativeDllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern int InitializePipe(string command, IntPtr pPipe);

		[DllImport(NativeDllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern int ClosePipe(IntPtr pPipe);

		[DllImport(NativeDllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern int WritePipe(byte[] data, int size, IntPtr pPipe);

		#endregion PInvokes
	}
}