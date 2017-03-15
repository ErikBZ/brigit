using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit.Structure
{
    [Serializable, DataContract]
    [KnownType(typeof(Dialog))]
    [KnownType(typeof(Choice))]
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
        /// TODO refactor this to use the new Flag enum
        public string RequiredFlags { get; set; }

        /// <summary>
        /// The flags that this Node will set
        /// </summary>
        [DataMember]
        Dictionary<string, bool> flagSets;

        /// <summary>
        /// The character that "owns" this node, IE the character who said this
        /// </summary>
        [DataMember]
        string character;

        /// <summary>
        /// The background for the character. Will usually not change, so the first
        /// node can have the background set and all other nodes will inherit it
        /// </summary>
        [DataMember]
        string background;

        [DataMember]
        NodeType type;

        public Dictionary<string, bool> FlagToggles
        {
            get { return flagSets; }
            set { flagSets = value; }
        }

        public string Character
        {
            get { return character; }
            set { character = value; }
        }

        public string Background
        {
            get { return background; }
            set { background = value; }
        }

        public NodeType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Default constructor, creates a Dom Node with no indeces.
        /// </summary>
        public DomNode()
        {
            Children = new DomNode[0];
            RequiredFlags = string.Empty;
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
            this.RequiredFlags = flags;
            this.flagSets = flagSets;
            this.character = character;
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
                bool isNextChild= child.EvaluateFlags(scene);
                if (isNextChild && next != null)
                {
                    // throw exception more than one node has met the flags
                }
                else if (isNextChild && next == null)
                {
                    next = child;
                    if(next.type == NodeType.Empty)
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
        public bool EvaluateFlags(DomTree scene)
        {
            if (scene.GlobalFlags.ContainsKey(RequiredFlags))
            {
                return scene.GlobalFlags[RequiredFlags];
            }
            else if (scene.LocalFlags.ContainsKey(RequiredFlags))
            {
                return scene.LocalFlags[RequiredFlags];
            }
            else if(RequiredFlags == string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the possible branches for this node
        /// </summary>
        /// <param name="next"></param>
        public void SetChildren(DomNode[] next)
        {
            if(next.Length == 1 && next[0].type == NodeType.Empty)
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
             * I'll need to refactor RequiredFlags, and FlagSets at some point
             */
            bool nodesAreEqual = this.RequiredFlags.Equals(node.RequiredFlags) &&
                this.Children.Length == node.Children.Length &&
                this.character.Equals(node.character) &&
                this.flagSets.Equals(node.flagSets);

            return nodesAreEqual;
        }
    }
}
