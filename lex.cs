using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
#pragma warning disable 0168
namespace PL
{
	public class Lex
	{
		private List<Token> tokens;

		public Lex()
		{
			Compiler.debug("=====================================");
			Compiler.debug("Lex analyze ...");

			string sourcePath = Compiler.TargetFile;
			this.tokens = new List<Token>();

			string line;
			try
			{
				using (var file = new StreamReader(sourcePath, Encoding.Default))
				{
					int lineNumber = 0;
					Compiler.verbose("Matching Regex ...");
					Token.initRegex();
					while( (line = file.ReadLine()) != null )
					{
						lineNumber++;
						string[] code_comment = SplitString(line,'#');
						if(code_comment.Length == 0) continue;
						string code = code_comment[0];
						string[] splitComma = SplitString(code,',');

						for(int i=0;i < splitComma.Length; i++)
						{
							foreach(string word in SplitString(splitComma[i],' ','\t'))
							{
								if(word.Length == 0) continue;
								Token newToken = Token.construct(word,lineNumber);
								if(newToken != null)
								{
									this.tokens.Add(newToken);
								}
								else
								{
									//no any token match
									Compiler.Error("Lexer","#" + lineNumber + " '" + word + "' is not valid");
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
						Compiler.debug("Lex analyzing completed");
						Compiler.debug("=====================================");
			            Compiler.debug("-- Token Stream --");
						printTokens();
					}
				}
			}
			catch(Exception e)
			{
				Compiler.Error("System","file '" + sourcePath + "' not found");
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

		public static string[] SplitString(string text,params char[] seperators)
		{
			List<string> result = new List<string>();
			string word = "";
			bool isSep;
			bool inString = false;
			foreach(char c in text)
			{
				isSep = false;
				if(c == '\"')
				{
					word += c;
					if(inString)
					{
						//out from string
						result.Add(word);
						word = "";
					}
					inString = !inString;
				}
				else
				{
					if(inString)
					{
						word += c;
					}
					else
					{
						foreach(char sep in seperators)
						{
							if(c == sep)
							{
								result.Add(word);
								word = "";
								isSep = true;
								break;
							}
						}
						if(!isSep)
						{
							word += c;
						}
					}
				}
			}
			if(word.Length > 0)
			{
				result.Add(word);
			}

			return result.ToArray();
		}

#region RIP
		public static string[] SplitOuterQuote(string text,params char[] array) {
            int i,j,len= text.Length;
            bool splited = false , quoteCount = false;
            List<string> stringList = new List<string>();
            string splitText="";

            for (i = 0; i < len;i++ ) {
                splited = false;
                if (text[i] == '"') {
                    quoteCount = !quoteCount;
                }
                if (!quoteCount) {
                    for (j = 0; j < array.Length; j++) {
                        if (text[i] == array[j]) {
                            splited = true;
                            break;
                        }
                    }
                    if (i == len - 1 || !splited) { splitText += text[i]; }
                }
                else {
                    //collect all text in "????????????" no split any thing :3
                    splitText += text[i];
                }
                if (splited || i == len - 1) {
                    if (splitText != "") { stringList.Add(splitText); }
                    splitText = "";
                }
            }


            return stringList.ToArray();
        }
#endregion
	}
}
