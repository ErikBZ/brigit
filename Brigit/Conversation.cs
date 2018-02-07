using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Attributes;
using Brigit.Attributes.Operators;

namespace Brigit
{
    // what we are aiming for
    public class Conversation
    {
        // should be a unique name and indentifier
        // will be the name of the file
        public string ConversationName { get; set; }
        private BrigitGraph ll;

        // the Flags
        private static Dictionary<string, Flag> GlobalFlags;
        private Dictionary<string, Flag> LocalFlags;

        private int tracker;
        private Node curr;

        public bool Complete { get { return curr == null; } }

        public Conversation()
        {
            ConversationName = string.Empty;
            ll = new BrigitGraph();
            LocalFlags = new Dictionary<string, Flag>();
        }

        public Conversation(BrigitGraph ll)
        {
            ConversationName = "thingy";
            this.ll = ll;
            curr = ll.Head;
            LocalFlags = new Dictionary<string, Flag>();
        }

        public void StartNewRun()
        {
            // resets stuff
            tracker = 0;
            curr = ll.Head;
        }

        public Info GetInfo()
        {
            Info inf;

            if (curr.Data is Dialog)
            {
                inf = new Info(curr.Data as Dialog, tracker);
            }
            else if (curr.Data is Descision)
            {

                inf = new Info (curr.Data as Descision);
            }
            else
            {
                throw new Exception("Data does not match");
            }

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
                // ohhh this already did the thing. woops
                // removing functionality so that the choice is made on the frontend
                // but verfied here
                Descision decide = curr.Data as Descision;
                if (playerChoice < curr.Next.Count)
                {
                    curr = curr.Next[playerChoice];
                    ResetLocals();
                }
                // The descision was invalid and should not move forward
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
