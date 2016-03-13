using System;

#pragma warning disable 0219

namespace PL
{
	public class Compiler
	{
		public static bool DEBUG;
		private static string path;
		public static int MemorySize;

		private static void init()
		{
			DEBUG = false;
			MemorySize = 500;
		}

		public static void Main(string[] args)
		{
			init();
			parseArgument(args);
			
			Lex lex = new Lex(path);
			Parser parser = new Parser(lex.getTokenStream());
			Executer exe = new Executer(parser.getTree());
		}



		private static void parseArgument(string[] args)
		{
			bool paramFile = false;
			if(args.Length > 0)
			{
				for(int i = 0; i < args.Length; i++)
				{
					if(args[i] == "-t")
					{
						test();
						System.Environment.Exit(1);
					}
					if(args[i] == "-d")
					{
						DEBUG = true;
					}
					if(args[i] == "-f")
					{
						paramFile = true;
						if(i + 1 < args.Length)
							path = args[i+1];
						else
							Error("SysArg","you should tell me where the file is");
						i++;
					}
					if(args[i] == "-m")
					{
						if(i + 1 < args.Length)
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
						else
							Error("SysArg","you should tell me how much memory you want");
						i++;
					}
				}

			}
			if(!paramFile)	path = "test.txt";

		}

		public static void Error(string header, string msg)
		{
			Console.WriteLine("！Σ(￣□￣;) <[Error@" + header + ":" + msg +"]");
			System.Environment.Exit(0);
		}

		public static void Warning(string header, string msg)
		{
			Console.WriteLine("(ー_ー;) <[Warning@" + header + ":" + msg +"]");
		}

		public static void log(params object[] args)
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

		public static void test()
		{
			string x = "		la	\" string here , \"		$a0 ";
			Lex.SplitString(x,' ','\t');
		}
	}
}
