using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Brigit.Attributes.Operators
{
	[DataContract]
	public class Variable : IExpression
	{
		[DataMember]
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
            if (variableName == "TRUE")
            {
                return Flag.True;
            }
			else if (locals != null && locals.ContainsKey(variableName))
			{
				return locals[variableName];
			}
			else if(globals != null && globals.ContainsKey(variableName))
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
