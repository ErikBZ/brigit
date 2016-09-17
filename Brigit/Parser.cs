using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System.Diagnostics;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit
{
    // static class
    public class Parser
    {
        public Eater muncher;
        
        /// <summary>
        /// Creates a DOM structure for a Brigit Text File
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        // This is where most of the logic for creating the actuall
        // structure will be. This isn't an ordinary DOM since this struct
        // can have nodes with more than 1 parent
        public DomTree ParseBrigitText()
        {
            if (muncher.SniffChar() != '[')
                throw new Exception("brigit txt must start with [load] tag");
            else
                muncher.ConsumeChar();
            DomTree tree = ParseLoadTag();
            DomNode oldNode = null;
            while(!muncher.Complete())
            {
                DomNode[] children = ParseNode();
                if(tree.Head == null)
                {
                    if(children.Length != 1)
                    {
                        // TODO, create empty node
                    }
                    else
                    {
                        tree.Head = children[0];
                        oldNode = tree.Head;
                    }
                }
                else if(oldNode != null)
                {
                    oldNode.Children = children;
                    oldNode = children[0];
                }
            }

            return tree;    
        }

        /// <summary>
        /// Generic Parser which will choose which method to call
        /// depending on the tag it finds
        /// </summary>
        /// <returns></returns>
        public DomNode[] ParseNode()
        {
            DomNode[] node = null;
            // Eat the fluff in front until muncher sees a open bracket
            muncher.EatWhiteSpace();
            if (muncher.CheckChar('['))
            {
                muncher.ConsumeChar();
                if (muncher.StartsWith("res"))
                {
                    node = new DomNode[] { ParseResTag() };
                }
                else if (muncher.StartsWith("rep"))
                { }
                else
                { }
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
            // Eating awy until the next tag
            muncher.EatWhiteSpace();
            return node;
        }

        /// <summary>
        /// Parses the load tag which must be set at the beginning of the file
        /// </summary>
        /// <returns>Returns a new DomTree that will be used as the base</returns>
        public DomTree ParseLoadTag()
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

        /// <summary>
        /// Parses an entire Response tag all the way until after the 
        /// final ']'
        /// </summary>
        /// <returns></returns>
        public DomNode ParseResTag()
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
        public string ParseParagraphs()
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
        public Dictionary<string, string[]> ParseArgumentSetPairs()
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
        public string[] ParseSetOfStrings()
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
        // the overall position of the eater
        int pos;
        // The actual line and position of the character
        int lineNum;
        int posNum;

        public string Text
        {
            get { return data; }
            set { data = value; }
        }

        public int Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public Eater():this(string.Empty)
        {
        }

        public Eater(string data)
        {
            this.data = data;
            pos = 0;
        }

        /// <summary>
        /// Checks if the eater has reached the EOF
        /// </summary>
        /// <returns></returns>
        public bool Complete()
        {
            return pos >= data.Length;
        }

        /// <summary>
        /// Gets the remaining string to be parsed
        /// </summary>
        /// <returns></returns>
        public string GetRemainingString() => data.Substring(pos);


        /// <summary>
        /// Checks to see if a the char at pos is equal to the given char
        /// </summary>
        /// <param name="c">A Char</param>
        /// <returns>True or False</returns>
        public bool CheckChar(char c)
        {
            bool b = false;
            if(!Complete())
            {
                b = c == data[pos];
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
