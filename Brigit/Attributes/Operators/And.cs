using System;
using System.Collections.Generic;

namespace Brigit.Attributes.Operators
{
    public class And:IExpression
    {
		private List<IExpression> exp;

        public And()
        {
			exp = new List<IExpression>();
        }

		public void Add(IExpression exp)
		{
			throw new NotImplementedException();
		}

		public Flag Evaluate(Dictionary<string, Flag> locals, Dictionary<string, Flag> globals)
		{
			Flag eval = Flag.True;

			foreach(IExpression e in exp)
			{
				Flag subEval = e.Evaluate(locals, globals);
				if(subEval == Flag.False)
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