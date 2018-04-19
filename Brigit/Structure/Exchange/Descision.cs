﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Attributes;

namespace Brigit.Structure.Exchange
{
	public class Descision : ExchangeUnit
	{
		public List<Choice> Choices { get; set; }
        // false if you want the block to get the next node on it's own
        // by evaluating the attributes of each choice
        public bool Interactive { get; set; }

		public Descision()
		{
			Choices = new List<Choice>();
            Interactive = true;
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
            equal &= this.Interactive == other.Interactive;

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
            int hash = 0;
            foreach(Choice ch in Choices)
            {
                hash ^= ch.GetHashCode();
            }
            hash = (~0xf & hash) | Choices.Count();
			return hash;
		}
	}
}