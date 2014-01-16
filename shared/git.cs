using System;
using System.Diagnostics;
using System.IO;
using System.Text;

public static class gitUtil
{
	public static string getGit()
	{
		string git = string.Empty;
		string[] gitPaths;

		gitPaths = new string[] {
			@"c:\Tools\Git\bin\git.exe",
			@"c:\Program Files\Git\bin\git.exe",
			@"c:\Program Files (x86)\Git\bin\git.exe",
			@"c:\Git\bin\git.exe",
			@"git.exe"
		};

		foreach (string p in gitPaths) {
			if (File.Exists(p)) {
				git = p;
				break;
			}
		}

		return git;
	}

	public static string[] RunCommand( string cmd, string arguments )
	{
		ProcessStartInfo startInfo;
		Process p;
		StringBuilder result = new StringBuilder();

		startInfo = new ProcessStartInfo(cmd, arguments);
		startInfo.WorkingDirectory = Environment.CurrentDirectory;
		startInfo.WindowStyle = ProcessWindowStyle.Hidden;
		startInfo.CreateNoWindow = true;
		startInfo.ErrorDialog = false;
		startInfo.RedirectStandardError = false;
		startInfo.RedirectStandardInput = false;
		startInfo.RedirectStandardOutput = true;
		startInfo.UseShellExecute = false;

		p = Process.Start(startInfo);

		using (StreamReader o = p.StandardOutput) {
			//while (!o.EndOfStream) {
			result.Append(o.ReadToEnd());
			//}
		}

		return result.ToString().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
	}
}