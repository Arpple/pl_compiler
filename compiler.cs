using System;
using System.Diagnostics;

#pragma warning disable 0219

namespace PL
{
	public class Compiler
	{
		public static bool DEBUG;
		public static bool VERBOSE;
		public static string TargetFile;
		public static int MemorySize;
		public static bool AutoMemory;
		public static string Language;

		public static void Main(string[] args)
		{
			Stopwatch sw = Stopwatch.StartNew();
			parseArgument(args);

			Lex lex = new Lex();
			Parser parser = new Parser(lex.getTokenStream());

			sw.Stop();
			Compiler.Success("compiling completed in " + sw.Elapsed + " ,executing program");

			Executer exe = new Executer(parser.getTree());

			Console.WriteLine();
			Compiler.Success("executing completed");
			System.Environment.Exit(0);
		}

		private static void parseArgument(string[] args)
		{
			TargetFile = "test.txt";
			DEBUG = false;
			VERBOSE = false;
			MemorySize = 500;
			AutoMemory = false;
			Language = "TH";

			if(args.Length > 0)
			{
				for(int i = 0; i < args.Length; i++)
				{
					if(args[i] == "--help")
					{
						help();
					}
					if(args[i] == "-t")
					{
						test();
						System.Environment.Exit(0);
					}
					if(args[i] == "-d")
					{
						DEBUG = true;
					}
					if(args[i] == "-v")
					{
						VERBOSE = true;
					}
					if(args[i] == "-f")
					{
						if(i + 1 < args.Length)
							TargetFile = args[i+1];
						else
							Error("SysArg","you should tell me where the file is");
						i++;
					}
					if(args[i] == "-m")
					{
						if(i + 1 < args.Length)
						{
							if(args[i+1] == "auto")
							{
								AutoMemory = true;
							}
							else
							{
								int m = 0;
								int.TryParse(args[i+1],out m);
								if(m > 0)
								{
									MemorySize = m;
								}
								else
								{
									Warning("SysArg","memory size '" + args[i+1] + "' is weird so im gonna go with " + MemorySize);
								}
							}
						}
						else
							Error("SysArg","you should tell me how much memory you want");
						i++;
					}
					if(args[i] == "-l")
					{
						if(i + 1 < args.Length)
						{
							if(args[i+1] == "TH" || args[i+1] == "th" || args[i+1] == "en" || args[i+1] == "EN")
							{
								Language = args[i+1];
							}
							else
							{
								Warning("SysArg","language '" + args[i+1] + "' is too hard so im gonna go with " + Language);
							}
						}
						else
							Error("SysArg","you should tell me the language you want");
						i++;
					}
				}
			}
		}

#region String
		private static void help()
		{
			Console.WriteLine("(-ＯvＯ*) <[");

			Console.WriteLine("Usage: Compiler.exe [-f file][-m size][-l lang][-d][-v]");
			Console.WriteLine("  -f file       compile and run target file (default 'test.txt')");
			Console.WriteLine("  -m size       allocate memory of specific size (default 500)");
			Console.WriteLine("                you can also enter size 'auto' to auto extend size of memory");
			Console.WriteLine("  -l lang       compile the text in specific language (TH/EN)(default EN)");
			Console.WriteLine("  -d            show debug");
			Console.WriteLine("  -v            show verbose");


			Console.WriteLine("]");
			System.Environment.Exit(0);
		}

		public static void Error(string header, string msg)
		{
			Console.WriteLine("！Σ(￣□￣;) <[Error@" + header + ":" + msg +"]");
			System.Environment.Exit(1);
		}

		public static void Warning(string header, string msg)
		{
			Console.WriteLine("(ー_ー;) <[Warning@" + header + ":" + msg +"]");
		}

		public static void Success(string msg)
		{
			Console.WriteLine("( ＞▽＜) <[" + msg + "]");
		}

		public static void debug(params object[] args)
		{
			if(DEBUG)
			{
				string msg = "";
				foreach(object arg in args)
				{
					msg += arg.ToString();
				}
				Console.WriteLine(msg);
			}
		}

		public static void verbose(params object[] args)
		{
			if(VERBOSE)
			{
				string msg = "";
				foreach(object arg in args)
				{
					msg += arg.ToString();
				}
				Console.WriteLine(msg);
			}
		}
#endregion

		public static void test()
		{
			string x = "		la	\" string here , \"		$a0 ";
			Lex.SplitString(x,' ','\t');
		}
	}
}
