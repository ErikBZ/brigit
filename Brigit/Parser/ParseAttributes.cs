using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Attributes;
using Brigit.Parser.Stream;
using Brigit.Attributes.ExpressionParser;
using Brigit.Attributes.Operators;

namespace Brigit.Parser
{
	public partial class BrigitParser
	{
		// i can add more when i need it
		const string REQFLAGS = "ReqFlg";
		const string TRUEFLAGS = "SetT";
		const string FALSEFLAGS = "SetF";
		const string DONTCARE = "SetDC";
		const string SETEMOTE = "Emt";

		// starts at ( and eats away the closing paren
		public static AttributeManager ParseAttributes(TomeStream stream)
		{
			AttributeManager am = new AttributeManager();

			// since we're in here we assume this is the [ character
			AssertChar(stream, '[');

			string attributesString = GrabSubstringToParse(stream);
			// nothing was found for some reason
			if (attributesString == string.Empty)
			{
				AssertChar(stream, ']');
				// just return a new manager with nothing in it
				return am;
			}

			string[] attributes = attributesString.Split(',');

			// looking for keywords
			foreach (string s in attributes)
			{
				string[] KeywordAndValue = s.Split(':');
				string keyword = KeywordAndValue[0];
				string value = KeywordAndValue[1];

				if (keyword.StartsWith(REQFLAGS) || keyword.StartsWith(" " + REQFLAGS))
				{
					am.Expression = ParseRequiredFlags(value);
				}
				else if (keyword.StartsWith(TRUEFLAGS) || keyword.StartsWith(" " + TRUEFLAGS))
				{
					string[] flagsToSetTrue = value.Split(' ');
					SetFlagArrayTo(Flag.True, flagsToSetTrue, am);
				}
				else if (keyword.StartsWith(FALSEFLAGS) || keyword.StartsWith(" " + FALSEFLAGS))
				{
					string[] falseFlags = value.Split(' ');
					SetFlagArrayTo(Flag.False, falseFlags, am);
				}
				else if (keyword.StartsWith(DONTCARE) || keyword.StartsWith(" " + DONTCARE))
				{
					string[] dontCareFlags = value.Split(' ');
					SetFlagArrayTo(Flag.DontCare, dontCareFlags, am);
				}
				else if (keyword.StartsWith(SETEMOTE) || keyword.StartsWith(" " + SETEMOTE))
				{
					am.Emote = value;
				}
			}

			AssertChar(stream, ']');

			return am;
		}

		private static void SetFlagArrayTo(Flag f, String[] flagNames, AttributeManager am)
		{
			foreach (string name in flagNames)
			{
				am.SetFlags.Add(name, f);
			}
		}
		private static string GrabSubstringToParse(TomeStream stream)
		{
			StringBuilder sb = new StringBuilder();

			while(stream.PeekChar() != ']')
			{
				sb.Append(stream.NextChar());
			}

			return sb.ToString();
		}

		private static IExpression ParseRequiredFlags(string str)
		{
			return BrigitExpressionParser.Parse(str);
		}
	}
}
