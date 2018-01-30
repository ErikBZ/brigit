using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Parser.Stream;
using Brigit.Structure.Exchange;
using Brigit.Structure;
using Brigit.Parser.Wrapper;

namespace Brigit.Parser
{
	public partial class BrigitParser
	{
		public static BrigitGraph ParseDescision(TomeStream stream, Dictionary<string, OpenChoice> branchEndings)
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

			ParsingState state = ParsingState.ExpectingMore;
			while(state == ParsingState.ExpectingMore)
			{
				// parse the choice (same as parse text with esacape)
				// parse attributes if there are any
				Choice ch = ParseChoice(stream, ref state);
				// -1 is the place holder for now. will be set to an actual number
				// or to the size of list
				ch.NextNode = -1;
				descision.Choices.Add(ch);

				// at this point either the parsing is complete
				// or there is an arrow pointing to a sub branch or a branch name

				// Parseing.Expecting more implies that the arrow maybe still be here
                // Some branches have multiple "nexts" where the next either points to the same node
                // or to two different ones within the same branching pattern.
                //  It's the addition of the nodes that's wrong 
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
                        OpenChoice openCh = new OpenChoice(root, ch);
						branchEndings.Add(BranchName, openCh);
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

            return ll;
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
		private static Choice ParseChoice(TomeStream stream, ref ParsingState state)
		{
			Choice choice = new Choice();
			// the biggest difference these two have is the pointer to the index
			SpeechText st = ParseSpeechText(stream, ref state);

			choice.Text = st.Text;
			choice.Attributes = st.Attributes;

			return choice;
		}
	}
}
