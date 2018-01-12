using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Attributes.Operators;

namespace Brigit.Attributes
{
	public class AttributeManager
	{
		// Flags
		public IExpression Expression { get; set; }

		public Dictionary<string, Flag> SetFlags { get; set; }


        // emote for the saying
        public string Emote { get; set; }

        // duration of the dialog
        public string Length { get; set; }


		public AttributeManager()
		{
			SetFlags = new Dictionary<string, Flag>();

			Emote = string.Empty;

			Length = string.Empty;

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
				equal &= Emote.Equals(am.Emote);
				equal &= Length.Equals(am.Length);
				equal &= Expression.Equals(am.Expression);
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
                        dictionariesEqual = dictionariesEqual && !(kvp.Value == flags2[kvp.Key]);
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
