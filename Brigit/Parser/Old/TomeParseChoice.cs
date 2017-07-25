using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit;
using Brigit.Structure;
using Brigit.Attributes;

namespace Brigit.Parser
{
	partial class TomeParser 
	{
        /// <summary>
        /// Parses the possible choices that a player has for a certain part
        /// </summary>
        /// <returns></returns>
        private DomTree ParseChoice()
        {
            // this will be used later when trees can be created within a CHOICE block
            DomTree tree = new DomTree();
            // for now we only really care about making sure Chioce works
            UserChoice node = new UserChoice();

            // parsing the header and the beginning
            muncher.ConsumeString("CHOICE");
            muncher.EatWhiteSpace();
            
            // attribute parsing here? yeah probably but i'll save that for later
            // TODO add the parsing of the attributes here
			if(muncher.CheckChar('['))
			{
				muncher.ConsumeChar('[');
				AttributeManager attributes = ParseAttributes();
				// attribute manager does not eat the remaining end paren
				muncher.ConsumeChar(']');
			}

            muncher.ConsumeChar('{');
            muncher.EatWhiteSpace();


            List<Selection> choices = new List<Selection>();
            List<DomTree> branches = new List<DomTree>();
            int numberOfChoices = 0;

            while (!muncher.CheckChar('}'))
            {
                DomTree branch = null;
                numberOfChoices++;

				String text = ParseSpeechText();
				Selection parsedSelection = new Selection()
				{
					Text = text
				};

                // Every plaintext speech blurb must be ended with an asterisk
                if (muncher.SpitChar() != '*')
                {
                    throw new Exception($"Speech text at {muncher.Position} did not end in an *");
                }

                // if there's a paren then it means this choice has local or global
                // flags to set or require
                // TODO parse required, local, and global flags
                if (muncher.CheckChar('['))
                {
                    AttributeManager attr = ParseAttributes();
					parsedSelection.Attributes = attr;
                }

				choices.Add(parsedSelection);

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

	}
}
