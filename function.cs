using System;
using System.Collections.Generic;

namespace PL
{
	public class Function
	{
		public Token.TokenType key;
		public List<Token.TokenType> args;

		public Function(Token.TokenType key, params Token.TokenType[] args)
		{
			this.key = key;
			this.args = new List<Token.TokenType>();
			foreach(Token.TokenType arg in args)
			{
				this.args.Add(arg);
			}
		}

		private static Dictionary<Token.TokenType,Function> _function;
		public static void initFunction()
		{
			_function = new Dictionary<Token.TokenType,Function>();

			_function.Add(Token.TokenType.Add_KEY,new Function(Token.TokenType.Add_KEY,Token.TokenType.Register,Token.TokenType.Register,Token.TokenType.Register));
			_function.Add(Token.TokenType.Addi_KEY,new Function(Token.TokenType.Addi_KEY,Token.TokenType.Register,Token.TokenType.Register,Token.TokenType.Const));
			_function.Add(Token.TokenType.Sub_KEY,new Function(Token.TokenType.Sub_KEY,Token.TokenType.Register,Token.TokenType.Register,Token.TokenType.Register));
			_function.Add(Token.TokenType.Subi__KEY,new Function(Token.TokenType.Subi__KEY,Token.TokenType.Register,Token.TokenType.Register,Token.TokenType.Const));
			_function.Add(Token.TokenType.Mul_KEY,new Function(Token.TokenType.Mul_KEY,Token.TokenType.Register,Token.TokenType.Register,Token.TokenType.Register));
			_function.Add(Token.TokenType.Muli_KEY,new Function(Token.TokenType.Muli_KEY,Token.TokenType.Register,Token.TokenType.Register,Token.TokenType.Const));
			_function.Add(Token.TokenType.Div_KEY,new Function(Token.TokenType.Div_KEY,Token.TokenType.Register,Token.TokenType.Register,Token.TokenType.Register));
			_function.Add(Token.TokenType.Divi_KEY,new Function(Token.TokenType.Divi_KEY,Token.TokenType.Register,Token.TokenType.Register,Token.TokenType.Const));

			_function.Add(Token.TokenType.And_KEY,new Function(Token.TokenType.And_KEY,Token.TokenType.Register,Token.TokenType.Register,Token.TokenType.Register));
			_function.Add(Token.TokenType.Andi_KEY,new Function(Token.TokenType.Andi_KEY,Token.TokenType.Register,Token.TokenType.Register,Token.TokenType.Const));
			_function.Add(Token.TokenType.Or_KEY,new Function(Token.TokenType.Or_KEY,Token.TokenType.Register,Token.TokenType.Register,Token.TokenType.Register));
			_function.Add(Token.TokenType.Ori_KEY,new Function(Token.TokenType.Ori_KEY,Token.TokenType.Register,Token.TokenType.Register,Token.TokenType.Const));
			_function.Add(Token.TokenType.Nor_KEY,new Function(Token.TokenType.Nor_KEY,Token.TokenType.Register ,Token.TokenType.Register,Token.TokenType.Register));

			_function.Add(Token.TokenType.Beq_KEY,new Function(Token.TokenType.Beq_KEY,Token.TokenType.Register,Token.TokenType.Register_Or_Const,Token.TokenType.Label_Address));
			_function.Add(Token.TokenType.Bnq_KEY,new Function(Token.TokenType.Bnq_KEY,Token.TokenType.Register,Token.TokenType.Register_Or_Const,Token.TokenType.Label_Address));
			_function.Add(Token.TokenType.Blt_KEY,new Function(Token.TokenType.Blt_KEY,Token.TokenType.Register,Token.TokenType.Register_Or_Const,Token.TokenType.Label_Address));
			_function.Add(Token.TokenType.Blte_KEY,new Function(Token.TokenType.Blte_KEY,Token.TokenType.Register,Token.TokenType.Register_Or_Const,Token.TokenType.Label_Address));
			_function.Add(Token.TokenType.Bgt_KEY,new Function(Token.TokenType.Bgt_KEY,Token.TokenType.Register,Token.TokenType.Register_Or_Const,Token.TokenType.Label_Address));
			_function.Add(Token.TokenType.Bgte_KEY,new Function(Token.TokenType.Bgte_KEY,Token.TokenType.Register,Token.TokenType.Register_Or_Const,Token.TokenType.Label_Address));

			_function.Add(Token.TokenType.Jump_KEY,new Function(Token.TokenType.Jump_KEY,Token.TokenType.Label_Address));
			_function.Add(Token.TokenType.Jal_KEY,new Function(Token.TokenType.Jal_KEY,Token.TokenType.Label_Address));
			_function.Add(Token.TokenType.Jr_KEY,new Function(Token.TokenType.Jr_KEY,Token.TokenType.Register));

			_function.Add(Token.TokenType.Load_KEY,new Function(Token.TokenType.Load_KEY,Token.TokenType.Register,Token.TokenType.Address));
			_function.Add(Token.TokenType.Save_KEY,new Function(Token.TokenType.Save_KEY,Token.TokenType.Register,Token.TokenType.Address));
			_function.Add(Token.TokenType.Li_KEY,new Function(Token.TokenType.Li_KEY,Token.TokenType.Register,Token.TokenType.Const));
			_function.Add(Token.TokenType.La_KEY,new Function(Token.TokenType.La_KEY,Token.TokenType.Register,Token.TokenType.Label_Address));

			_function.Add(Token.TokenType.Move_KEY,new Function(Token.TokenType.Move_KEY,Token.TokenType.Register,Token.TokenType.Register));
			_function.Add(Token.TokenType.Syscall_KEY,new Function(Token.TokenType.Syscall_KEY));
		}

		public static Function get(Token.TokenType key)
		{
			Function f;
			_function.TryGetValue(key,out f);
			return f;
		}
	}
}
