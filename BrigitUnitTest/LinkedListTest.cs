using System;
using Brigit.Structure;
using NUnit.Framework;

namespace Brigit.Test
{
	[TestFixture]
	public class LinkedListTest
	{
		// TODO add more linkedlist manipulation tests
		// or create constructed linked lists to verify

		[Test]
		// Tests to make sure that you can add a node just fine
		public void Add_NodeToLinkList_NoException()
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

			string dotFile = ll.ToString();
		}

		[Test]
		public void Add_LinkedListToLinkedList_NoException()
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

			ll.Add(ll2);
		}

		[Test]
		public void Add_BranchingLinkedListToLinkedList_NoException()
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
		}

		[Test]
		public void Add_GraphInBetweenNodes_NoException(){
			BrigitGraph bg = new BrigitGraph();
			bg.Add(new Node() { Data = 1 });
			bg.Add(new Node() { Data = 3 });

			BrigitGraph bg2 = new BrigitGraph();
			bg2.Add(new Node() { Data = 6 });
			bg2.Add(new Node() { Data = 7 });

			bg.AddInBetween(bg.Head, bg2);
		}
	}
}
