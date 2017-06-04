using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit.Structure
{
    // backgrounds, and flags are required and used

    [Serializable, DataContract]
    public class DomTree
    {
        /// <summary>
        /// The DomTree's name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The beginning of the script
        /// </summary>
        [DataMember]
        public DomNode Head { get; set; }

        /// <summary>
        /// Keeps track of the end of the list
        /// </summary>
        [DataMember]
        public DomNode[] Tail { get; set; }

        /// <summary>
        /// Trees can have multiple inner trees but
        /// only one outer tree
        /// </summary>
        [DataMember]
        public TreeType Type { get; set; }

        // I may turn these into Lists since it'll be up to the runtime to determine what they map to
        /// <summary>
        /// The list of characters that will be in this Scene
        /// Used for asset loading
        /// </summary>
        [DataMember]
        List<string> chars;

        /// <summary>
        /// A Dictionary of all the Backgrounds possible
        /// Used for asset loading
        /// </summary>
        [DataMember]
        List<string> backgrounds;

        /// <summary>
        /// Will load in flags that are used through out the story
        /// </summary>
        public Dictionary<string, bool> GlobalFlags { get; set; }
        /// <summary>
        /// Used only in a single tree and are instantiated false to begin with
        /// </summary>
        public Dictionary<string, bool> LocalFlags { get; set; }
        
        public List<string> Characaters
        {
            get { return chars; }
            set { chars = value; }
        }

        public List<string> Background
        {
            get { return backgrounds; }
            set { backgrounds = value; }
        }

        public static DomTree CreateEmptyDomTree()
        {
            DomTree tree = new DomTree();
            tree.Type = TreeType.Inner;
            tree.Head = new DomNode();
            tree.Head.Type = NodeType.Empty;
            tree.Tail = new DomNode[] { tree.Head };
            return tree;
        }

        public DomTree()
        {
            Head = null;
            Name = string.Empty;
            chars = new List<string>();
            backgrounds = new List<string>();
            LocalFlags = new Dictionary<string, bool>();
            GlobalFlags = new Dictionary<string, bool>();
        }

        public DomTree(List<string> cArray)
        {
            Head = null;
            chars = cArray;
        }

        // Actual useful functions are here
        // I don't know why I didn't have tree methods to help with branching
        // until just now

        /// <summary>
        /// Adds ALL nodes that were passed to the method to all
        /// Tail nodes at the end of the tree
        /// </summary>
        /// <param name="nodes"></param>
        public void Add(params DomNode[] nodes)
        {
            if(Head == null && nodes.Length == 1)
            {
                Head = nodes[0];
            }
            else
            {
                // there wasn't a Head before
                if(Head == null)
                {
                    Head = new DomNode();
                    Head.Type = NodeType.Empty;
                    Tail = new DomNode[] { Head };
                }
                // now that we have either a "empty node" or
                // actual nodes in Tail
                foreach (DomNode t in Tail)
                {
                    t.SetChildren(nodes);
                }
            }
            Tail = nodes;
        }

        public void Add(DomTree tree)
        {
            // add the Head node
            this.Add(tree.Head);
            this.Tail = tree.Tail;

            // adding characters that are in tree but not
            // in this.tree
            foreach(string ch in tree.Characaters)
            {
                if(!this.Characaters.Contains(ch))
                {
                    this.Characaters.Add(ch);
                }
            }
            // adding bakgrounds from tree that are not
            // already in this.tree
            foreach(string bq in tree.Background)
            {
                if(!this.Background.Contains(bq))
                {
                    this.Background.Add(bq);
                }
            }
            // tree has now been added to this tree!
            // wish i could null and free tree
        }

        public void Add(params DomTree[] trees)
        {
            DomTree connectedTrees = ConnectTrees(trees);
            this.Add(connectedTrees);
            // all those treees have now been added to the tree!
        }

        /// <summary>
        /// Connects trees by giving them the same Head, an empty Head
        /// and setting the "Tail" to the tails of both trees
        /// </summary>
        /// <param name="trees"></param>
        public static DomTree ConnectTrees(params DomTree[] trees)
        {
            DomTree newTree = new DomTree();
            newTree.Type = TreeType.Inner;
            // I don't set Tail here so maybe that's why there's an error later on
            //newTree.Head = new DomNode();
            //newTree.Head.Type = NodeType.Empty;

            List<DomNode> nodeHeads = new List<DomNode>();
            List<DomNode> nodeTails = new List<DomNode>();

            // finds the Heads of the trees
            // and the tails
            foreach (DomTree t in trees)
            {
                nodeHeads.Add(t.Head);
                foreach (DomNode tl in t.Tail)
                {
                    nodeTails.Add(tl);
                }
            }

            // new tree Head is empty.
            newTree.Add(nodeHeads.ToArray());
            newTree.Tail = nodeTails.ToArray();
            return newTree;
        }

        /// <summary>
        /// Sets the Dictionary of characters for this tree
        /// </summary>
        /// <param name="chars"></param>
        [Obsolete("Decepricated can be removed at some point")]
        public void SetCharacterDict(List<string> chars)
        {
            this.chars = chars;
        }

        /// <summary>
        /// do not use
        /// </summary>
        /// <param name="charNames"></param>
        // TODO right now this is just a place holder
        // isn't needed anymore
        public void SetCharacterDict(string[] charNames)
        {
            for(int i=0;i<charNames.Length;i++)
            {
                chars.Add(charNames[i]);
            }
        }

        public void SetBackgrounds(string[] bckNames)
        {
            for(int i=0;i<bckNames.Length;i++)
            {
                backgrounds.Add(bckNames[i]);
            }
        }

        /// <summary>
        /// Super costly, returns a formatted version of the Dom Tree
        /// </summary>
        /// <returns>A pretty picture</returns>
        public override string ToString()
        {
            return this.Name;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is DomTree))
            {
                return false;
            }
            else
            {
                return TraverseTrees((DomTree)obj);
            }
        }

        // Takes the head of the tree and adds it to a queue,
        // keeps check to see if both heads are equal, then adds it's children to the queue,
        // afterwards it checks those nodes, and keeps adding in the children to the queue until
        // there is nothing left int he queue
        public bool TraverseTrees(DomTree tree)
        {
            // assume they are true for now to test the head
            bool treesAreEqual = true;
            Queue<DomNode> thisQueue = new Queue<DomNode>();
            Queue<DomNode> otherQueue = new Queue<DomNode>();

            thisQueue.Enqueue(this.Head);
            otherQueue.Enqueue(tree.Head);

            // Keep going until you have found a node that is not equal, or
            // until there are no more nodes to check. Once the queues are done
            // trees are equal will have the correct values
            while(thisQueue.Count != 0 && otherQueue.Count != 0 && treesAreEqual)
            {
                DomNode thisNode = thisQueue.Dequeue();
                DomNode otherNode = otherQueue.Dequeue();

                // usable nodes should either be choices are dialogs
                // they should not be DomNodes
                if (thisNode is Choice)
                {
                    treesAreEqual = ((Choice)thisNode).Equals(otherNode);
                }
                else if (thisNode is Dialog)
                {
                    treesAreEqual = ((Dialog)thisNode).Equals(otherNode);
                }
                else
                {
                    // this is not supposed to happen
                    treesAreEqual = false;
                }

                if(treesAreEqual)
                {
                    foreach(DomNode ch in thisNode.Children)
                    {
                        thisQueue.Enqueue(ch);
                    }

                    foreach(DomNode ch in otherNode.Children)
                    {
                        otherQueue.Enqueue(ch);
                    }
                }
            }

            return treesAreEqual;
        }

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

    }
}
