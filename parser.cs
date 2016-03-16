using System;
using System.Collections.Generic;

namespace PL
{
    public class Parser
    {
#region Static
		private static void Error(string msg)
		{
			Compiler.Error("Parser",msg);
		}
#endregion

        private List<Token> _tokenStream;
        private Token token
        {
            get
            {
                if(_tokenStream.Count > 0)
                    return _tokenStream[0];
                else
                    return null;
            }

        }

        private CodeTree tree;

        public Parser(List<Token> tokenStream)
        {
            Compiler.debug("=====================================");
            Compiler.debug("Parsing Tokens ...");

            this._tokenStream = tokenStream;
            this.tree = new CodeTree();
			Function.initFunction();
            Data.init();
            program();

            if(Compiler.DEBUG)
            {
                Compiler.debug("Parsing completed");
                Compiler.debug("=====================================");
                Compiler.debug("-- Data --");
                Data.print();
                Compiler.debug("=====================================");
                Compiler.debug("-- Text --");
                tree.printTree();
            }
        }

        public CodeTree getTree()
        {
            return tree;
        }

        private bool nextToken()
        {
            //Console.WriteLine("next : " + token);
            _tokenStream.RemoveAt(0);
            if(token != null)
                return true;
            else
                return false;
        }

        private void check(params Token.TokenType[] type)
        {
			if(type[0] == Token.TokenType.Const)
			{
				check(Token.TokenType.Int,Token.TokenType.Char);
				return;
			}
			else if(type[0] == Token.TokenType.Register_Or_Const)
			{
				check(Token.TokenType.Register,Token.TokenType.Int,Token.TokenType.Char);
				return;
			}
			
			string msg = "";
			foreach(Token.TokenType tt in type)
			{
				msg += tt + " ";
			}
            if(token == null)
            {
                Error("there should be more " +msg+ "at the end");
            }
			foreach(Token.TokenType tt in type)
			{
				if(token.type == tt)
					return;
			}
			Error("#" + token.lineNumber + " :It should be " + msg + "but I found " + token);

        }

        private void nextLine()
        {
            if(token == null) return;
            while(token.type == Token.TokenType.Endl_KEY)
            {
                if(!nextToken())
                    break;
            }
        }

        private void program()
        {
            data_segment();
            text_segment();
        }
#region Data
        private void data_segment()
        {
			Compiler.verbose("Data Segment:");
            check(Token.TokenType.Data_KEY);
            nextToken();
            nextLine();

            data_st_list();
        }

        private void data_st_list()
        {
            if(token.type != Token.TokenType.Text_KEY)
            {
                check(Token.TokenType.Label);
                Token label = token;
                nextToken();
                data_st(label);
                data_st_list();
            }
        }

        private void data_st(Token label)
        {
            switch(token.type)
            {
                case Token.TokenType.Asciiz_KEY : asciiz_st(label); break;
                case Token.TokenType.Word_KEY : word_st(label); break;
                case Token.TokenType.Byte_KEY : byte_st(label); break;
                case Token.TokenType.Space_KEY : space_st(label); break;
                default : Error("#" + token.lineNumber + " :" + token.type + " is not data type"); break;
            }
        }

        private void asciiz_st(Token label)
        {
            Token key = token;
            nextToken();    //consume key

            check(Token.TokenType.String);
            Token arg = token;
            nextToken();
            nextLine();

            new Data(label,key,arg);
        }

        private void word_st(Token label)
        {
            Token key = token;
            nextToken();    //consume key

            List<Token> args_list = new List<Token>();

            check(Token.TokenType.Int);
            args_list.Add(token);
            nextToken();
            while(token.type == Token.TokenType.Comma_KEY)
            {
                nextToken(); //consume comma
                check(Token.TokenType.Int);
                args_list.Add(token);
                nextToken(); // consume arg
            }
            new Data(label,key,args_list.ToArray());
            nextLine();
        }

        private void byte_st(Token label)
        {
            Token key = token;
            nextToken();    //consume key

            List<Token> args_list = new List<Token>();

            check(Token.TokenType.Char);
            args_list.Add(token);
            nextToken();
            while(token.type == Token.TokenType.Comma_KEY)
            {
                nextToken(); //consume comma
                check(Token.TokenType.Char);
                args_list.Add(token);
                nextToken(); // consume arg
            }
            new Data(label,key,args_list.ToArray());
            nextLine();
        }

        private void space_st(Token label)
        {
            Token key = token;
            nextToken();    //consume key

            check(Token.TokenType.Const);
            Token arg = token;
            nextToken();
            nextLine();

            new Data(label,key,arg);
        }
#endregion

#region Text
        private void text_segment()
        {
			Compiler.verbose("Text Segment:");
            check(Token.TokenType.Text_KEY);
            nextToken();
            nextLine();

            text_block_list();
        }

        private void text_block_list()
        {
            if(token == null) return;
            check(Token.TokenType.Label);
            tree.insertLabel(token.value);

			Compiler.verbose(token.value);

            nextToken();    //consume Label
            nextLine();

            text_block();
            text_block_list();
        }

        private void text_block()
        {
            text_st_list();
        }

        private void text_st_list()
        {
            if(token == null || token.type == Token.TokenType.Label) return;

            text_st();
            text_st_list();
        }

        private void text_st()
        {
            checkFunction(token.type);
        }

        private void checkFunction(Token.TokenType key)
        {
            //Console.WriteLine(token.lineNumber);
            Function f = Function.get(key);
            if(f == null) Error("#" + token.lineNumber + " :no function of type " + key);
            check(f.key);
            Token keyToken = token;
            nextToken();
            Token[] argsToken = new Token[f.args.Count];
            for(int i=0;i < f.args.Count; i++)
            {
                check(f.args[i]);
                argsToken[i] = token;
                nextToken();

                //consume comma
                if(i < f.args.Count - 1)
                {
                    check(Token.TokenType.Comma_KEY);
                    nextToken();
                }
            }
            //Console.WriteLine("ARP:" + token);
            nextLine();

            //create code node
            CodeNode node = new CodeNode(tree.nextAddress(),keyToken, argsToken);
            tree.insertNode(node);

			Compiler.verbose("\t" + node);
        }
#endregion


    }
}
