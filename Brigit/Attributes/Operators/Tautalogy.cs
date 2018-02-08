using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brigit.Attributes.Operators
{
    class Tautalogy : IExpression
    {
        public void Add(IExpression exp)
        {
            return;
        }

        public Flag Evaluate(Dictionary<string, Flag> locals, Dictionary<string, Flag> globals)
        {
            return Flag.True;
        }

        public override bool Equals(object obj)
        {
            return obj is Tautalogy;
        }
    }
}
