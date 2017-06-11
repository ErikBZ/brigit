using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Attributes;

namespace Brigit.Structure
{
	public class Selection
	{
		public string Text { get; set; }
		public AttributeManager Attributes { get; set; }

		public Selection()
		{
			Text = string.Empty;
			Attributes = new AttributeManager();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Selection))
			{
				return false;
			}

			Selection other = (Selection)obj;

			// checking the text
			bool equal = true;
			equal &= Text == other.Text;
			equal &= this.Attributes.Equals(other.Attributes);

			return equal;
		}

		public override string ToString()
		{
			return Text;
		}
	}
}
