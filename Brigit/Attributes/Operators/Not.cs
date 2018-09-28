using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brigit.Attributes.Operators
{
    class Not : IExpression
    {
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
