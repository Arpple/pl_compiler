using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PL
{
    public class Executer
    {
#region Static
		private static void Error(Code code,string msg)
		{
			Compiler.Error("Runtime","#" + code.key.lineNumber + "=" + code + ":" + msg);
		}

		private static int getConst(string data)
		{
			if(data[0] == '\'')
				return (int)data[1];
			else
				return int.Parse(data);
		}
#endregion


        private CodeTree tree;
        private CodeNode current;
        private Registers regs;

        public Executer(CodeTree tree)
        {
            Compiler.debug("=====================================");
            Compiler.debug("-- Runtime --");
            this.current = tree.root.next;
            this.regs = new Registers();
            this.tree = tree;

            //Read Data Segment


            //Execute Text Segment
            while(this.current != null)
            {
                execute(current.code);
                next();
            }

            Compiler.debug("\n=====================================");
            Compiler.debug("Debug Result of $t1 : " + regs.get("$t1"));
        }

		private int getRegisterOrConst(string data)
		{
			if(data[0] == '$')
			{
				return regs.get(data);
			}
			else
			{
				return getConst(data);
			}
		}

        private void execute(Code code)
        {
            Compiler.verbose("Executing : "+ current);

            switch(code.key.type)
            {
                case Token.TokenType.Add_KEY : execute_add(code); break;
                case Token.TokenType.Addi_KEY : execute_addi(code); break;
                case Token.TokenType.Sub_KEY : execute_sub(code); break;
                case Token.TokenType.Subi__KEY : execute_subi(code); break;
                case Token.TokenType.Mul_KEY : execute_mul(code); break;
                case Token.TokenType.Muli_KEY : execute_muli(code); break;
                case Token.TokenType.Div_KEY : execute_div(code); break;
                case Token.TokenType.Divi_KEY : execute_divi(code); break;

                case Token.TokenType.And_KEY : execute_and(code); break;
                case Token.TokenType.Andi_KEY : execute_andi(code); break;
                case Token.TokenType.Or_KEY : execute_or(code); break;
                case Token.TokenType.Ori_KEY : execute_ori(code); break;
                case Token.TokenType.Nor_KEY : execute_nor(code); break;

                case Token.TokenType.Beq_KEY : execute_beq(code); break;
                case Token.TokenType.Bnq_KEY : execute_bnq(code); break;
                case Token.TokenType.Blt_KEY : execute_blt(code); break;
                case Token.TokenType.Blte_KEY : execute_blte(code); break;
                case Token.TokenType.Bgt_KEY : execute_bgt(code); break;
                case Token.TokenType.Bgte_KEY : execute_bgte(code); break;

                case Token.TokenType.Jump_KEY : execute_jump(code); break;
                case Token.TokenType.Jal_KEY : execute_jal(code); break;
                case Token.TokenType.Jr_KEY : execute_jr(code); break;

                case Token.TokenType.Load_KEY : execute_load(code); break;
                case Token.TokenType.Save_KEY : execute_save(code); break;
                case Token.TokenType.Li_KEY : execute_li(code); break;
                case Token.TokenType.La_KEY : execute_la(code); break;

                case Token.TokenType.Move_KEY : execute_move(code); break;
                case Token.TokenType.Syscall_KEY : execute_syscall(code); break;
            }
        }

        private void next()
        {
            this.current = this.current.next;
        }

#region Math_Op
        private void execute_add(Code code)
        {
            //add $0 $1 $2 => $0 = $1 + $2
            int result = regs.get(code.value(1)) + regs.get(code.value(2));
            regs.set(code.value(0),result);
        }

        private void execute_addi(Code code)
        {
            //addi $0 $1 x => $0 = $1 + x
			int result = regs.get(code.value(1)) + getConst(code.value(2));
            regs.set(code.value(0),result);
        }

        private void execute_sub(Code code)
        {
            //sub $0 $1 $2 => $0 = $1 - $2
            int result = regs.get(code.value(1)) - regs.get(code.value(2));
            regs.set(code.value(0),result);
        }

        private void execute_subi(Code code)
        {
            //subi $0 $1 x => $0 = $1 - x
            int result = regs.get(code.value(1)) - getConst(code.value(2));
            regs.set(code.value(0),result);
        }

        private void execute_mul(Code code)
        {
            //mul $0 $1 $2 => $0 = $1 * $2
            int result = regs.get(code.value(1)) * regs.get(code.value(2));
            regs.set(code.value(0),result);
        }

        private void execute_muli(Code code)
        {
            //muli $0 $1 x => $0 = $1 * x
            int result = regs.get(code.value(1)) * getConst(code.value(2));
            regs.set(code.value(0),result);
        }

        private void execute_div(Code code)
        {
            //div $0 $1 $2 => $0 = $1 / $2
            int result = regs.get(code.value(1)) / regs.get(code.value(2));
            regs.set(code.value(0),result);
        }

        private void execute_divi(Code code)
        {
            //div $0 $1 x => $0 = $1 / x
            int result = regs.get(code.value(1)) / getConst(code.value(2));
            regs.set(code.value(0),result);
        }
        #endregion

#region Logic_Op
        private void execute_and(Code code)
        {
            //and $0 $1 $2 => $0 = $1 & $2
            int x = regs.get(code.value(1));
            int y = regs.get(code.value(2));
            int result = and(x,y);
            regs.set(code.value(0), result);
        }

        private void execute_andi(Code code)
        {
            //andi $0 $1 x => $0 = $1 & x
            int x = regs.get(code.value(1));
            int y = getConst(code.value(2));
            int result = and(x,y);
            regs.set(code.value(0), result);
        }

        private int and(int x, int y)
        {
            string binary1 = toBinaryString(x);
            string binary2 = toBinaryString(y);
            string result = "";

            for(int i=0; i < 32; i++)
            {
                if(binary1[i] == '1' && binary2[i] == '1')
                    result += "1";
                else
                    result += "0";
            }

            return Convert.ToInt32(result,2);
        }

        private void execute_or(Code code)
        {
            //or $0 $1 $2 => $0 = $1 | $2
            int x = regs.get(code.value(1));
            int y = regs.get(code.value(2));
            int result = or(x,y);
            regs.set(code.value(0), result);
        }

        private void execute_ori(Code code)
        {
            //or $0 $1 $2 => $0 = $1 | $2
            int x = regs.get(code.value(1));
            int y = getConst(code.value(2));
            int result = or(x,y);
            regs.set(code.value(0), result);
        }

        private int or(int x, int y)
        {
            string binary1 = toBinaryString(x);
            string binary2 = toBinaryString(y);
            string result = "";

            for(int i=0; i < 32; i++)
            {
                if(binary1[i] == '1' || binary2[i] == '1')
                    result += "1";
                else
                    result += "0";
            }

            return Convert.ToInt32(result,2);
        }

        private void execute_nor(Code code)
        {
            //or $0 $1 $2 => $0 = $1 | $2
            int x = regs.get(code.value(1));
            int y = regs.get(code.value(2));
            int result = nor(x,y);
            regs.set(code.value(0), result);
        }

        private int nor(int x, int y)
        {
            string binary1 = toBinaryString(x);
            string binary2 = toBinaryString(y);
            string result = "";

            for(int i=0; i < 32; i++)
            {
                if(binary1[i] == '1' || binary2[i] == '1')
                    result += "0";
                else
                    result += "1";
            }

            return Convert.ToInt32(result,2);
        }

        private string toBinaryString(int x)
        {
            return Convert.ToString(x,2).PadLeft(32,'0');
        }
#endregion

#region Branch
        private void execute_beq(Code code)
        {
            //beq $0 $1 addr => if $0 == $1 , jump addr
            bool result = regs.get(code.value(0)) == getRegisterOrConst(code.value(1));
            if(result)
			{
				if(!jump(code.value(2)))
					Error(code,"I can't jump to that!!");
			}
        }

        private void execute_bnq(Code code)
        {
            //beq $0 $1 addr => if $0 != $1 , jump addr
            bool result = regs.get(code.value(0)) != getRegisterOrConst(code.value(1));
            if(result)
			{
				if(!jump(code.value(2)))
					Error(code,"I can't jump to that!!");
			}
        }

        private void execute_blt(Code code)
        {
            //blt $0 $1 addr => if $0 < $1 , jump addr
            bool result = regs.get(code.value(0)) < getRegisterOrConst(code.value(1));
            if(result)
			{
				if(!jump(code.value(2)))
					Error(code,"I can't jump to that!!");
			}
        }

        private void execute_blte(Code code)
        {
            //blte $0 $1 addr => if $0 <= $1 , jump addr
            bool result = regs.get(code.value(0)) <= getRegisterOrConst(code.value(1));
            if(result)
			{
				if(!jump(code.value(2)))
					Error(code,"I can't jump to that!!");
			}
        }

        private void execute_bgt(Code code)
        {
            //bgt $0 $1 addr => if $0 > $1 , jump addr
            bool result = regs.get(code.value(0)) > getRegisterOrConst(code.value(1));
            if(result)
			{
				if(!jump(code.value(2)))
					Error(code,"I can't jump to that!!");
			}
        }

        private void execute_bgte(Code code)
        {
            //bgte $0 $1 addr => if $0 >= $1 , jump addr
            bool result = regs.get(code.value(0)) >= getRegisterOrConst(code.value(1));
            if(result)
			{
				if(!jump(code.value(2)))
					Error(code,"I can't jump to that!!");
			}
        }
#endregion

#region Jump
        private void execute_jump(Code code)
        {
            //jump addr
            if(!jump(code.value(0)))
				Error(code,"I can't jump to that!!");
        }

        private void execute_jal(Code code)
        {
            //jal addr => set $ra = current ,then jump
            regs.set("$ra",current.address);
            if(!jump(code.value(0)))
				Error(code,"I can't jump to that!!");
        }

        private void execute_jr(Code code)
        {
            //jr $0 => jump to address stored in reg
            if(!jumpAddress(regs.get(code.value(0))))
				Error(code,"I can't jump to that!!");
        }

        private bool jump(string address)
        {
            CodeNode node = this.tree.getNodeFromLabel(address + ":");
            if(node != null)
            {
                this.current = node;
				return true;
            }
            else
            {
                return false;
            }
        }

        private bool jumpAddress(int address)
        {
            CodeNode node = this.tree.getNodeFromAddress(address);
            if(node != null)
            {
				this.current = node;
                return true;
            }
            else
            {
				return false;
            }
        }
#endregion

#region Save/Load
        private void execute_load(Code code)
        {
            //load $0 , offset(word) => $0 = load(word[offset])
            string[] addr_str = code.value(1).Split(new char[]{'(',')'});
            int offset = 0;
            if(addr_str[0].Length > 0)
                offset = int.Parse(addr_str[0]);
            int addr = regs.get(addr_str[1]);
            int dt = Data.getData(addr,offset);
            regs.set(code.value(0),dt);
        }

        private void execute_save(Code code)
        {
            //save $0 , offset(word) => word[offset] = $0
            string[] addr_str = code.value(1).Split(new char[]{'(',')'});
            int offset = 0;
            if(addr_str[0].Length > 0)
                offset = int.Parse(addr_str[0]);
            int addr = regs.get(addr_str[1]);
            int val = regs.get(code.value(0));
            Data.setData(addr,offset,val);
        }

        private void execute_li(Code code)
        {
            //li $0 , x => $0 = x
			string value = code.value(1);
			if(value[0] == '\'')
			{
				regs.set(code.value(0),(int)value[1]);
			}
			else
			{
				regs.set(code.value(0),int.Parse(value));
			}
        }

        private void execute_la(Code code)
        {
            //la $0 , label => $0 = address of label
            int addr = Data.getAddress(code.value(1) + ":");
			if(addr < 0)
				Error(code,"address '" + code.value(1) + "' cannot be found");
            regs.set(code.value(0),addr);
        }
#endregion

#region Syscall
        private void execute_move(Code code)
        {
            //move $0 , $1 => $0 = 1
            int val = regs.get(code.value(1));
            regs.set(code.value(0), val);
        }

        enum Syscall
        {
            Print_Int = 1,
            Print_String = 4,
            Read_Int = 5,
            Read_String = 8,
            Exit = 10
        }

        private void execute_syscall(Code code)
        {
            int op_code = regs.get("$v0");
            switch((Syscall)op_code)
            {
                case Syscall.Print_Int : syscall_print_int(); break;
                case Syscall.Print_String : syscall_print_string(); break;
                case Syscall.Read_Int : syscall_read_int(); break;
                case Syscall.Read_String : syscall_read_string(); break;
                case Syscall.Exit : syscall_exit(); break;
                default :
                    Error(code,"I can't do syscall with code " + op_code);
                    break;
            }
        }

        private void syscall_print_int()
        {
            Console.Write(regs.get("$a0"));
        }

        private void syscall_print_string()
        {
            string str = "";
            int i = regs.get("$a0");
            while(true)
            {
                char c = (char)Data.getData(i);
                if(c == '\0')
                    break;
                else
                {
                    str += c;
                    i++;
                }
            }
            Console.Write(str);
        }

        private void syscall_read_int()
        {
            int input = 0;
            int.TryParse(Console.ReadLine(),out input);
            regs.set("$v0", input);
        }

        private void syscall_read_string()
        {
            string input = "";
            int bufferSize = regs.get("$a1");

            input = Console.ReadLine();
            int index = regs.get("$a0");
            int i = 0;
            for(; i < input.Length &&  i < bufferSize - 1; i++)
            {
                Data.setData(index,i,(int)input[i]);
            }
            Data.setData(index,i,(int)'\0');
        }

        private void syscall_exit()
        {
            Compiler.Success("executing completed (exit by syscall)");
            System.Environment.Exit(0);
        }
#endregion
    }
}
