using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Attributes;
using Brigit.Attributes.Operators;
using Brigit.Structure;

// Rename BrigitParser to BrigitParser
namespace Brigit.TomeParser
{

    /// <summary>
    /// Parses a tome document into a DOM data structure
    /// </summary>
    public partial class TomeParser
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
                scene.GlobalFlags = new Dictionary<string, Flag>();
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
    }
}