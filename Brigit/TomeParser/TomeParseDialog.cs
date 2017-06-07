using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Structure;
using Brigit.Attributes;

namespace Brigit.TomeParser
{
	partial class TomeParser
	{
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
            if (muncher.CheckChar('['))
            {
				// safe to assume char is being eaten is [
				muncher.ConsumeChar();
                ParseAttributes();
				// forcing the section to end
				muncher.ConsumeChar(']');
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
