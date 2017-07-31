using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Parser.Stream;
using Brigit.Structure.Exchange;
using Brigit.Structure;

namespace Brigit.Parser
{
	public partial class BrigitParser
	{
		public static (BrigitGraph, Dictionary<string, (Node, Choice)>) ParseDescision(TomeStream stream)
		{
			Descision descision = new Descision();
			BrigitGraph ll = new BrigitGraph();
			Node root = new Node()
			{
				Data = descision
			};

			ll.Add(root);

			// first parse away the @player:
			AssertChar(stream, '@');
			AssertAlphaDigitString(stream, "player");
			AssertChar(stream, ':');
			Whitespace(stream);

			// this is tracks the branchnames and where the
			// the branches should be placed
			Dictionary<string, (Node, Choice)> branchNameToNodeAndNode = new Dictionary<string, (Node, Choice)>();

			ParsingState state = ParsingState.ExpectingMore;
			while(state == ParsingState.ExpectingMore)
			{
				// parse the choice (same as parse text with esacape)
				// parse attributes if there are any
				Choice ch;

				(ch, state) = ParseChoice(stream);
				// -1 is the place holder for now. will be set to an actual number
				// or to the size of list
				ch.NextNode = -1;
				descision.Choices.Add(ch);

				// at this point either the parsing is complete
				// or there is an arrow pointing to a sub branch or a branch name

				// Parseing.Expecting more implies that the arrow maybe still be here
				if (ParseArrow(stream))
				{
					// whitespace
					Whitespace(stream);
					// either it's a branch 
					if (stream.PeekChar() == '{')
					{
						AssertChar(stream, '{');
						// parse the subbranch if there is one
						// parse the branch name is there is one, and we didn't parse a sub branch
						// add the subbranch to the next list if there is none set their "next" to -1
						BrigitGraph subGraph = ParseBrigitGraph(stream);

						ll.AddBranch(root, subGraph);
						ch.NextNode = root.Next.Count - 1;	
					}
					// or a branch name
					else
					{
						string BranchName = ParseOnlyTextNoEscape(stream);
						branchNameToNodeAndNode.Add(BranchName, (root, ch));
					}

					// this means it has reach the end
					if(stream.PeekChar() == '*')
					{
						state = ParsingState.Complete;
						stream.NextChar();
					}
				}
			}

			// any next nodes in descision that are -1 should be set
			// to the count of the list
			foreach(Choice c in descision.Choices)
			{
				if(c.NextNode == -1)
				{
					c.NextNode = root.Next.Count;		
				}
			}

			return (ll, branchNameToNodeAndNode);
		}

		private static bool ParseArrow(TomeStream stream)
		{
			if(stream.PeekChar() == '-')
			{
				stream.NextChar();
				return AssertChar(stream, '>');
			}
			return false;
		}

		// this uses the same method from the parse speech
		private static (Choice, ParsingState) ParseChoice(TomeStream stream)
		{
			Choice choice = new Choice();
			// the biggest difference these two have is the pointer to the index
			(SpeechText st, ParsingState state) = ParseSpeechText(stream);

			choice.Text = st.Text;
			choice.Attributes = st.Attributes;

			return (choice, state);
		}
	}
}
