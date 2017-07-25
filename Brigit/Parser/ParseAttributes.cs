using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Attributes;
using Brigit.Parser.Stream;

namespace Brigit.Parser
{
	public partial class BrigitParser
	{
		// starts at ( and eats away the closing paren
		public static AttributeManager ParseAttributes(TomeStream stream)
		{
			AttributeManager am = new AttributeManager();

			// since we're in here we assume this is the [ character
			stream.NextChar();

			// i'll do this later
			// here we parse the actual attributes
			while (stream.PeekChar() != ']')
			{
				stream.NextChar();
			}

			// eating away the ending )
			stream.NextChar();

			return am;
		}
	}
}
