using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PL
{
    public class Executer
    {
        private CodeTree tree;
        private CodeNode current;
        private Registers regs;

        public Executer(CodeTree tree)
        {
            Compiler.log("=====================================");
            Compiler.log("-- Runtime --");
            this.current = tree.root.next;
            this.regs = new Registers();
            this.tree = tree;

            while(this.current != null)
            {
                execute(current.code);
                next();
            }

            Compiler.log("=====================================");
            Compiler.log("Debug Result of $t1 : " + regs.get("$t1"));
        }

        private void execute(Code code)
        {
            Compiler.log("Executing : "+ current);

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
            int result = regs.get(code.value(1)) + int.Parse(code.value(2));
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
            int result = regs.get(code.value(1)) - int.Parse(code.value(2));
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
            int result = regs.get(code.value(1)) * int.Parse(code.value(2));
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
            int result = regs.get(code.value(1)) / int.Parse(code.value(2));
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
            int y = int.Parse(code.value(2));
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
            int y = int.Parse(code.value(2));
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
            bool result = regs.get(code.value(0)) == regs.get(code.value(1));
            if(result) jump(code.value(2));
        }

        private void execute_bnq(Code code)
        {
            //beq $0 $1 addr => if $0 != $1 , jump addr
            bool result = regs.get(code.value(0)) != regs.get(code.value(1));
            if(result) jump(code.value(2));
        }

        private void execute_blt(Code code)
        {
            //blt $0 $1 addr => if $0 < $1 , jump addr
            bool result = regs.get(code.value(0)) < regs.get(code.value(1));
            if(result) jump(code.value(2));
        }

        private void execute_blte(Code code)
        {
            //blte $0 $1 addr => if $0 <= $1 , jump addr
            bool result = regs.get(code.value(0)) <= regs.get(code.value(1));
            if(result) jump(code.value(2));
        }

        private void execute_bgt(Code code)
        {
            //bgt $0 $1 addr => if $0 > $1 , jump addr
            bool result = regs.get(code.value(0)) > regs.get(code.value(1));
            if(result) jump(code.value(2));
        }

        private void execute_bgte(Code code)
        {
            //bgte $0 $1 addr => if $0 >= $1 , jump addr
            bool result = regs.get(code.value(0)) >= regs.get(code.value(1));
            if(result) jump(code.value(2));
        }
#endregion

#region Jump
        public static Regex labelAdress = new Regex("^[0-9A-Za-z]+$");

        private void execute_jump(Code code)
        {
            //jump addr
            jump(code.value(0));
        }

        private void execute_jal(Code code)
        {
            //jal addr => set $ra = current ,then jump
            regs.set("$ra",current.address);
            jump(code.value(0));
        }

        private void execute_jr(Code code)
        {
            //jr $0 => jump to address stored in reg
            jumpAddress(regs.get(code.value(0)));
        }

        private void jump(string address)
        {
            if(labelAdress.IsMatch(address))
            {
                CodeNode node = this.tree.getNodeFromLabel(address + ":");
                if(node != null)
                {
                    this.current = node;
                }
                else
                {
                    Compiler.Error("Runtime","address " + address + " not found");
                }
            }
        }

        private void jumpAddress(int address)
        {
            CodeNode node = this.tree.getNodeFromAddress(address);
            if(node != null)
            {
                this.current = node;
            }
            else
            {
                Compiler.Error("Runtime","address x" + address + " not found");
            }
        }

#endregion
    }
}
