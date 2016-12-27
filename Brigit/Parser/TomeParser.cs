using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        // scene tracker
        DomTree scene;

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
            scene = new DomTree();
        }

        /// <summary>
        /// Parses an entire tome file into a DomTree for writing or 
        /// </summary>
        /// <returns>DomTree parsed from a tome file</returns>
        public DomTree Parse()
        {
            scene.Add(ParseDomTree(false));
            return scene;
        }

        public DomTree ParseDomTree(bool sceneIsBranch)
        {
            DomTree scene = new DomTree();
            // add stuff for parses things before the actually starts here
            // idk what but stuff I guess

            muncher.EatWhiteSpace();
            if(sceneIsBranch)
            {
                muncher.ConsumeChar('{');
                muncher.EatWhiteSpace();
            }


            // this while loop keeps going if there is still more to parse
            // OR if this Parse is for a branch then it stops when the end of the branch is reached
            // noted by a '}'
            while (!muncher.Complete() && !(sceneIsBranch && muncher.CheckChar('}')))
            {
                if (muncher.StartsWith("CHOICE"))
                {
                    scene.Add(ParseChoice());
                }
                else
                {
                    scene.Add(ParseCharacterDialog());
                }
                muncher.EatWhiteSpace();
            }
            if (sceneIsBranch)
            {
                muncher.ConsumeChar('}');
            }

            return scene;
        }

        /// <summary>
        /// Parses the text of what a character says
        /// </summary>
        /// <returns></returns>
        private string ParseSpeechText()
        {
            string entry = muncher.SpitUpWhile(delegate (char c)
            {
                // If there is more than one space or tab that is not delimted
                // in the string then eat all but one of the spaces
                if(muncher.StartsWith("  "))
                {
                    muncher.EatWhiteSpace();
                    c = muncher.SniffChar();
                }

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
        /// Parses the possible choices that a player has for a certain part
        /// </summary>
        /// <returns></returns>
        private DomTree ParseChoice()
        {
            // this will be used later when trees can be created within a CHOICE block
            DomTree tree = new DomTree();
            // for now we only really care about making sure Chioce works
            Choice node = new Choice();

            // parsing the header and the beginning
            muncher.ConsumeString("CHOICE");
            muncher.EatWhiteSpace();
            // attribute parsing here? yeah probably but i'll save that for later
            // TODO add the parsing of the attributes here
            muncher.ConsumeChar('{');
            muncher.EatWhiteSpace();


            List<string> choices = new List<string>();
            List<DomTree> branches = new List<DomTree>();
            int numberOfChoices = 0;
            while (!muncher.CheckChar('}'))
            {
                bool useDefaultLocalFlags = true;
                choices.Add(ParseSpeechText());
                if (muncher.SpitChar() != '*')
                {
                    throw new Exception($"Speech text at {muncher.Position} did not end in an *");
                }
                // attibute and flag requirements here.
                // if there are none then the default will be used

                if(muncher.StartsWith("->"))
                {
                    muncher.ConsumeString("->");
                    if (muncher.CheckChar('{'))
                    {
                        DomTree branch = ParseDomTree(true);
                        if (useDefaultLocalFlags)
                        {
                            if (!scene.LocalFlags.ContainsKey($"choice{numberOfChoices}"))
                            {
                                scene.LocalFlags.Add($"choice{numberOfChoices}", false);
                            }
                            // what is required for the head of the branch node
                            branch.Head.RequiredFlags = $"choice{numberOfChoices}";

                            // the flags raised by this node
                            node.FlagsRasiedByChoices.Add(numberOfChoices, new Dictionary<string, bool>());
                            node.FlagsRasiedByChoices[numberOfChoices].Add($"choice{numberOfChoices}", true);
                        }
                    }
                    else
                    {
                        throw new Exception($"New branch must start with open '{{'. {muncher.Position}");
                    }
                }
                else
                {
                    node.FlagsRasiedByChoices.Add(numberOfChoices, new Dictionary<string, bool>());
                }
                numberOfChoices++;
                muncher.EatWhiteSpace();
            }
            // eating the last closing bracket
            muncher.ConsumeChar();
            node.Choices = choices.ToArray();
            tree.Add(node);
            return tree;
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
            if(!characters.Contains(characterName))
            {
                characters.Add(characterName);
            }
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

            muncher.ConsumeChar('{');

            // parsing the actual text
            while (muncher.SniffChar() != '}')
            {
                DomNode newNode = ParseSingleDialog();
                newNode.Character = character;
                tree.Add(newNode);
                char asterisk = muncher.SniffChar();
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
            return lineNum == all_text.Length ||
                (lineNum == all_text.Length-1 && posNum == all_text[lineNum].Length);
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
            {
                return '\0';
            }
            if (all_text[lineNum] == string.Empty)
            {
                return ' ';
            }
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
                throw new Exception($"{Position}, reached the end of the file.");
            }
            else if (all_text[lineNum].Length-1 == posNum || all_text[lineNum] == string.Empty)
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
        /// Checks to see if a specfic char is at the location and consusmes it
        /// if not then it throws an exception
        /// </summary>
        /// <param name="x"></param>
        public bool ConsumeChar(char x)
        {
            if(CheckChar(x))
            {
                ConsumeChar();
                return true;
            }
            else
            {
                throw new Exception($"Expected {x} at position {Position}");
            }
        }

        /// <summary>
        /// Checks if the reader will read in the given string
        /// and then consumes it. Throws an expception otherwise
        /// </summary>
        /// <param name="str"></param>
        public bool ConsumeString(string str)
        {
            if (this.StartsWith(str))
            {
                this.ConsumeChar(str.Length);
                return true;
            }
            else
            {
                throw new Exception($"Expected keyword {str}, but it was not found. {this.posNum}");
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
                sb.Append(SpitChar());
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
