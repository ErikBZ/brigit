using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Brigit.Attributes.Operators
{
	[DataContract]
    public class And:IExpression
    {
		[DataMember]
		private List<IExpression> ExpList;

        public And()
        {
			ExpList = new List<IExpression>();
        }

		public void Add(IExpression exp)
		{
			ExpList.Add(exp);
		}

		public Flag Evaluate(Dictionary<string, Flag> locals, Dictionary<string, Flag> globals)
		{
			Flag eval = Flag.True;

			foreach(IExpression e in ExpList)
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

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if(!(obj is And))
			{
				return false;
			}

			And other = (And)obj;
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