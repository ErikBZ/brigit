using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Attributes;

namespace Brigit.Structure.Exchange
{
	public class Descision
	{
		public List<Choice> Choices { get; set; }

		public Descision()
		{
			Choices = new List<Choice>();
		}

		public override bool Equals(object obj)
		{
			if(!(obj is Descision))
			{
				return false;
			}
			Descision other = obj as Descision;
			int i = 0;
			bool equal = this.Choices.Count == other.Choices.Count;

			while(i < Choices.Count && equal)
			{
				Choice thisChoice = this.Choices[i];
				Choice otherChoice = other.Choices[i];
				equal &= thisChoice.Equals(otherChoice);
				i++;
			}

			return equal;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}