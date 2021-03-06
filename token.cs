using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PL
{
	public class Token
	{
#region database
		public enum TokenType
		{
			//data key
			Asciiz_KEY,
			Word_KEY,
			Byte_KEY,
			Space_KEY,

			//function key
			Data_KEY,
			Text_KEY,

			Add_KEY,
			Addi_KEY,
			Sub_KEY,
			Subi__KEY,
			Mul_KEY,
			Muli_KEY,
			Div_KEY,
			Divi_KEY,

			And_KEY,
			Andi_KEY,
			Or_KEY,
			Ori_KEY,
			Nor_KEY,

			Beq_KEY,
			Bnq_KEY,
			Blt_KEY,
			Blte_KEY,
			Bgt_KEY,
			Bgte_KEY,

			Jump_KEY,
			Jal_KEY,
			Jr_KEY,

			Load_KEY,
			Save_KEY,
			Li_KEY,
			La_KEY,

			Move_KEY,
			Syscall_KEY,

			//terminal key
			Comma_KEY,
			Endl_KEY,

			//constant key
			Register,
			Int,
			String,
			Char,
			Const,
			Address,
			Register_Or_Const,
			Label_Address,
			Label
		}

		private static Dictionary<TokenType,string> regEx;

		public static void initRegex()
		{
			regEx = new Dictionary<TokenType,string>();

			if(Compiler.Language == "EN")
			{
				regEx.Add(TokenType.Asciiz_KEY,"^\\.asciiz$");
				regEx.Add(TokenType.Word_KEY,"^\\.word$");
				regEx.Add(TokenType.Byte_KEY,"^\\.byte$");
				regEx.Add(TokenType.Space_KEY,"^\\.space$");

				regEx.Add(TokenType.Data_KEY,"^\\.data$");
				regEx.Add(TokenType.Text_KEY,"^\\.text$");

				regEx.Add(TokenType.Add_KEY,"^add$");
				regEx.Add(TokenType.Addi_KEY,"^addi$");
				regEx.Add(TokenType.Sub_KEY,"^sub$");
				regEx.Add(TokenType.Subi__KEY,"^subi$");
				regEx.Add(TokenType.Mul_KEY,"^mul$");
				regEx.Add(TokenType.Muli_KEY,"^muli$");
				regEx.Add(TokenType.Div_KEY,"^div$");
				regEx.Add(TokenType.Divi_KEY,"^divi$");

				regEx.Add(TokenType.And_KEY,"^and$");
				regEx.Add(TokenType.Andi_KEY,"^andi$");
				regEx.Add(TokenType.Or_KEY,"^or$");
				regEx.Add(TokenType.Ori_KEY,"^ori$");
				regEx.Add(TokenType.Nor_KEY,"^nor$");

				regEx.Add(TokenType.Beq_KEY,"^beq$");
				regEx.Add(TokenType.Bnq_KEY,"^bnq$");
				regEx.Add(TokenType.Blt_KEY,"^blt$");
				regEx.Add(TokenType.Blte_KEY,"^blte$");
				regEx.Add(TokenType.Bgt_KEY,"^bgt$");
				regEx.Add(TokenType.Bgte_KEY,"^bgte$");

				regEx.Add(TokenType.Jump_KEY,"^jump$");
				regEx.Add(TokenType.Jal_KEY,"^jal$");
				regEx.Add(TokenType.Jr_KEY,"^jr$");

				regEx.Add(TokenType.Load_KEY,"^load$");
				regEx.Add(TokenType.Save_KEY,"^save$");
				regEx.Add(TokenType.Li_KEY,"^li$");
				regEx.Add(TokenType.La_KEY,"^la$");

				regEx.Add(TokenType.Move_KEY,"^move$");
				regEx.Add(TokenType.Syscall_KEY,"^syscall$");

				regEx.Add(TokenType.Comma_KEY,"^,$");
				regEx.Add(TokenType.Endl_KEY,"^$");

				regEx.Add(TokenType.Register,"^\\$[0-9A-Za-z]*$");
				regEx.Add(TokenType.Int,"^(-)?[0-9]+$");
				regEx.Add(TokenType.String,"\".*\"");
				regEx.Add(TokenType.Char,"^\\\'.\\\'$");
				regEx.Add(TokenType.Const,"^(-)?[0-9]+$|^\\\'.\\\'$");
				regEx.Add(TokenType.Address,"^[0-9]*?\\(\\$[0-9A-Za-z]+\\)$");
				regEx.Add(TokenType.Label,"^[a-zA-Z0-9_]+:$");
				regEx.Add(TokenType.Label_Address,"^[a-zA-Z0-9_]+$");
				regEx.Add(Token.TokenType.Register_Or_Const,"^\\$[0-9A-Za-z]*$|^(-)?[0-9]+$|^\\\'.\\\'$");
			}
			else if (Compiler.Language == "TH")
			{
				regEx.Add(TokenType.Asciiz_KEY,"^\\.แอสกี้$");
				regEx.Add(TokenType.Word_KEY,"^\\.คำ$");
				regEx.Add(TokenType.Byte_KEY,"^\\.ไบท์$");
				regEx.Add(TokenType.Space_KEY,"^\\.จักรวาล$");

				regEx.Add(TokenType.Data_KEY,"^\\.ข้อมูล$");
				regEx.Add(TokenType.Text_KEY,"^\\.ข้อความ$");

				regEx.Add(TokenType.Add_KEY,"^บวก$");
				regEx.Add(TokenType.Addi_KEY,"^บวกอิ$");
				regEx.Add(TokenType.Sub_KEY,"^ลบ$");
				regEx.Add(TokenType.Subi__KEY,"^ลบอิ$");
				regEx.Add(TokenType.Mul_KEY,"^มูล$");
				regEx.Add(TokenType.Muli_KEY,"^มูลอิ$");
				regEx.Add(TokenType.Div_KEY,"^ดริฟต์$");
				regEx.Add(TokenType.Divi_KEY,"^ดริฟต์อิ$");

				regEx.Add(TokenType.And_KEY,"^แอน");
				regEx.Add(TokenType.Andi_KEY,"^แอนดี้$");
				regEx.Add(TokenType.Or_KEY,"^ออ$");
				regEx.Add(TokenType.Ori_KEY,"^อออิ$");
				regEx.Add(TokenType.Nor_KEY,"^นอ$");

				regEx.Add(TokenType.Beq_KEY,"^แยกอีคั่ว$");
				regEx.Add(TokenType.Bnq_KEY,"^แยกไม่อีคั่ว$");
				regEx.Add(TokenType.Blt_KEY,"^แยกน้อย$");
				regEx.Add(TokenType.Blte_KEY,"^แยกน้อยอีคั่ว$");
				regEx.Add(TokenType.Bgt_KEY,"^แยกมาก$");
				regEx.Add(TokenType.Bgte_KEY,"^แยกมากอีคั่ว$");

				regEx.Add(TokenType.Jump_KEY,"^พุ่ง$");
				regEx.Add(TokenType.Jal_KEY,"^พุ่งเสียบ$");
				regEx.Add(TokenType.Jr_KEY,"^พุ่งร์$");

				regEx.Add(TokenType.Load_KEY,"^โหลด$");
				regEx.Add(TokenType.Save_KEY,"^เสพ$");
				regEx.Add(TokenType.Li_KEY,"^ลิ$");
				regEx.Add(TokenType.La_KEY,"^ละ$");

				regEx.Add(TokenType.Move_KEY,"^หมูบ$");
				regEx.Add(TokenType.Syscall_KEY,"^ซิสคอล$");

				regEx.Add(TokenType.Comma_KEY,"^,$");
				regEx.Add(TokenType.Endl_KEY,"^$");

				regEx.Add(TokenType.Register,"^\\$[0-9A-Za-z]*$");
				regEx.Add(TokenType.Int,"^(-)?[0-9]+$");
				regEx.Add(TokenType.String,"\".*\"");
				regEx.Add(TokenType.Char,"^\\\'.\\\'$");
				regEx.Add(TokenType.Const,"^(-)?[0-9]+$|^\\\'.\\\'$");
				regEx.Add(TokenType.Address,"^[0-9]*?\\(\\$[0-9A-Za-z]+\\)$");
				regEx.Add(TokenType.Label,"^[a-zA-Z0-9_ก-๙]+:$");
				regEx.Add(TokenType.Label_Address,"^[a-zA-Z0-9_ก-๙]+$");
				regEx.Add(Token.TokenType.Register_Or_Const,"^\\$[0-9A-Za-z]*$|^(-)?[0-9]+$|^\\\'.\\\'$");
			}
		}

#endregion

		public TokenType type;
		public string value;
		public int lineNumber;

		public static Token construct(string word,int lineNumber)
		{
			Compiler.verbose("matching " + word);
			Array rxs = Enum.GetValues(typeof(TokenType));
			foreach(TokenType type in rxs)
			{
				string rx_str;
				regEx.TryGetValue(type,out rx_str);
				Regex rx = new Regex(rx_str);
				if(rx.IsMatch(word))
				{
					Compiler.verbose("=" + type);
					return new Token(type,word,lineNumber);
				}
			}
			return null;
		}

		public Token(TokenType type, string value, int lineNumber)
		{
			this.type = type;
			this.value = value;
			this.lineNumber = lineNumber;
		}

		public void print(bool printValue = false)
		{
			string msg = "<" + this.type.ToString();
			if(printValue) msg += "," + this.value;
			msg += ">";
			Console.Write(msg + " ");
		}

		public override string ToString()
		{
			return "<" + this.type + ":" + this.value + ">";
		}
	}
}
