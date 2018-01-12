using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit;
using Brigit.Structure;
using Brigit.Structure.Exchange;

namespace Brigit.Parser.Wrapper
{
    // Wrapper for node and choice
    public class OpenChoice
    {
        public Node EnclosingNode;
        public Choice BranchingChoice;

        public OpenChoice(Node node, Choice ch)
        {
            EnclosingNode = node;
            BranchingChoice = ch;
        }
    }
}
