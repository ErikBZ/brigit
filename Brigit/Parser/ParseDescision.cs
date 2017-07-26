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
		public static LinkedList ParseDescision(TomeStream stream)
		{
			Descision descision = new Descision();
			LinkedList ll = new LinkedList();
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
				Choice ch;
				(ch, state) = ParseChoice(stream);
				// -1 is the place holder for now. will be set to an actual number
				// or to the size of list
				ch.NextNode = -1;
				descision.Choices.Add(ch);

				// at this point either the parsing is complete
				// or there is an arrow pointing to a sub branch or a branch name

				// Parseing.Expecting more implies that the arrow maybe still be here
				if(ParseArrow(stream))
				{
					AssertChar(stream, '{');
					// parse the subbranch if there is one
					// parse the branch name is there is one, and we didn't parse a sub branch
					// add the subbranch to the next list if there is none set their "next" to -1
					LinkedList subGraph = ParseTome(stream);
					//Whitespace(stream);
					//AssertChar(stream, '}');

					ll.AddBranch(root, subGraph);
					ch.NextNode = root.Next.Count - 1;	
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
