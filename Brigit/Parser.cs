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
        
        public Parser(string[] data)
        {
            muncher = new Eater(data);
        }

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
                    oldNode.SetChildren(children);
                    oldNode = children[0];
                }
                muncher.EatWhiteSpace();
            }
            Console.WriteLine("end reached with no issues");
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
                        Console.WriteLine("res tag cannot have more than 1 set per parameter");
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

            // TODO add eat tag to Eater and give it more versatility
            // eating up the [*] ending tag
            if(muncher.StartsWith("[*]"))
            {
                muncher.ConsumeChar(3);
            }
            else
            {
                Console.WriteLine("RES tag did not end properly");
                muncher.EatWhile(delegate (char c)
                {
                    return c != ']';
                });
            }
            if(muncher.SniffChar() == ']')
            {
                muncher.ConsumeChar();
            }
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

            str = Regex.Replace(str, @"\n", " ");
            return str;
        }

        /// <summary>
        /// parses an entire tag of arguments for any tag
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> ParseArgumentSetPairs()
        {
            Dictionary<string, string[]> arguments = new Dictionary<string, string[]>();
            while (!muncher.CheckChar(']'))
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
                Console.WriteLine("end of tag not found");
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
                {
                    Console.WriteLine("Malformed line");
                }

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
        string[] all_text;
        // The actual line and position of the character
        int lineNum;
        int posNum;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(string s in all_text)
            {
                sb.Append(s);
            }
            return sb.ToString();
        }

        public string Position
        {
            // the plus 1 is to normalize it
            get { return $"Line: {lineNum+1}, Position: {posNum+1}"; }
        }

        public Eater():this(null)
        {
        }

        public Eater(string[] text)
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
            for(int i=lineNum;i<all_text.Length;i++)
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
            if(!Complete())
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
        /// Steps forware by x in the string
        /// </summary>
        /// <param name="x">Skip x chars</param>
        // Slower but safer
        public void ConsumeChar(int x)
        {
            for(int i=0;i<x && !Complete();i++)
            {
                ConsumeChar();
            }
            if(Complete())
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
        public void EatWhile(Func<Char, bool> predicate)
        {
            while(predicate(SniffChar()) && !Complete())
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
        /// Consumes a string of characters then spits them back up
        /// </summary>
        /// <param name="pred">A predicate that will test a char</param>
        /// <returns>Returns a string fufilling the predicate</returns>
        public string SpitUpWhile(Func<char, bool> pred)
        {
            StringBuilder sb = new StringBuilder();
            while(pred(SniffChar()) && !Complete())
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
            String sub = all_text[lineNum].Substring(posNum);
            bool b = sub.StartsWith(s);
            return b;
        }
    }
}
