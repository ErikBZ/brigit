using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Attributes.Operators;
using System.Runtime.Serialization;

namespace Brigit.Attributes
{
	/// <summary>
	/// Manages what attribute a node holds. Attributes are overidden by least specific to most specfic.
	/// A singleton's attributes override a nodes attributs
	/// </summary>
	[KnownType(typeof(Variable))]
	[KnownType(typeof(And))]
	[KnownType(typeof(Mux))]
	[KnownType(typeof(Or))]
	[DataContract]
	public class AttributeManager
	{
		// Flags
		[DataMember]
		public IExpression Expression { get; set; }

		[DataMember]
		public Dictionary<string, Flag> SetFlags { get; set; }

        public Dictionary<string, string> ExtraAttributes { get; set; }

		public AttributeManager()
		{
			SetFlags = new Dictionary<string, Flag>();
            ExtraAttributes = new Dictionary<string, string>();
			Expression = new Variable("TRUE");
		}

        public override bool Equals(object obj)
        {
            bool equal = true;
            if(!(obj is AttributeManager))
            {
                equal = false;
            }
            else
            {
                AttributeManager am = (AttributeManager)obj;
				equal = FlagsAreEqual(SetFlags, am.SetFlags);
				equal &= Expression.Equals(am.Expression);

                foreach( KeyValuePair<string, string> kvp in ExtraAttributes)
                {
                    if (!am.ExtraAttributes.ContainsKey(kvp.Key) || !am.ExtraAttributes[kvp.Key].Equals(kvp.Value))
                    {
                        equal = false;
                        break;
                    }
                }
            }

            return equal;
        }

        public bool FlagsAreEqual(Dictionary<string, Flag> flags1, Dictionary<string, Flag> flags2)
        {
            // checking the equality of the dictionaries
            bool dictionariesEqual = flags1.Count == flags2.Count;
            // if the dictionary counts are not equal then obviously the
            // entries will be different as well. this only runs to check the actual
            // entries if there are the same count of entries.
            if (dictionariesEqual)
            {
                foreach (KeyValuePair<string, Flag> kvp in flags1)
                {
                    if (flags2.ContainsKey(kvp.Key))
                    {
                        // Since we're basically using intergers with for the Flag values
                        // we can just check their equality
                        dictionariesEqual = dictionariesEqual && (kvp.Value == flags2[kvp.Key]);
                    }
                    else
                    {
                        dictionariesEqual = false;
                    }
                }
            }
            return dictionariesEqual;
        }

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
    }
}
