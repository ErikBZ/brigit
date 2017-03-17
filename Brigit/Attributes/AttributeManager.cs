using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brigit.Attributes
{
    class AttributeManager
    {
        // Flags
        Dictionary<string, Flag> RequiredLocalFlags;
        Dictionary<string, Flag> RequiredGlobalFlags;
        Dictionary<string, Flag> SetLocalFlags;
        Dictionary<string, Flag> SetGlobalFlags;

        // emote for the saying
        string Emote;

        // duration of the dialog
        string Length;

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
                equal = FlagsAreEqual(RequiredGlobalFlags, am.RequiredGlobalFlags) &&
                    FlagsAreEqual(RequiredLocalFlags, am.RequiredLocalFlags) &&
                    FlagsAreEqual(SetLocalFlags, am.SetLocalFlags) &&
                    FlagsAreEqual(SetGlobalFlags, am.SetGlobalFlags) &&
                    Emote.Equals(am.Emote) &&
                    Length.Equals(am.Length); ;
            }

            return equal;
        }

        public bool FlagsAreEqual(Dictionary<string, Flag> flags1, Dictionary<string, Flag> flags2)
        {
            bool equal = true;
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
            return equal;
        }
    }
}
