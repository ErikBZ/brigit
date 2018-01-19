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
            int hash = GetHashCode();
            StringBuilder sb = new StringBuilder();
            if(Data is Exchange.Descision)
            {
                sb.Append("DescisionBlock_");
            }
            else if (Data is Exchange.Dialog)
            {
                sb.Append("DialogBlock_");
            }

            // adding the hash
            sb.Append(hash.ToString("X4"));
			return sb.ToString();
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
			return Data.GetHashCode();
		}
	}
}
