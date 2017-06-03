using System;
using System.Collections.Generic;

namespace Brigit.Attributes.Operators
{
	// Mux is a better way to describe what this operation does
	public class Mux:IExpression
    {
		private List<IExpression> exp;

		public Mux()
		{
			exp = new List<IExpression>();
		}

		public void Add(IExpression exp)
		{
			throw new NotImplementedException();
		}


		public Flag Evaluate(Dictionary<string, Flag> locals, Dictionary<string, Flag> globals)
		{
			Flag eval = Flag.False;
			
			foreach(IExpression e in exp)
			{
				Flag subEval = e.Evaluate(locals, globals);

				if(eval == Flag.True && subEval == Flag.True)
				{
					return Flag.False;	
				}
				else if(subEval == Flag.Unset)
				{
					return Flag.Unset;
				}
			}

			return eval;
		}
	} 
}