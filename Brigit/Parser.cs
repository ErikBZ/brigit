using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit
{
    // static class
    public class Parser
    {
        public static Eater muncher;

        /// <summary>
        /// Creates a DOM structure for a Brigit Text File
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        // This is where most of the logic for creating the actuall
        // structure will be. This isn't an ordinary DOM since this struct
        // can have nodes with more than 1 parent
        public static DomTree ParseBrigitText(string data)
        {
            muncher = new Eater(data);
            if (muncher.SniffChar() != '[')
                throw new Exception("brigit txt must start with [load] tag");
            else
                muncher.ConsumeChar();
            DomTree tree = ParseLoadTag();
            muncher.EatWhiteSpace();
            tree.Head = ParseResTag();
            return tree;    
        }

        public static DomNode ParseTag()
        {
            // Eat the fluff in front until muncher sees a open bracket
            muncher.EatWhiteSpace();
            if (muncher.CheckChar('['))
            {
                muncher.ConsumeChar();
                // parsing a pair of tag that must be closed out
                /*
                else
                {
                // TODO add code here pls
                }
                */
            }
            else
            {
                throw new Exception("Malformed txt file, no free characters allowed");
            }
            return new DomNode();
        }

        /// <summary>
        /// Parses the load tag which must be set at the beginning of the file
        /// </summary>
        /// <returns>Returns a new DomTree that will be used as the base</returns>
        public static DomTree ParseLoadTag()
        {
            DomTree returnDom = new DomTree();
            // consume the tag
            muncher.ConsumeChar(4);

            Dictionary<string, string[]> argumentSet = ParseArgumentSetPairs();
            foreach(KeyValuePair<string, string[]> kvPiar in argumentSet)
            {
                switch(kvPiar.Key)
                {
                    case "char":
                        returnDom.SetCharacterDict(kvPiar.Value);
                        break;
                    case "background":
                        returnDom.SetBackgrounds(kvPiar.Value);
                        break;
                    default:
                        break;
                }
            }
            return returnDom;
        }

        public static DomNode ParseResTag()
        {
            Response node = new Response();
            if(muncher.SniffChar() == '[')
            {
                muncher.ConsumeChar();
            }
            // eating the tag, "res"
            muncher.ConsumeChar(3);
            // Parsing the tag [] and it's parameters
            Dictionary<string, string[]> arguments = ParseArgumentSetPairs();
            foreach(KeyValuePair<string, string[]> entry in arguments)
            {
                if(entry.Key == "char" || entry.Key == "background")
                {
                    if(entry.Value.Length > 1)
                    {
                        throw new Exception("char and background cannot have more than one set");
                    }
                }
                switch(entry.Key)
                {
                    case "char":
                        node.Character = new Character(entry.Value[0]);
                        break;
                    case "background":
                        node.Background = new Background(entry.Value[0]);
                        break;
                    default:
                        Console.WriteLine("RES does not have a solution for the {0} tag", entry.Key);
                        break;
                }
            }
            // throwing out all the carriages and spaces before the text
            // actually starts
            muncher.EatWhiteSpace();
            // parsing the actual text
            string text = ParseParagraphs();
            node.Text = text;
            // eating up the [*] ending tag
            muncher.ConsumeChar(3);
            return node;
        }

        /// <summary>
        /// Parses a section of text to be used for dialog, usually.
        /// </summary>
        /// <returns>
        ///     Returns a string where return carriages are replaced
        ///     with spaces
        /// </returns>
        public static string ParseParagraphs()
        {
            string str = string.Empty;
            str = muncher.SpitUpWhile(delegate (char c)
            {
                return c != '[';
            });
            str = Regex.Replace(str, @"\r\n?\n", " ");
            return str;
        }

        /// <summary>
        /// parses an entire tag of arguments for any tag
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string[]> ParseArgumentSetPairs()
        {
            Dictionary<string, string[]> arguments = new Dictionary<string, string[]>();
            while (muncher.SniffChar() != ']')
            {
                muncher.EatWhiteSpace();
                string argument = muncher.SpitUpAlpha();
                string[] set = null;

                if (muncher.CheckChar(':'))
                {
                    muncher.ConsumeChar();
                    set = ParseSetOfStrings();
                }

                arguments.Add(argument, set);
            }

            if (muncher.CheckChar(']'))
            {
                muncher.ConsumeChar();
            }
            else
            {
                throw new Exception("End of tag not found after arguments were parsed");
            }

            return arguments;
        }

        // what should the regex for this be?
        public static string[] ParseSetOfStrings()
        {
            Queue<string> que = new Queue<string>();
            while (!(muncher.SniffChar() == ';' || muncher.SniffChar() == ']'))
            {
                muncher.EatWhiteSpace();
                char ch = muncher.SniffChar();
                if (!Char.IsLetterOrDigit(ch) || ch == '_' || ch == '-' || ch == '.')
                    throw new Exception("Malformed set, sets can only consits of . - _ and letters and numbers");

                que.Enqueue(muncher.SpitUpWhile(delegate (char c)
                {
                    return Char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.';
                }));
            }
            // eating up the ';'
            if(muncher.SniffChar() == ';')
            {
                muncher.ConsumeChar();
            }
            return que.ToArray();
        }
    }

    /// <summary>
    /// A class that steps through a string to parse it
    /// </summary>
    public class Eater
    {
        string data;
        int pos;

        public Eater():this(string.Empty)
        {
        }

        public Eater(string data)
        {
            this.data = data;
            pos = 0;
        }

        public bool Complete()
        {
            return pos >= data.Length;
        }

        /// <summary>
        /// Checks to see if a the char at pos is equal to the given char
        /// </summary>
        /// <param name="c">A Char</param>
        /// <returns>True or False</returns>
        public bool CheckChar(char c)
        {
            return data[pos] == c;
        }

        /// <summary>
        /// Peeks at the current char
        /// </summary>
        /// <returns>Returns the current char</returns>
        public char SniffChar()
        {
            // if pos is greater than the string length return the null terminator
            if (pos >= data.Length)
                return '\0';
            return data[pos];
        }

        /// <summary>
        /// Steps forward by 1 in the string
        /// </summary>
        public void ConsumeChar()
        {
            pos++;
        }

        /// <summary>
        /// Steps forware by x in the string
        /// </summary>
        /// <param name="x">Skip x chars</param>
        public void ConsumeChar(int x)
        {
            pos += x;
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
        /// Func is pretty cool
        /// </summary>
        /// <param name="predicate"></param>
        public void EatWhile(Func<Char, bool> predicate)
        {
            while(predicate(SniffChar()))
            {
                ConsumeChar();
            }
        }

        /// <summary>
        /// I hope I'm using this right
        /// </summary>
        public void EatWhiteSpace()
        {
            Func<char, bool> p = delegate (char c) { return Char.IsWhiteSpace(c); };
            EatWhile(p);
        }

        /// <summary>
        /// Consumes a string of characters then spits them back up
        /// </summary>
        /// <param name="pred">A predicate that will test a char</param>
        /// <returns>Returns a string fufilling the predicate</returns>
        public string SpitUpWhile(Func<char, bool> pred)
        {
            StringBuilder sb = new StringBuilder();
            while(pred(SniffChar()) && pos < data.Length)
            {
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
            bool b = true;
            for(int i= pos;i<s.Length;i++)
            {
                if (data[pos + i] != s[i])
                    b = false;
            }
            return b;
        }
    }
}
