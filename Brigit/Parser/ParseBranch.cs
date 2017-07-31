using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Parser.Stream;

namespace Brigit.Parser
{
	public static partial class BrigitParser
	{
		public static (string, BrigitGraph) ParseBranch(TomeStream stream)
		{
			string name = string.Empty;
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

			return (name, branchGraph);
		}
	}
}
