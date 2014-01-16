using System;
using System.Collections.Generic;
using System.IO;

namespace gitdiff
{
	class Program
	{
		static int Main( string[] args )
		{
			ConsoleColor defaultColor = Console.ForegroundColor;
			ConsoleColor stagedColor = ConsoleColor.DarkGreen;
			ConsoleColor notStagedColor = ConsoleColor.DarkRed;

			string[] lines;
			string git;

			git = gitUtil.getGit();
			if (git.Length == 0 || !File.Exists(git)) {
				Console.WriteLine("git.exe was not found at \"" + git + "\"");
				return 1;
			}

			lines = gitUtil.RunCommand(git, "status");
			if (lines == null || lines.Length == 0) {
				return 2;
			}

			ConsoleKeyInfo k;
			bool cancel = false,
				notStaged = false,
				untrackedFiles = false;
			List<string> files = new List<string>();

			Console.WriteLine("-------------------------------------------------------------------------------");
			Console.WriteLine("  Legend:");
			Console.WriteLine("    Escape|Q to cancel.");
			Console.WriteLine("    D runs 'git difftool fname'. (`difftool` must launch an external diff tool!)");
			Console.WriteLine("    Any other key goes to the next file.");
			Console.WriteLine("-------------------------------------------------------------------------------");

			for (int i = 0; i < lines.Length; i++) {
				string l = lines[i].Trim();

				if (l.Equals("# Changes not staged for commit:")) {
					Console.WriteLine(l);
					notStaged = true;
					untrackedFiles = false;
				} else if (l.Equals("# Untracked files:")) {
					Console.WriteLine(l);
					notStaged = false;
					untrackedFiles = true;
				} else if (untrackedFiles) {
					if (l.StartsWith("#\t")) {
						string fname = l.Substring(2).Trim();
						string star;
						int left;

						Console.CursorLeft = 0;
						Console.Write("#\t");
						Console.ForegroundColor = defaultColor;
						Console.Write("   [");
						left = Console.CursorLeft;
						Console.Write(" ] ");
						Console.ForegroundColor = notStagedColor;
						Console.Write(fname);
						Console.ForegroundColor = defaultColor;
						Console.CursorLeft = left;

						star = " ";
						while (true) {
							k = Console.ReadKey(true);
							if (k.Key == ConsoleKey.Q || k.Key == ConsoleKey.Escape) {
								cancel = true;
								break;
							} else if (k.Key == ConsoleKey.Spacebar || k.Key == ConsoleKey.Y) {
								files.Add(fname);
								star = "*";
								break;
							} else if (k.Key == ConsoleKey.D) {
								// Diff!
								gitUtil.RunCommand(git, string.Format("difftool \"{0}\"", fname));
							} else {
								star = " ";
								break;
							}
						}
						if (cancel) {
							break;
						}

						Console.CursorLeft = 0;
						Console.Write("#\t");
						Console.ForegroundColor = defaultColor;
						Console.Write("   [{0}] ", star);
						Console.ForegroundColor = notStagedColor;
						Console.WriteLine(fname);
						Console.ForegroundColor = defaultColor;
					}
				} else if (notStaged) {
					if (l.StartsWith("#\tmodified:")
							|| l.StartsWith("#\tnew file:")) { // || l.StartsWith("#       deleted:")
						int pos = l.IndexOf(':');
						string cmd = l.Substring(1, pos + 1).Trim();
						string fname = l.Substring(pos + 1).Trim();
						string star;
						int left;

						Console.CursorLeft = 0;
						Console.Write("#\t");
						Console.ForegroundColor = notStagedColor;
						Console.Write(cmd);
						Console.ForegroundColor = defaultColor;
						Console.Write("   [");
						left = Console.CursorLeft;
						Console.Write(" ] ");
						Console.ForegroundColor = notStagedColor;
						Console.Write(fname);
						Console.ForegroundColor = defaultColor;
						Console.CursorLeft = left;

						star = " ";
						while (true) {
							k = Console.ReadKey(true);
							if (k.Key == ConsoleKey.Q || k.Key == ConsoleKey.Escape) {
								cancel = true;
								break;
							} else if (k.Key == ConsoleKey.Spacebar || k.Key == ConsoleKey.Y) {
								files.Add(fname);
								star = "*";
								break;
							} else if (k.Key == ConsoleKey.D) {
								// Diff!
								gitUtil.RunCommand(git, string.Format("difftool \"{0}\"", fname));
							} else {
								star = " ";
								break;
							}
						}
						if (cancel) {
							break;
						}

						Console.CursorLeft = 0;
						Console.Write("#\t");
						Console.ForegroundColor = notStagedColor;
						Console.Write(cmd);
						Console.ForegroundColor = defaultColor;
						Console.Write("   [{0}] ", star);
						Console.ForegroundColor = notStagedColor;
						Console.WriteLine(fname);
						Console.ForegroundColor = defaultColor;
					} else if (l.Equals("#")) {
						Console.WriteLine(l);
					} else {
						Console.WriteLine(l);
					}
				} else {
					if (l.StartsWith("#\tmodified:")
							|| l.StartsWith("#\tnew file:")) { // || l.StartsWith("#       deleted:")

						int pos = l.IndexOf(':');
						string cmd = l.Substring(1, pos + 1).Trim();
						string fname = l.Substring(pos + 1).Trim();

						Console.Write("#\t");
						Console.ForegroundColor = stagedColor;
						Console.WriteLine("{0}   {1}", cmd, fname);
						Console.ForegroundColor = defaultColor;
					} else {
						Console.WriteLine(l);
					}
				}
			}

			Console.WriteLine();

			if (cancel) {
				Console.WriteLine("Canceled.");
				return 0;
			}

			if (lines == null || lines.Length == 0) {
				Console.WriteLine("No files can be added.");
				return 0;
			}

			if (files.Count > 0) {
				Console.Write("Press Enter to accept or Escape to quit: ");
				k = Console.ReadKey(true);
				if (k.Key == ConsoleKey.Enter || k.Key == ConsoleKey.Spacebar) {
					Console.WriteLine();
					for (int i = 0; i < files.Count; i++) {
						string cmd = string.Format("add \"{0}\"", files[i]);
						Console.WriteLine("\n" + cmd);
						lines = gitUtil.RunCommand(git, cmd);
						foreach (string l in lines) {
							Console.WriteLine(l);
						}
					}
				} else {
					Console.WriteLine("Canceled.");
				}
			} else {
				Console.WriteLine("No files selected to add.");
			}

			return 0;
		}
	}
}
