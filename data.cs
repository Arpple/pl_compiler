using System;
using System.Collections.Generic;

namespace PL
{
    public class Data
    {
#region static
        private static int lastAddress;
        //private static List<Data> dataList;
        private static int[] dataList;
        private static Dictionary<string,int> dataTable;

        public static void init()
        {
            Console.WriteLine(Compiler.MemorySize);
            dataList = new int[Compiler.MemorySize];
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

        public static int getData(int address,int offset)
        {
            return dataList[address + offset];
        }

        public static void saveData(int address,int offset, int value)
        {
            dataList[address + offset] = value;
        }

        public static int getAddress(string label)
        {
            int addr = -1;
            dataTable.TryGetValue(label,out addr);
            if(addr < 0)
                Compiler.Error("Runtime","cannot get address from " + label);
            return addr;
        }

#endregion

        public Token.TokenType type;
        public int address;
        private int size;

        public Data(Token label, Token key, params Token[] args)
        {
            this.type = key.type;
            this.address = lastAddress;

            switch(key.type)
            {
                case Token.TokenType.Asciiz_KEY : init_asciiz(args); break;
                case Token.TokenType.Word_KEY : init_word(args); break;
                case Token.TokenType.Byte_KEY : init_byte(args); break;
                case Token.TokenType.Space_KEY : init_space(args); break;
            }

            dataTable.Add(label.value, this.address);
        }

        public void alloc(int size)
        {
            if(lastAddress + size <= Compiler.MemorySize)
            {
                lastAddress += size;
                this.size = size;
            }
            else
            {
                Compiler.Error("Data","not enough memory space, try config with -m <mem_size>");
            }
        }

        public override string ToString()
        {
            string ret = "Dx" + this.address + " : ";
            if(this.type == Token.TokenType.Asciiz_KEY)
            {
                ret += '\"';
                int i = this.address;
                char c = (char)dataList[i];
                while(c != '\0')
                {
                    ret += c;
                    i++;
                    c = (char)dataList[i];
                }
                ret += '\"';
            }
            else if(this.type == Token.TokenType.Word_KEY)
            {
                for(int i=this.address; i < this.size - 1; i++)
                {
                    ret += dataList[i] + ',';
                }
                ret += dataList[this.address + this.size - 1];
            }
            else if(this.type == Token.TokenType.Byte_KEY)
            {
                for(int i=this.address; i < this.size - 1; i++)
                {
                    ret += "\'" + (char)dataList[i] + "\',";
                }
                ret += '\'' + (char)dataList[this.address + this.size - 1] + '\'';
            }
            else if(this.type == Token.TokenType.Space_KEY)
            {
                ret += '\"';
                for(int i=this.address; i < this.size; i++)
                {
                    if((char)dataList[i] == '\0')
                    {
                        break;
                    }
                    ret += (char)dataList[i];
                }
                ret += '\"';
            }

            return ret;
        }

        private void init_asciiz(Token[] args)
        {
            //str => int[]
            string str = args[0].value.Trim(new char[]{'\"'});
            alloc(str.Length);
            for(int i=0; i < str.Length; i++)
            {
                dataList[this.address + i] = (int)str[i];
            }

        }

        private void init_space(Token[] args)
        {
            int size = int.Parse(args[0].value);
            if(size <= 0)
                Compiler.Error("Parser-Data","Cannnot allocate space of size " + size);
            alloc(size);
            dataList[this.address] = '\0';
        }

        private void init_word(Token[] args)
        {
            alloc(args.Length);
            for(int i=0; i < args.Length; i++)
            {
                dataList[this.address + i] = int.Parse(args[i].value);
            }
        }

        private void init_byte(Token[] args)
        {
            alloc(args.Length);
            for(int i=0; i < args.Length; i++)
            {
                char c = args[i].value.Trim(new char[] {'\''})[0];
                dataList[this.address + i] = (int)c;
            }
        }

        public int get(int index)
        {
            if(index < 0 || index >= Compiler.MemorySize)
                Compiler.Error("RunTime","index out of range");
            return dataList[index];
        }
    }
}
