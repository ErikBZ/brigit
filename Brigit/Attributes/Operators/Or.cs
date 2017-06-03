using System;
using System.Collections.Generic;

namespace Brigit.Attributes.Operators
{
	public class Or : IExpression
	{
		private List<IExpression> exp;
		public Or()
		{
			exp = new List<IExpression>();
		}

		public void Add(IExpression exp)
		{
			exp.Add(exp);
		}

		public Flag Evaluate(Dictionary<string, Flag> locals, Dictionary<string, Flag> globals)
		{
			Flag evaluation = Flag.False;
			foreach (IExpression e in exp)
			{
				if(e.Evaluate(locals, globals) == Flag.True)
				{
					return Flag.True;
				}
				else if(e.Evaluate(locals, globals) == Flag.Unset)
				{
					return Flag.Unset;
				}
			}

			return evaluation;
		}
	}
}