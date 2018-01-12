using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Interface;

namespace Brigit
{
    // what we are aiming for
    public class Conversation
    {
        // should be a unique name and indentifier
        // will be the name of the file
        public string ConversationName { get; set; }
        private BrigitGraph ll;

        private int tracker;
        private Node curr;

        public bool Complete { get { return curr == null; } }

        public Conversation()
        {
            ConversationName = string.Empty;
            ll = new BrigitGraph();
        }

        public Conversation(BrigitGraph ll)
        {
            ConversationName = "thingy";
            this.ll = ll;
            curr = ll.Head;
        }

        public void StartNewRun()
        {
            // resets stuff
            tracker = 0;
            curr = ll.Head;
        }

        // gets the next text for the dialog
        public Renderable GetCurr()
        {
            Renderable rend = new Renderable();

            if (curr.Data is Dialog)
            {
                Dialog dia = curr.Data as Dialog;
                rend.CharacterName = dia.Character;
                rend.Info = dia.Text[tracker].Text;
            }
            else if (curr.Data is Descision)
            {
                Descision decide = curr.Data as Descision;
                rend.CharacterName = "Player";
                rend.Info = DescisionToString(decide);
            }
            else
            {

            }

            return rend;
        }

        public Info GetInfo()
        {
            Info inf = new Info();

            if (curr.Data is Dialog)
            {
                Dialog d = curr.Data as Dialog;
                inf = ConvertToInfo(d, tracker);
            }
            else if (curr.Data is Descision)
            {
                Descision d = curr.Data as Descision;
                inf = ConvertToInfo(d);

            }
            else
            {
                throw new Exception("Data does not match");
            }

            return inf;
        }

        public Info ConvertToInfo(Descision dec)
        {
            Info inf = new Info();
            inf.Character = "player";

            foreach (Choice ch in dec.Choices)
            {
                inf.Text.Add(ch.Text);
            }

            inf.type = Info.Type.Choice;

            return inf;
        }

        public Info ConvertToInfo(Dialog dia, int tracker)
        {
            Info inf = new Info();

            inf.Character = dia.Character;

            inf.Text.Add(dia.Text[tracker].Text);

            inf.type = Info.Type.Dialog;

            return inf;
        }

        // go to next MUST return true other wise we should stop execution since something went wrong
        public bool GoToNext(int playerChoice = 0)
        {
            // check first if this conv is complete
            if (curr.Next.Count == 0)
            {
                curr = null;
                return true;
            }

            // if Data is a dialog i can ignore next
            // this will almost never fail
            if (curr.Data is Dialog)
            {
                Dialog dia = curr.Data as Dialog;
                tracker++;
                // off by 1 error. this should check for equality as well
                // since tracker starts at 0 and count starts at 1
                if (tracker >= dia.Text.Count)
                {
                    // very simple way of doing things
                    curr = curr.Next[0];
                    ResetLocals();
                }
            }
            else if (curr.Data is Descision)
            {
                Descision decide = curr.Data as Descision;
                if (playerChoice < decide.Choices.Count)
                {
                    // player chose their choice, and that choice object holds
                    // the info for the true next node
                    int nextNode = decide.Choices[playerChoice].NextNode;

                    // it's possible to have branches and no next default.
                    // if a choice has no where to go, it should return null
                    // I.E if a choice points to an out of bounds index
                    if(nextNode >= curr.Next.Count)
                    {
                        curr = null;
                    }
                    else
                    {
                        curr = curr.Next[nextNode];
                    }
                    ResetLocals();
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void ResetLocals()
        {
            tracker = 0;
        }


		private string DescisionToString(Descision dec)
		{
			StringBuilder sb = new StringBuilder();

			for(int i=0;i<dec.Choices.Count;i++)
			{
				sb.Append(i);
				sb.Append(": ");
				sb.Append(dec.Choices[i].Text);
				sb.Append('\n');
			}

			return sb.ToString();
		}
	}
}
