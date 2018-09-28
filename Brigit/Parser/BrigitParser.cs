using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Parser.Stream;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Parser.Wrapper;
using Brigit.Attributes;

namespace Brigit.Parser
{
	// partial class this file keeps all the generic used by all methods
	// this can't be a static class i think
	public partial class BrigitParser
	{
		Dictionary<string, OpenChoice> BranchesToPlace = new Dictionary<string, OpenChoice>();
        TomeStream Stream;

        // Constructor
        // this class needs a stream so there will be no default constructor
        public BrigitParser(TomeStream stream)
        {
            Stream = stream;
        }
		
        public BrigitGraph Parse()
        {
            return ParseBrigitGraph(Stream);
        }

		public static BrigitGraph Parse(TomeStream stream)
		{
            BrigitParser parser = new BrigitParser(stream);
            var bg = parser.ParseBrigitGraph(stream);
            // TODO stuff here
            return bg;
		}
        
		public static void EatWhitespace(TomeStream stream)
		{
			while (Char.IsWhiteSpace(stream.PeekChar()))
			{
				stream.NextChar();
			}
		}

		// public for testing purpose
		// this chooses what is parsed next, IE a branch, a dialog or descision
		public BrigitGraph ParseBrigitGraph(TomeStream stream)
		{
			BrigitGraph ll = new BrigitGraph();

			while (!(stream.Complete() || stream.PeekChar() == '}'))
			{
				// getting rid of some beginning whitespace
				EatWhitespace(stream);

				// the real parsing starts here
				// can't use switch need to use if elses
				char c = stream.PeekChar();
				if (Char.IsLetterOrDigit(c))
				{
					// start parsing as a dialog
					// this one is simple. parse the dialog. then add
					// it to the list
					Node n = ParseDialog(stream);

                    // for the new AddInBetween function
                    // this will only work for whatever comes next. This isn't very good
                    // if there's multiple branches to place
                    // TODO make this functionality work better with dummy tail in the subgraphs
                    foreach(KeyValuePair<string, OpenChoice> kvp in BranchesToPlace)
                    {
                        if(kvp.Value.TailNode == null)
                        {
                            kvp.Value.TailNode = n;
                            break;
                        }
                    }

					ll.AddNode(n);
				}
				else if (c == '@')
				{
                    // naybe use a struct here?
                    Dictionary<string, OpenChoice> branchesToNode = new Dictionary<string, OpenChoice>();
                    // TODO change signature of ParseDescision to (obj stream, Dict brachesToNode)
					BrigitGraph subGraph = ParseDescision(stream, branchesToNode);

					foreach(KeyValuePair<string, OpenChoice> kvp in branchesToNode)
					{
						BranchesToPlace.Add(kvp.Key, kvp.Value);
					}

					// adding the dictionary entries to this
					ll.AddGraph(subGraph);
				}
				else if (c == '{')
				{
                    // this is a branch selector
                    // we can just pass in the big dictionary
                    BrigitGraph subGraph = ParseBranchSelector(BranchesToPlace);
                    ll.AddGraph(subGraph);
				}
				else if(c == '>')
				{
                    // this is a branch name
                    // TODO change signature of ParseBranch to ParseBranch(stream, ref string branchName) 
                    // TODO i'll probably need a wrapper for the Node and Ch entires
                    string branchName = String.Empty;
					BrigitGraph subGraph = ParseBranch(stream, ref branchName);
					if(BranchesToPlace.ContainsKey(branchName))
					{
						OpenChoice openCh = BranchesToPlace[branchName];
                        Node n = openCh.EnclosingNode;
                        Choice ch = openCh.BranchingChoice;

						ll.AddInBetween(n, new List<Node>() {openCh.TailNode}, subGraph);
						ch.NextNode = n.Next.Count - 1;
					}
					else
					{
                        String msg = String.Format("{0} not found in dictionary, could the name be mispelled?",
                                                    branchName);
						throw new Exception(msg);
					}
				}
				else
				{
                    // panic here
                    String msg = String.Format("Expected beginning of character name, branch or chioce but found {0} at {1}",
                                                stream.PeekChar(), stream.Position);
					throw new Exception(msg);
				}

				EatWhitespace(stream);
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
		public string ParseOnlyTextNoEscape(TomeStream stream)
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
		public string ParseAndCleanTextWithEscape(TomeStream stream)
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
		public string CleanString(string str)
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
                    String msg = String.Format("Error decoding cha {0}. No such \\{0} character found",
                                                c);
					throw new Exception(msg);
			}
		}

		private bool AssertChar(TomeStream stream, char c)
		{
			if(stream.PeekChar() != c)
			{
                String msg = String.Format("Expected {0} symbol but found {1}, at position {2}",
                                            c, stream.PeekChar(), stream.Position);
				throw new Exception(msg);
			}
			stream.NextChar();
			return true;
		}

		private bool AssertAlphaDigitString(TomeStream stream, string str)
		{
			string name = ParseOnlyTextNoEscape(stream);
			if(name != str)
			{
                String msg = String.Format("Expected {0} symbol but found {1}, at position {2}",
                                            str, name, stream.Position);
				throw new Exception(msg);
			}
			return true;
		}

        private string FetchNonStarSubString()
        {
            StringBuilder sb = new StringBuilder(); 
            while(Stream.PeekChar() != '*')
            {
                sb.Append(Stream.NextChar());
            }
            return sb.ToString();
        }
	}
}
