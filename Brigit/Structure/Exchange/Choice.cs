using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Attributes;

namespace Brigit.Structure.Exchange
{
	public class Choice
	{
		public string Text { get; set; }
		public AttributeManager Attributes { get; set; }
		// This points to the index of the node it maps to
		// I could make it a direct pointer
		public int NextNode { get; set; }

		public Choice()
		{
			Attributes = new AttributeManager();
			Text = string.Empty;
			NextNode = -1;
		}

		public Choice(string text)
		{
			Attributes = new AttributeManager();
			Text = text;
			NextNode = 0;
		}

		public Choice(string text, int next)
		{
			Attributes = new AttributeManager();
			Text = text;
			NextNode = next;
		}

        public override string ToString()
        {
            return Text;
        }

        public override bool Equals(object obj)
		{
			if(obj is Choice)
			{
				Choice ch = obj as Choice;
				bool equal = this.Text == ch.Text;
				equal &= this.NextNode == ch.NextNode;
				equal &= this.Attributes.Equals(ch.Attributes);

				return equal;
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
            return Text.GetHashCode();
		}
	}
}
