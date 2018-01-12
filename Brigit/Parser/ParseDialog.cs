using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Structure;
using Brigit.Parser.Stream;
using Brigit.Structure.Exchange;

namespace Brigit.Parser
{
	public static partial class BrigitParser
	{
		public static Node ParseDialog(TomeStream stream)
		{
			Node node = new Node();
			var data = new Dialog();

			// first things first, parse character name and then semicolon
			data.Character = ParseOnlyTextNoEscape(stream);

			// check for colon
			if(stream.PeekChar() != ':')
			{
				throw new Exception($"Expected :, found { stream.PeekChar() }, at { stream.Position }");
			}
			else
			{
				stream.NextChar();
			}

			// parsing the text then attributes of the text
			// getting rid of the starting whitespace
			Whitespace(stream);

			ParsingState state = ParsingState.ExpectingMore;
			while(state == ParsingState.ExpectingMore)
			{
				var text = ParseSpeechText(stream, ref state);
				data.Text.Add(text);
			}

			// everything has been completed for this section.
			// ParseSpeechText will eat the final *
			node.Data = data;
			
			return node;
		}

		private static SpeechText ParseSpeechText(TomeStream stream, ref ParsingState state)
		{
			SpeechText st = new SpeechText();
			state = ParsingState.ExpectingMore;

			// parse text here
			string text = ParseAndCleanTextWithEscape(stream);

			// make sure the text ends in a star
			if (stream.PeekChar() == '*')
			{
				stream.NextChar();
			}
			else
			{
				throw new Exception($"Expected * to end speech parsing but found {stream.PeekChar()} at {stream.Position}");
			}

			// parse the attribute
			if (stream.PeekChar() == '[')
			{
				// this closes the paren for us if there is any
				st.Attributes = ParseAttributes(stream);
			}

			// check if it ended
			// at this point we know there is at least 1 star
			// if this is true then this will be the last speech text
			if(stream.PeekChar() == '*')
			{
				stream.NextChar();
				state = ParsingState.Complete;
			}

			st.Text = text;

			return st;
		}
	}
}