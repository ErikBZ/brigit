using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Attributes;

namespace Brigit.Structure.Exchange
{
	public class SpeechText
	{
		public string Text { get; set; }
		public AttributeManager Attributes { get; set; }

		public SpeechText()
		{
			Text = string.Empty;
			Attributes = new AttributeManager();
		}

		public SpeechText(string text)
		{
			Text = text;
			Attributes = new AttributeManager();
		}

		public override bool Equals(object obj)
		{
			SpeechText other;
			if (obj is SpeechText)
			{
				other = obj as SpeechText;
			}
			else
			{
				return false;
			}

			// only checking text for now
			bool checker = this.Attributes.Equals(other.Attributes);
			checker &= other.Text == this.Text;
			return checker;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
