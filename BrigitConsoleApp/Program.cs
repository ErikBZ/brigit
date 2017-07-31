using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit;
using Brigit.Structure;

namespace BrigitConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			BrigitGraph ll = new BrigitGraph();
			Node n1 = new Node()
			{
				Data = 1
			};
			ll.Add(n1);

			Node n2 = new Node()
			{
				Data = 2
			};
			ll.Add(n2);

			Node n3 = new Node()
			{
				Data = 3
			};
			ll.Add(n3);

			Node nn1 = new Node() { Data = 4 };
			Node nn2 = new Node() { Data = 5 };
			Node nn3 = new Node() { Data = 6 };
			BrigitGraph ll2 = new BrigitGraph();
			ll2.Add(nn1);
			ll2.Add(nn2);
			ll2.Add(nn3);

			ll.AddBranch(n2, ll2);

			string dotFile = ll.ToString();

			Console.WriteLine(dotFile);
			Console.ReadLine();
		}
	}
}
