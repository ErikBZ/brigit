using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Brigit.Attributes.Operators
{
    public class LessThan : IExpression
    {
        [DataMember]
        private List<IExpression> ExpList;

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