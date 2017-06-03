using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brigit.Attributes.Operators
{
	public interface IExpression
	{
		Flag Evaluate(Dictionary<string, Flag> locals, Dictionary<string, Flag> globals);
		void Add(IExpression exp);
	}
}
