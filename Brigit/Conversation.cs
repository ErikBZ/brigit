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

                var data = curr.Data as Descision;
                data = GetAvailableChoices(data);
                inf = new Info (data);
            }
            else
            {
                throw new Exception("Data does not match");
            }

            return inf;
        }

        // takes a Descision and creates a new block with
        // the avaiable choices a player can make
        public Descision GetAvailableChoices(Descision data)
        {
            var payload = new Descision();

            for(int i=0; i<data.Choices.Count;i++)
            {
                var newChoice = new Choice();
                newChoice.Attributes = data.Choices[i].Attributes;
                newChoice.Text = data.Choices[i].Text;
                newChoice.NextNode = i;
                payload.Choices.Add(newChoice);
            }

            return payload;
        }

        // go to next MUST return true other wise we should stop execution since something went wrong
        public bool Next(int playerChoice = 0)
        {
            do
            {
                // this will almost never fail
                if (curr.Data is Dialog)
                {
                    curr = DialogNext(curr);
                }
                else if (curr.Data is Descision)
                {
                    // ohhh this already did the thing. woops
                    // removing functionality so that the choice is made on the frontend
                    // but verfied here
                    var next = ChoiceNext(curr, playerChoice);

                    // there was a viable next node but something went wrong
                    if (next == null && curr.Next.Count != 0)
                    {
                        return false;
                    }
                    else
                    {
                        curr = next;
                    }
                }
            } while (!IsValidState());

            return true;
        }

        // choice is the index of the chosen choice in the list
        // not of the next node
        private Node ChoiceNext(Node curr, int choice)
        {
            Descision data = curr.Data as Descision;
            if(choice < data.Choices.Count)
            {
                ResetLocals();
                int next = data.Choices[choice].NextNode;
                SetFlags(data.Choices[choice].Attributes.SetFlags);
                return curr.Next[next];
            }
            else
            {
                return null;
            }
        }

        private Node DialogNext(Node curr)
        {
            Dialog dia = curr.Data as Dialog;
            SetFlags(dia.Text[tracker].Attributes.SetFlags);

            tracker++;
            var next = curr;
            if(tracker >= dia.Text.Count)
            {
                next = curr.Next.Count != 0 ? curr.Next[0] : null;
                tracker = 0;
            }

            return next;
        }

        private bool IsValidState()
        {
            if (Complete || curr.Data is Descision)
            {
                return true;
            }
            else if(curr.Data is Dialog)
            {
                var data = curr.Data as Dialog;
                var text = data.Text[tracker];
                return data.Attributes.Expression.Evaluate(LocalFlags, null) == Flag.True &&
                       text.Attributes.Expression.Evaluate(LocalFlags, null) == Flag.True;
            }
            else
            {
                return false;
            }
        }

        // this takes an attribute manager and sets the local or global flags
        private void SetFlags(Dictionary<String, Flag> flags)
        {
            var dict = flags;
            foreach(KeyValuePair<string, Flag> kvp in dict)
            {
                LocalFlags.Add(kvp.Key, kvp.Value);
            }
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
