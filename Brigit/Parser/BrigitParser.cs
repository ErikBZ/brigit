using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Parser.Stream;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Attributes;

namespace Brigit.Parser
{
	// partial class this file keeps all the generic used by all methods
	// this can't be a static class i think
	static public partial class BrigitParser
	{
		static Dictionary<string, (Node, Choice)> BranchesToPlace = new Dictionary<string, (Node, Choice)>();
		
		public static Conversation Parse(string[] tome)
		{
			string[] processedTome = ComomentRemover.RemoveComments(tome);
			// TODO stuff here
			return new Conversation();
		}

		public static void Whitespace(TomeStream stream)
		{
			while (Char.IsWhiteSpace(stream.PeekChar()))
			{
				stream.NextChar();
			}
		}

		// public for testing purpose
		// this chooses what is parsed next, IE a branch, a dialog or descision
		public static BrigitGraph ParseBrigitGraph(TomeStream stream)
		{
			BrigitGraph ll = new BrigitGraph();

			while (!(stream.Complete() || stream.PeekChar() == '}'))
			{
				// getting rid of some beginning whitespace
				Whitespace(stream);

				// the real parsing starts here
				// can't use switch need to use if elses
				char c = stream.PeekChar();
				if (Char.IsLetterOrDigit(c))
				{
					// start parsing as a dialog
					// this one is simple. parse the dialog. then add
					// it to the list
					Node n = ParseDialog(stream);
					ll.Add(n);
				}
				else if (c == '@')
				{
					// start parsing as a descision
					BrigitGraph subGraph;
					Dictionary<string, (Node, Choice)> branchesToNode;
					(subGraph, branchesToNode) = ParseDescision(stream);

					foreach(KeyValuePair<string, (Node,Choice)> kvp in branchesToNode)
					{
						BranchesToPlace.Add(kvp.Key, kvp.Value);
					}

					// adding the dictionary entries to this
					ll.Add(subGraph);
				}
				else if (c == '^')
				{
					// this is a branch selector
				}
				else if(c == '>')
				{
					// this is a branch name
					(string branchName, BrigitGraph subGraph) = ParseBranch(stream);
					if(BranchesToPlace.ContainsKey(branchName))
					{
						(Node n, Choice ch) = BranchesToPlace[branchName];
						ll.AddInBetween(n, subGraph);
						Descision d = n.Data as Descision;
						ch.NextNode = d.Choices.Count - 1;
					}
					else
					{
						throw new Exception($"{branchName} not found in dictionary, could the name be mispelled");
					}
				}
				else
				{
					// panic here
					throw new Exception($"Expected beginning of character name, branch or choice found {stream.PeekChar()} at {stream.Position}");
				}

				Whitespace(stream);
			}

			if(!stream.Complete())
			{
				AssertChar(stream, '}');
			}

			return ll; 
		}

		// only parses a-zA-Z and returns it as a string
		/// <summary>
		/// Parses a-zA-Z0-9 but nothing else
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static string ParseOnlyTextNoEscape(TomeStream stream)
		{
			StringBuilder sb = new StringBuilder();
			
			while(Char.IsLetterOrDigit(stream.PeekChar()))
			{
				sb.Append(stream.NextChar());
			}

			return sb.ToString();
		}

		/// <summary>
		/// Parses text that can have escape characters and cleans up the string
		/// by removing double \t, \n and spaces
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static string ParseAndCleanTextWithEscape(TomeStream stream)
		{
			StringBuilder sb = new StringBuilder();

			// this will get all the text up to a star which is the delimiter
			while(stream.PeekChar() != '*')
			{
				// just in case i'll keep this here but i may remove it later
				// i actually need this to delimit the star
				if (stream.PeekChar() == '\\')
				{
					sb.Append(stream.NextChar());
					// add whatever is next regardless of what it is
					sb.Append(stream.NextChar());
				}
				else
				{
					sb.Append(stream.NextChar());	
				}
			}

			// this is slower but also safer
			string clean = CleanString(sb.ToString());
			string decoded = DecodeString(clean);

			return decoded;
		}

		/// <summary>
		/// iterates over a string and replaces multiple whitespaces instances with a single
		/// space unless it is at the beginning or the end where it adds nothing
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string CleanString(string str)
		{
			StringBuilder sb = new StringBuilder();
			int i = 0;
			str = str.Trim();

			// tracks if a char has been encountered yet
			bool charEncountered = false;
			for(int j=i;j<str.Length;j++)
			{
				char c = str[j];
				if(Char.IsWhiteSpace(c) && charEncountered)
				{
					sb.Append(' ');
					// stops multiple whitespace characaters from being addded
					charEncountered = false;
				}
				else if(!Char.IsWhiteSpace(c))
				{
					sb.Append(c);
					charEncountered = true;
				}
			}
			return sb.ToString();
		}

		// decodes \t to tabs \n to new lines and \\ to \
		// i could also add some more

		// it doesn't look like i need this?
		public static string DecodeString(string str)
		{
			StringBuilder sb = new StringBuilder();

			int i = 0;
			char[] chars = str.ToCharArray();
			while (i < chars.Length)
			{
				char c = chars[i];
				if (c == '\\')
				{
					char decodedChar = DecodeChar(chars[i + 1]);
					sb.Append(decodedChar);
					i++;
				}
				else
				{
					sb.Append(c);
				}
				i++;
			}

			return sb.ToString();
		}

		private static char DecodeChar(char c)
		{
			switch (c)
			{
				case 't':
					return '\t';
				case 'n':
					return '\n';
				case '\\':
					return '\\';
				case '*':
					return '*';
				default:
					throw new Exception($"Error decoding char {c}. No such \\{c} character found");
			}
		}

		private static bool AssertChar(TomeStream stream, char c)
		{
			if(stream.PeekChar() != c)
			{
				throw new Exception($"Expected {c} symbol but found { stream.PeekChar() }, at position {stream.Position}");
			}
			stream.NextChar();
			return true;
		}

		private static bool AssertAlphaDigitString(TomeStream stream, string str)
		{
			string name = ParseOnlyTextNoEscape(stream);
			if(name != str)
			{
				throw new Exception($"Expected {str} symbol but found { name }, at position {stream.Position}");
			}
			return true;
		}
	}
}
