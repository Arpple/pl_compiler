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
			//Lex lex = new Lex("/home/arpple/Desktop/file/prog_lang/test.txt");
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
							Error("Compiler","pls enter file name after -f");
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
						}
						else
							Error("Compiler","pls enter memory size");
						i++;
					}
				}

			}
			if(!paramFile)	path = "test.txt";

		}

		public static void Error(string header, string msg)
		{
			Console.WriteLine("ERROR> " + header + " : " + msg);
			System.Environment.Exit(0);
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
