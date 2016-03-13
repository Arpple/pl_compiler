using System;
using System.Collections.Generic;

namespace PL
{
    public class Code
    {
        public Token key;
        public Token[] args;

        public Code(Token key, params Token[] args)
        {
            this.key = key;
            if(args.Length > 0)
            {
                this.args = args;
            }
            else
            {
                this.args = new Token[0];
            }
        }

        public override string ToString()
        {
            string str = key.value + " ";
            foreach(Token arg in args)
            {
                str += arg.value + " ";
            }
            return str;
        }

        public string value(int index)
        {
            return args[index].value;
        }
    }
}
