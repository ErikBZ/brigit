using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Structure;
using Brigit.Attributes;

namespace Brigit.Parser
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

			AttributeManager am = new AttributeManager();

            if (muncher.CheckChar('['))
            {
				// safe to assume char is being eaten is [
				muncher.ConsumeChar();
				am = ParseAttributes();
				// forcing the section to end
				muncher.ConsumeChar(']');
            }

            muncher.ConsumeChar('{');
            // parsing the actual text
            while (muncher.SniffChar() != '}')
            {
                DomNode newNode = ParseSingleDialog();
                newNode.Character = character;
				newNode.Attributes = am;
                tree.Add(newNode);
                char asterisk = muncher.SniffChar();
                if (asterisk == '*')
                {
                    muncher.ConsumeChar();
                }
				am = new AttributeManager();
            }

            // eating the '}'
            muncher.ConsumeChar();

            return tree;
        }

	}
}
