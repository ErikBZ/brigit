using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brigit.Attributes.Operators
{
	public interface IExpression
	{
		Flag Evaluate(Dictionary<string, Flag> locals, Dictionary<string, Flag> globals);
		void Add(IExpression exp);
	}
}
