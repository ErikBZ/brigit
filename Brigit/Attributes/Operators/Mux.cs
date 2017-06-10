using System;
using System.Collections.Generic;

namespace Brigit.Attributes.Operators
{
	// Mux is a better way to describe what this operation does
	public class Mux:IExpression
    {
		private List<IExpression> ExpList;

		public Mux()
		{
			ExpList = new List<IExpression>();
		}

		public void Add(IExpression exp)
		{
			ExpList.Add(exp);
		}


		public Flag Evaluate(Dictionary<string, Flag> locals, Dictionary<string, Flag> globals)
		{
			Flag eval = Flag.False;
			
			foreach(IExpression e in ExpList)
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

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if(!(obj is Mux))
			{
				return false;
			}

			Mux other = (Mux)obj;
			bool subExpressionsEqual = true;

			int i = 0;
			while(i < ExpList.Count && subExpressionsEqual)
			{
				subExpressionsEqual = this.ExpList[i].Equals(other.ExpList[i]);
				i++;
			}

			return subExpressionsEqual;
		}
	} 
}