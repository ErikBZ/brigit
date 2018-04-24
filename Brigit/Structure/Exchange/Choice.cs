using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Attributes;
using System.Runtime.Serialization;

namespace Brigit.Structure.Exchange
{
	[DataContract]
	// A single choice of many
	// Many choices make a descision? that doesn't really make sense
	// Choice is a unit of a Descsion.
	// Maybe call this Option and rename Descision to Choice? **** this is the winner
	public class Choice
	{
		[DataMember]
		public string Text { get; set; }
		[DataMember]
		public AttributeManager Attributes { get; set; }
		// This points to the index of the node it maps to
		// I could make it a direct pointer
		[DataMember]
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
