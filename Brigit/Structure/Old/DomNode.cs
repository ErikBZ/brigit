using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using Brigit.Attributes;
using Brigit.Structure.Old;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit.Structure
{
    [Serializable, DataContract]
    [KnownType(typeof(Dialog))]
    [KnownType(typeof(UserChoice))]
    public class DomNode
    {
        /// <summary>
        /// The possible leaves this Node has
        /// </summary>
        [DataMember]
        public DomNode[] Children { get; set; }

        /// <summary>
        /// The flags required to get this leaf to appear and what
        /// those flags should evaluate to, try avoiding situations where
        /// more than 1 leaf can be activated by an overlapping set of flags
        /// </summary>
        [DataMember]
        /// <summary>
        /// The character that "owns" this node, IE the character who said this
        /// </summary>
		public string Character { get; set; }

        /// <summary>
        /// The background for the character. Will usually not change, so the first
        /// node can have the background set and all other nodes will inherit it
        /// </summary>
        [DataMember]
		// TODO put this in attribute
        string background;

		[DataMember]
		public AttributeManager Attributes { get; set; }

        [DataMember]
		public NodeType Type { get; set; }

        public string Background
        {
            get { return background; }
            set { background = value; }
        }

        /// <summary>
        /// Default constructor, creates a Dom Node with no indeces.
        /// </summary>
        public DomNode()
        {
            Children = new DomNode[0];
            Character = string.Empty;
			Attributes = new AttributeManager();
        }

		public static DomNode CreateEmptyDomNode()
		{
			DomNode empty = new DomNode
			{
				Type = NodeType.Empty
			};

			return empty;
		}

        /// <summary>
        /// Creates a new Dom Node for a tree.
        /// </summary>
        /// <param name="children"></param>
        public DomNode(DomNode[] children)
        {
            Children = children;
        }
        
        public DomNode(DomNode[] children, string flags,
            Dictionary<string, bool> flagSets, string character)
        {
            this.Children = children;
            this.Character = character;
        }

		public bool IsEmpty()
		{
			return Type == NodeType.Empty;
		}

        /// <summary>
        /// Will get you the next node given the context of the scene.
        /// IE, it will equate the flags and get the next node
        /// </summary>
        /// <returns></returns>
        public virtual DomNode GetNext(int choice, DomTree scene)
        {
            DomNode next = null;
            // shifting it to the left since 1 is mapped to 0
            if(Children.Length == 0)
            {
                return null;
            }

            foreach (DomNode child in Children)
            {
				// TODO I may want to change this later so that I can do different things
				// for different flags
                bool isNextChild= child.EvaluateFlags(scene) == Flag.True;
                if (isNextChild && next != null)
                {
                    // throw exception more than one node has met the flags
                }
                else if (isNextChild && next == null)
                {
                    next = child;
                    if(next.Type == NodeType.Empty)
                    {
                        next = next.GetNext(choice, scene);
                    }
                }
            }
            // really throw an excpetion here if none of the nodes meet the requirements
            if (next == null)
            {
                throw new Exception("No node matched the combination of flags raised. Consider using default branch");
            }

            return next;
        }

        /// <summary>
        /// Given the current context of the scene with flags, it returns
        /// a bool indicating if it's flag requirements are met
        /// </summary>
        /// <returns></returns>
        public Flag EvaluateFlags(DomTree scene)
        {
			return Attributes.Expression.Evaluate(scene.LocalFlags, scene.GlobalFlags);
        }

        /// <summary>
        /// Set the possible branches for this node
        /// </summary>
        /// <param name="next"></param>
        public void SetChildren(DomNode[] next)
        {
            if(next.Length == 1 && next[0].Type == NodeType.Empty)
            {
                // set this nodes Children to the Children of the empty node
                this.SetChildren(next[0].Children);
            }
            else
            {
                this.Children = next;
            }
        }

        /// <summary>
        /// Get all the Children of this node
        /// </summary>
        /// <returns></returns>
        public DomNode[] GetChildren()
        {
            return Children;
        }

        public override string ToString()
        {
            return "Dom Node";
        }

        public override bool Equals(object obj)
        {
            if(obj == null || !(obj is DomNode))
            {
                return false;
            }

            DomNode node = (DomNode)obj;
			/*
             * Checks the flags that the node sets, the number of children,
             * the character that owns the node, and what the flags the node sets
             */
			// splitting up the boolean expresssion since it's easier to debug that way

			bool nodesAreEqual = this.Children.Length == node.Children.Length;
			nodesAreEqual &= this.Character.Equals(node.Character);
			nodesAreEqual &= this.Attributes.Equals(node.Attributes);

            return nodesAreEqual;
        }

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
