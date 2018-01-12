using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Structure;
using Brigit.Parser.Stream;
using Brigit.Structure.Exchange;

namespace Brigit.Parser
{
	partial class BrigitParser
	{
		// this will select a branch from a set given to it
		public static BranchSelector ParseBranchSelector(TomeStream stream)
		{
			return new BranchSelector();
		}
	}
}
