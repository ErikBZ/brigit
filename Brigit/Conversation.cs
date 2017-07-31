using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public Conversation()
		{
			ConversationName = string.Empty;
			ll = new BrigitGraph();
		}

		public Conversation(BrigitGraph ll)
		{
			ConversationName = "thingy";
			this.ll = ll;
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

		// if a next couldn't be chosen then tell the player that
		public bool GoToNext(int next)
		{
			// if Data is a dialog i can ignore next
			// this will almost never fail
			if(curr.Data is Dialog)
			{
				Dialog dia = curr.Data as Dialog;
				tracker++;
				if(tracker > dia.Text.Count)
				{
					// very simple way of doing things
					curr = curr.Next[0];
				}
			}
			else if(curr.Data is Descision)
			{
				Descision decide = curr.Data as Descision;
				if(next < decide.Choices.Count)
				{
					// arrays start at 0
					// human arrats start at 1
					curr = curr.Next[next-1];
				}
				else
				{
					return false;
				}
			}

			return true;
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

		public bool Complete()
		{
			return curr.Next.Count == 0;
		}
	}
}
