using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace PL
{
	public class Lex
	{
		private List<Token> tokens;

		public Lex(string sourcePath)
		{
			if(Compiler.DEBUG)
			{
				Console.WriteLine("=====================================");
				Console.WriteLine("-- Lex --");
			}

			this.tokens = new List<Token>();

			string line;
			using (var file = new StreamReader(sourcePath, Encoding.Default))
			{
				int lineNumber = 0;
				Compiler.log("-Matching Regex-");
				while( (line = file.ReadLine()) != null )
				{
					lineNumber++;
					string code = line.Split('#')[0];
					string[] splitComma = code.Split(',');

					for(int i=0;i < splitComma.Length; i++)
					{
						foreach(string word in splitComma[i].Split())
						{
							if(word.Length == 0) continue;

							Token newToken = Token.construct(word,lineNumber);
							if(newToken != null)
							{
								this.tokens.Add(newToken);
							}
							else
							{
								Compiler.Error("Lexer","what dafuq is '" + word + "'? in line " + lineNumber);
							}
						}
						if(i < splitComma.Length - 1)
						{
							this.tokens.Add(new Token(Token.TokenType.Comma_KEY, ",", lineNumber));
						}
					}
					//End Line
					this.tokens.Add(new Token(Token.TokenType.Endl_KEY,"", lineNumber));
				}
				file.Close();

				if(Compiler.DEBUG)
				{
					Compiler.log("\nLexer analyzed completed");
					Compiler.log("=====================================");
		            Compiler.log("-- Token Stream --");
					printTokens();
				}
			}
		}

		public List<Token> getTokenStream()
		{
			return tokens;
		}

		private void printTokens()
		{
			foreach(Token t in tokens)
			{
			    t.print();
				if(t.type == Token.TokenType.Endl_KEY) Console.WriteLine("");
			}
		}
	}
}
