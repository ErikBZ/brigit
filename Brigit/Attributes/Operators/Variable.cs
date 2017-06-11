﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brigit.Attributes.Operators
{
	public class Variable : IExpression
	{
		string variableName;

		public Variable()
		{
			variableName = string.Empty;
		}

		public Variable(string name)
		{
			variableName = name;
		}

		// nothing can be added to variables
		// maybe return an error?
		public void Add(IExpression exp)
		{
			return;
		}

		public Flag Evaluate(Dictionary<string, Flag> locals, Dictionary<string, Flag> globals)
		{
			if (locals.ContainsKey(variableName))
			{
				return locals[variableName];
			}
			else if(globals.ContainsKey(variableName))
			{
				return globals[variableName];
			}
			else
			{
				// should throw an exception here
				return Flag.Unset;
			}
		}

		// this is the base case for all IExpressions
		public override bool Equals(object obj)
		{
			if(!(obj is Variable))
			{
				return false;
			}

			return ((Variable)obj).variableName == this.variableName;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}