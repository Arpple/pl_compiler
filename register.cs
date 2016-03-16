using System;
using System.Collections.Generic;

namespace PL
{
	public class Registers
	{
		public Dictionary<string,int> regs;

		public Registers()
		{
			regs = new Dictionary<string,int>();

			regs.Add("$zero", 0);
			regs.Add("$at", 0);
			regs.Add("$a0", 0);
			regs.Add("$a1", 0);
			regs.Add("$a2", 0);
			regs.Add("$a3", 0);
			regs.Add("$t0", 0);
			regs.Add("$t1", 0);
			regs.Add("$t2", 0);
			regs.Add("$t3", 0);
			regs.Add("$t4", 0);
			regs.Add("$t5", 0);
			regs.Add("$t6", 0);
			regs.Add("$t7", 0);
			regs.Add("$s0", 0);
			regs.Add("$s1", 0);
			regs.Add("$s2", 0);
			regs.Add("$s3", 0);
			regs.Add("$s4", 0);
			regs.Add("$s5", 0);
			regs.Add("$s6", 0);
			regs.Add("$s7", 0);
			regs.Add("$t8", 0);
			regs.Add("$t9", 0);
			regs.Add("$ra", 0);
			regs.Add("$v0", 0);
			regs.Add("$sp", 0);
		}

		public void set(string registerName, int value)
		{
			if(registerName == "$0") registerName = "$zero";
			if(regs.ContainsKey(registerName))
			{
				regs[registerName] = value;
			}
			else
			{
				Compiler.Error("Runtime","register " + registerName + " not found");
			}
		}

		public int get(string registerName)
		{
			if(registerName == "$0") registerName = "$zero";
			if(regs.ContainsKey(registerName))
			{
				return regs[registerName];
			}
			else
			{
				Compiler.Error("Runtime","register " + registerName + " not found");
			}
			return 0;
		}
	}
}
