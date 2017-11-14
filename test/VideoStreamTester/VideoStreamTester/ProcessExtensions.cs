namespace VideoStreamTester
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Management;

	public static class ProcessExtensions
	{
		/// <summary>
		/// Gets the child processes.
		/// </summary>
		/// <param name="process">The process.</param>
		/// <returns></returns>
		public static IEnumerable<Process> GetChildProcesses(this Process process)
		{
			List<Process> children = new List<Process>();
			ManagementObjectSearcher mos = new ManagementObjectSearcher($"Select * From Win32_Process Where ParentProcessID={process.Id}");

			foreach (var o in mos.Get())
			{
				var mo = (ManagementObject) o;
				try
				{
					Process childProcess = Process.GetProcessById(Convert.ToInt32(mo["ProcessID"]));
					children.Add(childProcess);
				}
				catch (Exception)
				{
					// ignored
				}
			}

			return children;
		}

		/// <summary>
		/// Kills the children.
		/// </summary>
		/// <param name="process">The process.</param>
		public static void KillChildren(this Process process)
		{
			var subProcesses = process.GetChildProcesses();
			foreach (var chidlProcess in subProcesses)
			{
				chidlProcess.Kill();
				chidlProcess.Close();
			}
		}
	}
}