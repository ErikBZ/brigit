using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Attributes;
using Brigit.Attributes.Operators;
using Brigit.Attributes.ExpressionParser;

namespace Brigit.TomeParser
{
	/// <summary>
	/// Parses the Attributes of any node or for a choice. Each set of attributes is delimited by a ':' and
	/// lists within the value are delimted by spaces
	/// This method should start at a '['
	/// </summary>
	public partial class TomeParser
	{
		// i can add more when i need it
		const string REQFLAGS = "ReqFlags";
		const string TRUEFLAGS = "SFlagsTrue";
		const string FALSEFLAGS = "SFlagsFalse";
		const string DONTCARE = "SFlagDC";
		const string SETEMOTE = "SEmote";

		public AttributeManager ParseAttributes()
		{
			AttributeManager am = new AttributeManager();

			// grabs block to be parsed by 
			string stringToParse = GrabSubstringToParse();

			// nothing was found for some reason
			if(stringToParse == string.Empty)
			{
				return null;
			}

			string[] attributes = stringToParse.Split(',');

			// looking for keywords
			foreach (string s in attributes)
			{
				string[] KeywordAndValue = s.Split(':');
				string keyword = KeywordAndValue[0];
				string value = KeywordAndValue[1];

				if (s.StartsWith(REQFLAGS) || s.StartsWith(" " + REQFLAGS))
				{
					am.Expression = ParseRequiredFlags(value);
				}
				else if (s.StartsWith(TRUEFLAGS) || s.StartsWith(" " + TRUEFLAGS))
				{
					string[] flagsToSetTrue = value.Split(' ');
					SetFlagArrayTo(Flag.True, flagsToSetTrue, am);
				}
				else if (s.StartsWith(FALSEFLAGS) || s.StartsWith(" " + FALSEFLAGS))
				{
					string[] falseFlags = value.Split(' ');
					SetFlagArrayTo(Flag.False, falseFlags, am);
				}
				else if (s.StartsWith(DONTCARE) || s.StartsWith(" " + DONTCARE))
				{
					string[] dontCareFlags = value.Split(' ');
					SetFlagArrayTo(Flag.DontCare, dontCareFlags, am);
				}
				else if (s.StartsWith(SETEMOTE) || s.StartsWith(" " + SETEMOTE))
				{
					am.Emote = value;
				}
			}

			return am;
		}

		private void SetFlagArrayTo(Flag f, String[] flagNames, AttributeManager am)
		{
			foreach(string name in flagNames)
			{
				am.SetFlags.Add(f, name);
			}
		}
		private string GrabSubstringToParse()
		{
			return muncher.SpitUpWhile((c) => c != ']');
		}

		private IExpression ParseRequiredFlags(string str)
		{
			return Parser.Parse(str);
		}
	}
}
