using System;
using System.Collections.Generic;

namespace PL
{
    public class Data
    {
#region static
        private static int lastAddress;
        private static List<Data> dataList;
        private static Dictionary<string,int> dataTable;

        public static void init()
        {
            dataList = new List<Data>();
            dataTable = new Dictionary<string,int>();
            lastAddress = 0;
        }

        public static void print()
        {
            foreach(KeyValuePair<string,int> entry in dataTable)
            {
                Console.WriteLine(entry.Key + " " + dataList[entry.Value]);
            }
        }

#endregion

        public Token.TokenType type;
        public int address;

        private int[] _value;

        public Data(Token label, Token key, params Token[] args)
        {
            switch(key.type)
            {
                case Token.TokenType.Asciiz_KEY : init_asciiz(args); break;
                case Token.TokenType.Word_KEY : init_word(args); break;
                case Token.TokenType.Byte_KEY : init_byte(args); break;
                case Token.TokenType.Space_KEY : init_space(args); break;
            }

            this.type = key.type;
            this.address = lastAddress;
            dataList.Add(this);
            dataTable.Add(label.value, this.address);
            lastAddress++;
        }

        public override string ToString()
        {
            string ret = "Dx" + this.address + " : ";
            if(this.type == Token.TokenType.Asciiz_KEY)
            {
                char[] char_val = new char[this._value.Length];
                for(int i=0; i < this._value.Length; i++)
                {
                    char_val[i] = (char)this._value[i];
                }
                ret += new string(char_val);
            }
            else if(this.type == Token.TokenType.Word_KEY)
            {
                for(int i=0; i < this._value.Length - 1; i++)
                {
                    ret += this._value[i] + ",";
                }
                ret += this._value[this._value.Length - 1];
            }
            else if(this.type == Token.TokenType.Byte_KEY)
            {
                for(int i=0; i < this._value.Length - 1; i++)
                {
                    ret += "\'" + ((char)this._value[i]).ToString() + "\',";
                }
                ret += ((char)this._value[this._value.Length - 1]).ToString();
            }
            else if(this.type == Token.TokenType.Space_KEY)
            {
                ret += "\"";
                char[] char_val = new char[this._value.Length];
                for(int i=0; i < this._value.Length; i++)
                {
                    if((char)char_val[i] == '\0')
                    {
                        break;
                    }
                    ret += (char)this._value[i];
                }
                ret += "\"";
            }

            return ret;
        }

        private void init_asciiz(Token[] args)
        {
            //str => int[]
            string str = args[0].value.Trim(new char[]{'\"'});
            this._value = new int[str.Length];

            for(int i=0; i < str.Length; i++)
            {
                this._value[i] = (int)str[i];
            }
        }

        private void init_space(Token[] args)
        {
            int size = int.Parse(args[0].value);
            if(size <= 0)
                Compiler.Error("Parser-Data","Cannnot allocate space of size " + size);
            this._value = new int[size];
            this._value[0] = (int)'\0';
        }

        private void init_word(Token[] args)
        {
            this._value = new int[args.Length];
            for(int i=0; i < args.Length; i++)
            {
                this._value[i] = int.Parse(args[i].value);
            }
        }

        private void init_byte(Token[] args)
        {
            this._value = new int[args.Length];
            for(int i=0; i < args.Length; i++)
            {
                char c = args[i].value.Trim(new char[] {'\''})[0];
                this._value[i] = (int)c;
            }
        }

        public int get(int index)
        {
            if(index < 0 || index >= this._value.Length)
                Compiler.Error("RunTime","index out of range");
            return this._value[index];
        }
    }
}
