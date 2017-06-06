using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Attributes.Operators;

namespace Brigit.Attributes
{
	public class AttributeManager
	{
		// Flags
		public IExpression Expression { get; set; }

		public Dictionary<Flag, string> SetFlags { get; set; }


        // emote for the saying
        public string Emote { get; set; }

        // duration of the dialog
        public string Length { get; set; }


		public AttributeManager()
		{
			SetFlags = new Dictionary<Flag, string>();

			Emote = string.Empty;

			Length = string.Empty;

			Expression = null;
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
                equal = FlagsAreEqual(SetFlags, am.SetFlags) &&
                    Emote.Equals(am.Emote) &&
                    Length.Equals(am.Length); 
            }

            return equal;
        }

        public bool FlagsAreEqual(Dictionary<Flag, string> flags1, Dictionary<Flag, string> flags2)
        {
            bool equal = true;
            // checking the equality of the dictionaries
            bool dictionariesEqual = flags1.Count == flags2.Count;
            // if the dictionary counts are not equal then obviously the
            // entries will be different as well. this only runs to check the actual
            // entries if there are the same count of entries.
            if (dictionariesEqual)
            {
                foreach (KeyValuePair<Flag, string> kvp in flags1)
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
            return equal;
        }

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
    }
}
