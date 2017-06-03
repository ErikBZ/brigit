using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brigit.Attributes.Operators
{
	class Variable : IExpression
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

		public void Add(IExpression exp)
		{
			throw new NotImplementedException();
		}

		public Flag Evaluate(Dictionary<string, Flag> locals, Dictionary<string, Flag> globals)
		{
			throw new NotImplementedException();
		}
	}
}
