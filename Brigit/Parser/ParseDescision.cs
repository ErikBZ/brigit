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

			// first parse away the @player:

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

				// parse the subbranch if there is one
				// parse the branch name is there is one, and we didn't parse a sub branch
				// add the subbranch to the next list if there is none set their "next" to -1

				// if there are any -1 set it to the count of the list
				// the next node will be last in the list whose index equals the current count
			}

			return ll;
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
