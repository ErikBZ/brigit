using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Attributes;
using Brigit.Structure;

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
            if (scene.GlobalFlags == null)
            {
                scene.GlobalFlags = new Dictionary<string, bool>();
            }
            return scene;
        }

        public DomTree ParseDomTree(bool sceneIsBranch)
        {
            DomTree scene = new DomTree();
            // add stuff for parses things before the actually starts here
            // idk what but stuff I guess

            muncher.EatWhiteSpace();
            if (sceneIsBranch)
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
                if (muncher.StartsWith("  "))
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
                DomTree branch = null;
                numberOfChoices++;

                // Parsing the plain text that a character will say
                choices.Add(ParseSpeechText());

                // Every plaintext speech blurb must be ended with an asterisk
                if (muncher.SpitChar() != '*')
                {
                    throw new Exception($"Speech text at {muncher.Position} did not end in an *");
                }

                // if there's a paren then it means this choice has local or global
                // flags to set or require
                // TODO parse required, local, and global flags
                if (muncher.CheckChar('('))
                {
                    Dictionary<string, Flag> thingy = ParseNodeAttributes();
                }

                // attibute and flag requirements here.
                // if there are none then the default will be used
                if (muncher.StartsWith("->"))
                {
                    muncher.ConsumeString("->");
                    if (muncher.CheckChar('{'))
                    {
                        branch = ParseDomTree(true);
                    }
                    else
                    {
                        throw new Exception($"New branch must start with open '{{'. {muncher.Position}");
                    }
                }
                else
                {
                    // TODO Create add an empty node as a possible branch if a choice does not point
                    // to a branch.
                    branch = DomTree.CreateEmptyDomTree();
                }

                branches.Add(branch);
                muncher.EatWhiteSpace();
            }
            // eating the last closing bracket
            muncher.ConsumeChar();
            node.Choices = choices.ToArray();
            tree.Add(node);
            tree.Add(branches.ToArray());
            return tree;
        }

        /// <summary>
        /// Parses a attributes of a node like expression, flag requirements and other things
        /// </summary>
        /// <returns></returns>
        // an open paren should always be the first thing that this method sees
        // ( [a-z]* )
        private Dictionary<string, Flag> ParseNodeAttributes()
        {
            Dictionary<string, Flag> attributes = new Dictionary<string, Flag>();

            // will error if this is not true
            muncher.ConsumeChar('(');

            // eats a string and then stops at the next :
            while (!muncher.CheckChar(')'))
            {
                string attr = muncher.SpitUpAlpha();
                // after the attribute there must be a semicolon to show that the value is coming next
                muncher.ConsumeChar(':');

                // depending on the value there maybe more than one value for the attribute like for flags
                string[] values = ParseAttributeValues().ToArray();
            }
            muncher.ConsumeChar(')');
            return attributes;
        }

        // will eat whitespace first to make sure it can start at an alpha character
        private List<string> ParseAttributeValues()
        { 
            List<string> values = new List<string>();

            muncher.EatWhiteSpace();
            
            // if it's an end paren then the attributes list has ended, otherwise
            // if it s a comma then it is the end of the value list
            while(!(muncher.CheckChar(')') || muncher.CheckChar(',')))
            {
                values.Add(muncher.SpitUpAlpha());
                // values should be delimited by spaces
                muncher.EatWhiteSpace();
            }

            if(muncher.CheckChar(','))
            {
                muncher.ConsumeChar();
            }

            return values;
        }


        /// <summary>
        /// Parses a characater name
        /// </summary>
        /// <returns></returns>
        private string ParseCharacterName()
        {
            string characterName = muncher.SpitUpWhile(delegate (char c)
            {
                if (Char.IsLetterOrDigit(muncher.SniffChar()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
            if (!characters.Contains(characterName))
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
            Dictionary<string, Flag> values = new Dictionary<string, Flag>();
            if (muncher.CheckChar('('))
            {
                ParseNodeAttributes();
            }

            muncher.ConsumeChar('{');
            // parsing the actual text
            while (muncher.SniffChar() != '}')
            {
                DomNode newNode = ParseSingleDialog();
                newNode.Character = character;
                tree.Add(newNode);
                char asterisk = muncher.SniffChar();
                if (asterisk == '*')
                {
                    muncher.ConsumeChar();
                }
            }

            // eating the '}'
            muncher.ConsumeChar();

            return tree;
        }
    }
}