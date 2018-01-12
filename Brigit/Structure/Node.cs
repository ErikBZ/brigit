using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brigit.Structure
{
	public class Node
	{
		// dyanmic for now until i figure out a beter way
		public object Data { get; set; }
		// empty list means there's nothing there
		public List<Node> Next { get; set; }

		public Node()
		{
			Next = new List<Node>();
		}
		public override string ToString()
		{
			return Data.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj is Node)
			{
				return this.Data.Equals((obj as Node).Data);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
