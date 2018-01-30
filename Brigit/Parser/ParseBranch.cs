using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Parser.Stream;

namespace Brigit.Parser
{
	public partial class BrigitParser
	{
		public BrigitGraph ParseBranch(TomeStream stream, ref string name)
		{
			BrigitGraph branchGraph = new BrigitGraph();

			// parse away the > and the name
			AssertChar(stream, '>');
			// getting the name
			name = ParseOnlyTextNoEscape(stream);

			// whitespace can be between the name and the opener
			Whitespace(stream);
			AssertChar(stream, '{');

			// parse like an ordinary tome after ward
			branchGraph = ParseBrigitGraph(stream);

			return branchGraph;
		}
	}
}
