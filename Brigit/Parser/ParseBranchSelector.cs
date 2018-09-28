using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Structure;
using Brigit.Parser.Stream;
using Brigit.Structure.Exchange;
using Brigit.Attributes.ExpressionParser;
using Brigit.Attributes.Operators;
using Brigit.Parser.Wrapper;

namespace Brigit.Parser
{
	partial class BrigitParser
	{
        // this will select a branch from a set given to it
        // this looks really similar to chioce with some smaller but important changes
        public BrigitGraph ParseBranchSelector(Dictionary<string, OpenChoice> branchEndings)
        {
            AssertChar(Stream, '{');
			// the loop consists of parsing expression, parsing branch
			// then check for a new expression or the end

			// TODO replace this fix with a more elegant solution
			// hot fix used for resetting the open choices
			List<string> branches = new List<string>();
            Decision selector = new Decision();
            selector.Interactive = false;
            BrigitGraph graph = new BrigitGraph();
            Node root = new Node()
            {
                Data = selector
            };
            graph.AddNode(root);

            while (Stream.PeekChar() != '}')
            {
                // gets chars up to the *
                string expression = FetchNonStarSubString();
                IExpression exp;
                if (expression == "_")
                {
                    //exp = new Tautalogy();
					exp = new Variable("TRUE");
                }
                else
                {
                    exp = BrigitExpressionParser.Parse(expression);
                }
                // eat the star that ends the expresssion
                AssertChar(Stream, '*');
                Choice ch = new Choice();
                ch.Attributes.Expression = exp;
                ch.Text = string.Empty;
                ch.NextNode = -1;

                EatWhitespace(Stream);

                // if there is no arrow then it should be considered a
                // "pass"
                // there can be multiple passes, but there can only be one default which is "_"
                if(Stream.PeekChar() == '-')
                {
                    // parse either a sub graph or parse a branchname
                    ParseArrow(Stream);
                    EatWhitespace(Stream);

                    if(Stream.PeekChar() == '{')
                    {
                        AssertChar(Stream, '{');
                        BrigitGraph subGraph = ParseBrigitGraph(Stream);
                        graph.AddBranch(root, subGraph);
                        ch.NextNode = root.Next.Count - 1;
                    }
                    else if (Char.IsLetterOrDigit(Stream.PeekChar()))
                    {
                        string BranchName = ParseOnlyTextNoEscape(Stream);
						branches.Add(BranchName);
                        OpenChoice open = new OpenChoice(root, ch);
                        branchEndings.Add(BranchName, open);
                    }
                    else
                    {
                        // throw not recognized
                        string msg = String.Format("Expected beginning of branch name or sub graph but found {0} at {1}", Stream.PeekChar(), Stream.Position);
                        throw new Exception(msg);
                    }
                }
                // a pass
                else
                {
                }
                selector.Choices.Add(ch);
                EatWhitespace(Stream);
            }

            // to end this off
            AssertChar(Stream, '}');
            
            foreach(Choice ch in selector.Choices)
            {
                if(ch.NextNode == -1)
                {
                    ch.NextNode = root.Next.Count;
                }
            }

			foreach(string s in branches)
			{
				branchEndings[s].TailNode = null;
			}

			return graph;
		}
	}
}
