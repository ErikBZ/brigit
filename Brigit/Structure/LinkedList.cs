using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brigit.Structure
{
	public class LinkedList
	{
		// impossible to have a linked list start with multiple heads
		// since Descision, Dialog and BranchSelector are all guarteened to be before
		// multiple nodes
		public Node Head;
		public List<Node> Tails;

		public LinkedList()
		{
			Head = null;
			Tails = new List<Node>();
		}

		public LinkedList(Node node)
		{
			Head = node;
			Tails = new List<Node>() { node };
		}

		/// <summary>
		/// Adds a node to the end of the LinkedList and sets it as the Tail
		/// </summary>
		/// <param name="node"></param>
		public void Add(Node node)
		{
			if(Head == null)
			{
				Head = node;
			}
			else
			{
				// adding the node as the next node
				// of all tail nodes
				foreach(Node n in Tails)
				{
					n.Next.Add(node);
				}
			}

			// this node does not point to anything else
			node.Next.Clear();

			// clearing the old tails
			Tails.Clear();
			// this is now the last node in the Linked List
			Tails.Add(node);
		}

		/// <summary>
		/// Adds another LinkedList to the end of this Linked List and set's the additional linked list's
		/// tails to the tail of this linked list
		/// </summary>
		/// <param name="ll"></param>
		public void AddToEnd(LinkedList ll)
		{
			// this means that this linked list is 
			// empty
			if (this.Head == null)
			{
				this.Head = ll.Head;
			}
			else
			{
				foreach(Node n in this.Tails)
				{
					n.Next.Add(ll.Head);
				}
			}

			this.Tails = ll.Tails;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <param name="ll"></param>
		/// <returns>The node where the branch was added</returns>
		// this will consolodate the tails of this linkied list with the linked list
		// that is being added
		public Node AddBranch(Node node, LinkedList ll)
		{
			node.Next.Add(ll.Head);

			foreach(Node n in ll.Tails)
			{
				this.Tails.Add(n);
			}

			return node;
		}

		/// <summary>
		/// Set's a node somewhere inside of the linked list as a tail 
		/// </summary>
		/// <param name="node"></param>
		public void SetAsTail(Node node)
		{
			this.Tails.Add(node);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			Queue<Node> que = new Queue<Node>();
			que.Enqueue(this.Head);

			while(que.Count != 0)
			{
				Node curr = que.Dequeue();

				// hopefully there are no cycles in the code
				foreach(Node n in curr.Next)
				{
					sb.Append(curr.Data.ToString());
					sb.Append(" -> ");
					sb.Append(n.ToString());
					sb.Append('\n');

					que.Enqueue(n);
				}
			}
			return sb.ToString();
		}

		public override bool Equals(object obj)
		{
			LinkedList other;
			if (obj is LinkedList)
			{
				other = obj as LinkedList;
			}
			else
			{
				return false;
			}

			bool equal = true;
			Queue<Node> otherQueue = new Queue<Node>();
			Queue<Node> thisQueue = new Queue<Node>();
			otherQueue.Enqueue(other.Head);
			thisQueue.Enqueue(this.Head);

			// need to use queues for this
			while (otherQueue.Count != 0 && thisQueue.Count != 0 && equal)
			{
				var thisNode = thisQueue.Dequeue();
				var otherNode = otherQueue.Dequeue();

				equal = thisNode.Equals(otherNode);

				// adding the next nodes to check
				foreach(Node n in thisNode.Next)
				{
					thisQueue.Enqueue(n);
				}

				foreach(Node n in otherNode.Next)
				{
					otherQueue.Enqueue(n);
				}
			}

			equal &= otherQueue.Count == thisQueue.Count;

			return equal;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
