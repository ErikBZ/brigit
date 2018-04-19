using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Structure;
using Brigit.Parser.Stream;
using Brigit.Structure.Exchange;
using Brigit.Parser.Wrapper;

namespace Brigit.Parser
{
	public partial class BrigitParser
	{
		public Node ParseDialog(TomeStream stream)
		{
			Node node = new Node();
			var data = new Dialog();

			// first things first, parse character name and then semicolon
			data.Character = ParseCharacterName(stream);

			// check for colon
			if(stream.PeekChar() != '{')
			{
                String msg = String.Format("Expected : or {, found {0}, at {1}", stream.PeekChar(), stream.Position);
				throw new Exception(msg);
			}
			else
			{
				stream.NextChar();
			}

			// parsing the text then attributes of the text
			// getting rid of the starting whitespace
			EatWhitespace(stream);

			ParsingState state = ParsingState.ExpectingMore;
			while(state == ParsingState.ExpectingMore)
			{
				var text = ParseSpeechText(stream, ref state);
				data.Text.Add(text);
			}

            if(stream.PeekChar() == '[')
            {
                data.Attributes = ParseAttributes(stream);
            }

			// everything has been completed for this section.
			// ParseSpeechText will eat the final *
			node.Data = data;

			return node;
		}

		/// <summary>
		/// Parse a character name. Allows for spaces, dashes, single quotes and dots
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public string ParseCharacterName(TomeStream stream)
		{
			StringBuilder sb = new StringBuilder();
			HashSet<char> charsToAllow = new HashSet<char>(new char[] {'.', '\'', '-', '(', ')', ' '});

			while (stream.PeekChar() != '{')
			{
				if (Char.IsLetterOrDigit(stream.PeekChar()) || charsToAllow.Contains(stream.PeekChar()))
				{
					sb.Append(stream.NextChar());
				}
				else
				{
					throw new Exceptions.InvalidCharacterNameException(String.Format("Character name cannot contain: '{0}'. Location: {1}", stream.PeekChar(), stream.Position));
				}
			}

			// for now just remove spaces at the end of the string
			if (!char.IsLetterOrDigit(sb[sb.Length - 1]))
			{
			}

			return sb.ToString();
		}

		private SpeechText ParseSpeechText(TomeStream stream, ref ParsingState state)
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
                String msg = String.Format("Expected * to end speech parsing but found {0} at {1}",
                                            stream.PeekChar(), stream.Position);
                throw new Exception(msg);
            }

            // parse the attribute
            if (stream.PeekChar() == '[')
            {
                // this closes the paren for us if there is any
                st.Attributes = ParseAttributes(stream);
            }

            EatWhitespace(stream);

            // check if it ended
            // at this point we know there is at least 1 star
            // if this is true then this will be the last speech text
            if (stream.PeekChar() == '}')
			{
				stream.NextChar();
				state = ParsingState.Complete;
			}

			st.Text = text;

			return st;
		}
	}
}