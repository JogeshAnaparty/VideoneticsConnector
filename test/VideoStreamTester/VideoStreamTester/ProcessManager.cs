namespace VideoStreamTester
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class ProcessManager
	{
		#region Consts

		public static readonly ProcessStartInfo DefaultProcessStartInfo = new ProcessStartInfo
		{
			CreateNoWindow = false,
			WindowStyle = ProcessWindowStyle.Hidden,
			UseShellExecute = false,
			RedirectStandardOutput = false,
			RedirectStandardError = true
		};

		#endregion

		#region Events

		public event EventHandler<ProcessManagerEventArgs> Exit;
		//public event EventHandler<ProcessManagerEventArgs> StartError;

		#endregion

		#region Fields

		protected Process process;
		protected Pipe pipe;
		protected bool usePipe;
		protected object syncObject = new object();
		protected StringBuilder outputStringBuilder = new StringBuilder();

		#endregion

		#region Properties

		/// <summary>
		/// Gets the process.
		/// </summary>
		/// <value>
		/// The process.
		/// </value>
		public Process Process => process;

		/// <summary>
		/// Gets the pipe.
		/// </summary>
		/// <value>
		/// The pipe.
		/// </value>
		public Pipe Pipe => pipe;

		/// <summary>
		/// Gets the output.
		/// </summary>
		/// <value>
		/// The output.
		/// </value>
		public string Output
		{
			get
			{
				lock (syncObject)
				{
					return outputStringBuilder.ToString();
				}
			}
		}

		#endregion

		#region Constructor(s)

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessManager"/> class.
		/// </summary>
		/// <param name="usePipe">if set to <c>true</c> [use pipe].</param>
		public ProcessManager(bool usePipe = false)
		{
			this.usePipe = usePipe;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Starts the process.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="arguments">The arguments.</param>
		/// <param name="startInfo">The start information. If not specified will use DefaultProcessStartInfo</param>
		public Process StartProcess(string fileName, string arguments, ProcessStartInfo startInfo = null)
		{
			if (usePipe)
			{
				pipe = new Pipe();
				var pipeVersion = pipe.GetVersion();

				// Processes snapshot
				var childProcessesBefore = Process.GetCurrentProcess().GetChildProcesses().ToDictionary(x => x.Id, x => x.Id);

				// Initialize and start pipe (escape if from xml settings)
				string command = $"{fileName} {arguments}".Replace("&quot;", "\"");
				var result = pipe.Initialize(command);
				if (result != 0)
					return null;

				// Processes snapshot
				var childProcessesAfter = Process.GetCurrentProcess().GetChildProcesses().ToDictionary(x => x.Id, x => x.Id);

				// Identify the process just created by using the pipe
				if (childProcessesAfter.Count > childProcessesBefore.Count)
				{
					var newProcessId = childProcessesAfter.Except(childProcessesBefore).First().Key;
					process = Process.GetProcessById(newProcessId);
					var childProcesses = process.GetChildProcesses();

					// the program is a child process...
					var enumerable = childProcesses as Process[] ?? childProcesses.ToArray();
					if (enumerable.Any())
						process = enumerable.First();

					if (process == null)
						throw new InvalidOperationException("Cannot find process just created...");

					process.EnableRaisingEvents = true;
					process.Exited += OnExit;
					pipe.ProcessId = newProcessId;
				}

				//Log.DebugFormat("Started FFmpeg Pipe {0}, command: {1}", pipeVersion, command);

				return process;
			}

			if (startInfo == null)
				startInfo = DefaultProcessStartInfo;

			if (string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException(nameof(fileName));

			startInfo.FileName = fileName;
			startInfo.Arguments = arguments;

			// Start the process with the specified info
			process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
			process.Exited += OnExit;
			process.Start();

			// listen for process output
			Task.Factory.StartNew(() =>
			{
				while (!process.StandardError.EndOfStream)
				{
					lock (syncObject)
					{
						outputStringBuilder.AppendLine(process.StandardError.ReadLine());
					}
				}
				//Console.WriteLine("Task exit..");
			});

			return process;
		}

		/// <summary>
		/// Stops the process.
		/// </summary>
		public void StopProcess()
		{
			pipe?.Close();

			if (process != null && !process.HasExited)
			{
				process.Kill();
				process.Close();
			}
		}

		/// <summary>
		/// Writes the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public int WriteDataIntoPipe(byte[] data, int size)
		{
			if (pipe == null)
				return -1;

			return pipe.Write(data, size);
		}

		#endregion

		#region internal event handlers

		protected virtual void OnExit(object sender, EventArgs e)
		{
			Exit?.Invoke(this, new ProcessManagerEventArgs() { Process = process, Output = Output });
		}

		#endregion
	}
}