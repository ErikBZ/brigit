using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO add the Eater class to this and
// Rename BrigitParser to BrigitParser
namespace Brigit.Parser
{

    /// <summary>
    /// Parses a tome document into a DOM data structure
    /// </summary>
    public class TomeParser
    {
        // MUNCHER LIVES ON
        TomeReader muncher;
        // leaving this public for testing purposes
        public List<String> characters;

        /// <summary>
        /// Don't use this
        /// </summary>
        public TomeParser()
        {
            muncher = null;
            characters = null;
        }

        /// <summary>
        /// Creates a new parser instance.
        /// </summary>
        /// <param name="textToParse"></param>
        public TomeParser(string[] textToParse)
        {
            muncher = new TomeReader(textToParse);
            characters = new List<string>();
        }

        /// <summary>
        /// Parses the text of what a character says
        /// </summary>
        /// <returns></returns>
        private string ParseSpeechText()
        {
            string entry = muncher.SpitUpWhile(delegate (char c)
            {
                if (muncher.StartsWith("\\*") || muncher.StartsWith("\\}"))
                {
                    muncher.ConsumeChar();
                    return true;
                }
                else
                {
                    return c != '*' && c != '}';
                }
            });
            return entry;
        }

        /// <summary>
        /// Parses the speech of character and the attributes if there are any
        /// </summary>
        /// <returns></returns>
        private Dialog ParseSingleDialog()
        {
            muncher.EatWhiteSpace();
            Dialog node = new Dialog();
            string speech = ParseSpeechText();
            node.Text = speech;
            node.Type = NodeType.Object;
            return node;
        }

        /// <summary>
        /// Parses a characater name
        /// </summary>
        /// <returns></returns>
        private string ParseCharacterName()
        {
            string characterName = muncher.SpitUpWhile(delegate (char c)
            {
                if(Char.IsLetterOrDigit(muncher.SniffChar()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
            return characterName;
        }


        /// <summary>
        /// Parses and entire dialog set that a character will say
        /// It is ended by a '}' closing curly brace
        /// </summary>
        /// <returns>A DomTree containing the entry nodes for what the character said</returns>
        // This is looking a lot like a function
        public DomTree ParseCharacterDialog()
        {
            DomTree tree = new DomTree();

            muncher.EatWhiteSpace();
            string character = ParseCharacterName();

            if (!characters.Contains(character))
            {
                // print exception and exit
                Console.WriteLine($"{character} is not character in the scene. Error found at {muncher.Position}");
            }

            // parse attributes like expression?
            // i'll save this for later

            muncher.EatWhiteSpace();
            char openBracket = muncher.SpitChar();
            if(openBracket != '{')
            {
                Console.WriteLine($"No open bracket for beginning of dialog by character. Error found at {muncher.Position}");
                // print exception and exit
            }

            // parsing the actual text
            while (muncher.SniffChar() != '}')
            {
                DomNode newNode = ParseSingleDialog();
                newNode.Character = character;
                tree.Add(newNode);
                char asterisk = muncher.SpitChar();
                if(asterisk == '*')
                {
                    muncher.ConsumeChar();
                }
            }

            // eating the '}'
            muncher.ConsumeChar();

            return tree;
        }
    }
    
    /// <summary>
    /// A class that steps through a string to parse it
    /// </summary>
    class TomeReader
    {
        string[] all_text;
        // The actual line and position of the character
        int lineNum;
        int posNum;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in all_text)
            {
                sb.Append(s);
            }
            return sb.ToString();
        }

        public string Position
        {
            // the plus 1 is to normalize it
            get { return $"Line: {lineNum + 1}, Position: {posNum + 1}"; }
        }

        public TomeReader() : this(null)
        {
        }

        public TomeReader(string[] text)
        {
            all_text = text;
            lineNum = 0;
            posNum = 0;
        }

        /// <summary>
        /// Checks if the eater has reached the EOF
        /// </summary>
        /// <returns></returns>
        public bool Complete()
        {
            return lineNum == all_text.Length;
        }

        /// <summary>
        /// Gets the remaining string to be parsed
        /// </summary>
        /// <returns></returns>
        public string GetRemainingString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = lineNum; i < all_text.Length; i++)
            {
                if (i == lineNum)
                {
                    sb.Append(all_text[lineNum].Substring(posNum));
                }
                else
                {
                    sb.Append(all_text[i]);
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Checks to see if a the char at pos is equal to the given char
        /// </summary>
        /// <param name="c">A Char</param>
        /// <returns>True or False</returns>
        public bool CheckChar(char c)
        {
            bool b = false;
            if (!Complete())
            {
                b = c == SniffChar();
            }
            return b;
        }

        /// <summary>
        /// Peeks at the current char
        /// </summary>
        /// <returns>Returns the current char</returns>
        public char SniffChar()
        {
            // if pos is greater than the string length return the null terminator
            if (Complete())
                return '\0';
            if (all_text[lineNum] == string.Empty)
                return ' ';
            if (all_text[lineNum].Length == posNum)
                return '\n';
            return all_text[lineNum][posNum];
        }

        /// <summary>
        /// Steps forward by 1 in the string
        /// </summary>
        public void ConsumeChar()
        {
            // The eater has already reached the end
            if (lineNum >= all_text.Length)
            {
                Console.WriteLine("Reached end of file cannot eat anymore");
            }
            else if (all_text[lineNum].Length == posNum || all_text[lineNum] == string.Empty)
            {
                posNum = 0;
                lineNum++;
            }
            else
            {
                posNum++;
            }
        }

        /// <summary>
        /// Steps forward by x in the string
        /// </summary>
        /// <param name="x">Skip x chars</param>
        // Slower but safer
        public void ConsumeChar(int x)
        {
            for (int i = 0; i < x && !Complete(); i++)
            {
                ConsumeChar();
            }
            if (Complete())
            {
                Console.WriteLine("Reached EOF cannot eat anymore");
            }
        }

        /// <summary>
        /// Returns the current char and steps forward by one
        /// </summary>
        /// <returns>Returns the current char</returns>
        public char SpitChar()
        {
            char c = SniffChar();
            ConsumeChar();
            return c;
        }

        /// <summary>
        /// Takes a delegate that returns a bool to keep eating
        /// until the function returns false.
        /// </summary>
        /// <param name="predicate"></param>
        public void EatWhile(Func<char, bool> predicate)
        {
            while (predicate(SniffChar()) && !Complete())
            {
                ConsumeChar();
            }
        }

        /// <summary>
        /// Only eats whitespace like tabs, spaces and carriage
        /// returns
        /// </summary>
        public void EatWhiteSpace()
        {
            Func<char, bool> p = delegate (char c) { return Char.IsWhiteSpace(c); };
            EatWhile(p);
        }

        /// <summary>
        /// Consumes a string and spits it back out. Only takes
        /// a conditional statement and cannot add extra things
        /// to the string builder
        /// </summary>
        /// <param name="pred"></param>
        /// <returns></returns>
        public string SpitUpWhile(Func<char, bool> pred)
        {
            StringBuilder sb = new StringBuilder();
            while (pred(SniffChar()) && !Complete())
            {
                sb.Append(SpitChar());
            }

            return sb.ToString();
        }

        // overloading SpitUpWhile
        public string SpitUpWhile(Func<char, StringBuilder, bool> pred)
        {
            StringBuilder sb = new StringBuilder();
            while(!Complete() && pred(SniffChar(), sb))
            {
                // all of the required things will be pred?
            }
            return sb.ToString();
        }

        /// <summary>
        /// Eats only letters and then spits them back up
        /// </summary>
        /// <returns>A string of letters</returns>
        public string SpitUpAlpha()
        {
            Func<char, bool> p = delegate (char c) { return Char.IsLetter(c); };
            return SpitUpWhile(p);
        }

        /// <summary>
        /// Checks to see if the string, starting at pos, has the string "s"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool StartsWith(string s)
        {
            string sub = string.Empty;
            bool b = false;
            if (!Complete())
            {
                sub = all_text[lineNum].Substring(posNum);
                b = sub.StartsWith(s);
            }
            return b;
        }
    }
}
