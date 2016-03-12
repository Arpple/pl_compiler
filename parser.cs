using System;
using System.Collections.Generic;

namespace PL
{
    public class Parser
    {
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
            Compiler.log("=====================================");
            Compiler.log("-- Parser --");
            this._tokenStream = tokenStream;
            Function.initFunction();
            this.tree = new CodeTree();
            Data.init();
            program();

            if(Compiler.DEBUG)
            {
                Compiler.log("Parser completed");
                Compiler.log("=====================================");
                Compiler.log("-- Data --");
                Data.print();
                Compiler.log("=====================================");
                Compiler.log("-- Text --");
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

        private void check(Token.TokenType type)
        {
            if(token == null)
            {
                Compiler.Error("Parser", "syntax error " + token.ToString() + " in line " + token.lineNumber);
            }
            if(token.type != type)
            {
                Compiler.Error("Parser", "syntax error expected token of type '" + type +  "' , found " + token.ToString() + " in line " + token.lineNumber);
            }
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
                default : Compiler.Error("Parser>data_st","wtf, why you give me " + token.ToString() + " in line " + token.lineNumber ); break;
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

            check(Token.TokenType.Const);
            args_list.Add(token);
            nextToken();
            while(token.type == Token.TokenType.Comma_KEY)
            {
                nextToken(); //consume comma
                check(Token.TokenType.Const);
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
            if(f == null) Compiler.Error("Parser-text_st","no function " + key);
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
        }
#endregion


    }
}
