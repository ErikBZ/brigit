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
			throw new NotImplementedException();
		}
	} 
}