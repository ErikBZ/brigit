using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Brigit.Structure;

namespace Brigit.Test
{
	[TestClass]
	public class LinkedListTest
	{
		[TestMethod]
		// Tests to make sure that you can add a node just fine
		public void AddNodeToLinkedList()
		{
			LinkedList ll = new LinkedList();
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

		[TestMethod]
		public void AddLinkedListToLinkedList()
		{
			LinkedList ll = new LinkedList();
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
			LinkedList ll2 = new LinkedList();
			ll2.Add(nn1);
			ll2.Add(nn2);
			ll2.Add(nn3);

			ll.AddToEnd(ll2);
		}

		[TestMethod]
		public void AddLinkedListBranchToLinkedList()
		{
			LinkedList ll = new LinkedList();
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
			LinkedList ll2 = new LinkedList();
			ll2.Add(nn1);
			ll2.Add(nn2);
			ll2.Add(nn3);

			ll.AddBranch(n2, ll2);
		}
	}
}
