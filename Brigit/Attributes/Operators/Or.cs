using System;
using System.Collections.Generic;

namespace Brigit.Attributes.Operators
{
	public class Or : IExpression
	{
		private List<IExpression> ExpList;
		public Or()
		{
			ExpList = new List<IExpression>();
		}

		public void Add(IExpression exp)
		{
			ExpList.Add(exp);
		}

		public Flag Evaluate(Dictionary<string, Flag> locals, Dictionary<string, Flag> globals)
		{
			Flag evaluation = Flag.False;
			foreach (IExpression e in ExpList)
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