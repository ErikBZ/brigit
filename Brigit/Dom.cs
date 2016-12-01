﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit
{
    /*
     * End may not be necessary since if a set has
     * a startNode then the the Node after the last MUST
     * be an End and can be assumed, denoting this node
     * just makes it easier to understand in may opinion
     * I'm probably gonna put this somewhere else and I have
     * to create a good namespace structure
     */
    public enum NodeType { Start, End, Dual, Object, Empty };
    /*
     * If a tree is an inner type tree then it only
     * needs to have tails and head correctly tracked
     */
    public enum TreeType { Outer, Inner };

    /// <summary>
    /// Static class for loading and writing
    /// DomTree's and lists of DomTrees maybe even Characters
    /// </summary>
    public class DomAdmin
    {
        /// <summary>
        /// Writes a single tree into a binary file
        /// </summary>
        /// <param name="tree"></param>
        public static void WriteDomTree(DomTree tree)
        {
            IFormatter iformatter = new BinaryFormatter();
            string folderPath = @"..\..\doms\" + tree.Name;
            Console.WriteLine(folderPath);
            Stream stream = new FileStream(folderPath, FileMode.Create,
                FileAccess.Write, FileShare.None);
            iformatter.Serialize(stream, tree);
            stream.Close();
        }

        /// <summary>
        /// Loads a DOM Tree from the disk
        /// </summary>
        /// <param name="path"></param>
        public static DomTree ReadDomTree(string path)
        {
            IFormatter iformatter = new BinaryFormatter();
            if(File.Exists(path))
            {
                Stream stream = new FileStream(path, FileMode.Open,
                    FileAccess.Read, FileShare.Read);
                DomTree tree = (DomTree)iformatter.Deserialize(stream);
                return tree;
            }
            return null;
        }

        public static DomTree ReadDomFromDialog()
        {
            // Create an instance of the file open dialog

            return null;
        }
    }

    [Serializable]
    public class DomTree
    {
        /// <summary>
        /// The DomTree's name
        /// </summary>
        string name;

        /// <summary>
        /// The beginning of the script
        /// </summary>
        DomNode head;

        /// <summary>
        /// Keeps track of the end of the list
        /// </summary>
        DomNode[] tail;

        /// <summary>
        /// Trees can have multiple inner trees but
        /// only one outer tree
        /// </summary>
        TreeType type;

        // I may turn these into Lists since it'll be up to the runtime to determine what they map to
        /// <summary>
        /// The list of characters that will be in this Scene
        /// </summary>
        Dictionary<string, Character> chars = new Dictionary<string, Character>();

        /// <summary>
        /// A dictionary of all the Backgrounds possible
        /// </summary>
        Dictionary<string, Background> backgrounds = new Dictionary<string, Background>();
        
        // properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public DomNode Head
        {
            get { return head; }
            set { head = value; }
        }

        public Dictionary<string, Character> Characaters
        {
            get { return chars; }
            set { chars = value; }
        }

        public Dictionary<string, Background> Background
        {
            get { return backgrounds; }
            set { backgrounds = value; }
        }

        public TreeType Type
        {
            get { return type; }
        }


        public DomTree()
        {
            head = null;
            chars = new Dictionary<string, Character>();
            backgrounds = new Dictionary<string, Background>();
        }

        public DomTree(Dictionary<string, Character>  cArray)
        {
            head = null;
            chars = cArray;
        }

        // Actual useful functions are here
        // I don't know why I didn't have tree methods to help with branching
        // until just now

        /// <summary>
        /// Adds ALL nodes that were passed to the method to all
        /// tail nodes at the end of the tree
        /// </summary>
        /// <param name="nodes"></param>
        public void Add(params DomNode[] nodes)
        {
            foreach(DomNode t in tail)
            {
                t.SetChildren(nodes);
            }
        }

        public void Add(DomTree tree)
        {
            // add the head node
            this.Add(tree.head);
            this.tail = tree.tail;
            // tree has now been added to this tree!
        }

        public void Add(params DomTree[] trees)
        {
            DomTree connectedTrees = ConnectTrees(trees);
            this.Add(connectedTrees);
            // all those treees have now been added to the tree!
        }

        /// <summary>
        /// Connects trees by giving them the same Head, an empty head
        /// and setting the "tail" to the tails of both trees
        /// </summary>
        /// <param name="trees"></param>
        public DomTree ConnectTrees(params DomTree[] trees)
        {
            DomTree newTree = new DomTree();
            newTree.type = TreeType.Inner;
            newTree.head = new DomNode();
            newTree.head.Type = NodeType.Empty;

            List<DomNode> nodeHeads = new List<DomNode>();
            List<DomNode> nodeTails = new List<DomNode>();

            // finds the heads of the trees
            // and the tails
            foreach (DomTree t in trees)
            {
                nodeHeads.Add(t.Head);
                foreach (DomNode tl in t.tail)
                {
                    nodeTails.Add(tl);
                }
            }

            // new tree head is empty.
            newTree.Add(nodeHeads.ToArray());
            newTree.tail = nodeTails.ToArray();
            return newTree;
        }

        /// <summary>
        /// Sets the dictionary of characters for this tree
        /// </summary>
        /// <param name="chars"></param>
        public void SetCharacterDict(Dictionary<string, Character> chars)
        {
            this.chars = chars;
        }

        /// <summary>
        /// Looks up the given character names and creates a dictionary using
        /// the names as a key
        /// </summary>
        /// <param name="charNames"></param>
        // TODO right now this is just a place holder
        public void SetCharacterDict(string[] charNames)
        {
            for(int i=0;i<charNames.Length;i++)
            {
                chars.Add(charNames[i], new Character(charNames[i]));
            }
        }

        public void SetBackgrounds(string[] bckNames)
        {
            for(int i=0;i<bckNames.Length;i++)
            {
                backgrounds.Add(bckNames[i], new Background(bckNames[i]));
            }
        }

        /// <summary>
        /// Super costly, returns a formatted version of the Dom Tree
        /// </summary>
        /// <returns>A pretty picture</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            GoodSortedList list = GetSortedDomNodes();

            for(int i=0;i<list.Count;i++)
            {
                ArrayList l = list.GetListAtDepth(i);
                if(l != null)
                {
                    for(int j=0;j<l.Count;j++)
                    {
                        sb.Append('(');
                        sb.Append(GetObjectType(l[j]));
                        sb.Append(')');
                    }
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }

        private string GetObjectType(Object obj)
        {
            string s = string.Empty;
            if (obj is Reply)
                s = ((Reply)obj).ToString();
            else if (obj is Response)
                s = ((Response)obj).ToString();

            return s;
        }

        public GoodSortedList GetSortedDomNodes()
        {
            GoodSortedList list = new GoodSortedList();
            AddDomNode(list, head, 0);
            return list;
        }

        private void AddDomNode(GoodSortedList l, DomNode d, int depth)
        {
            l.Add(d, depth);
            DomNode[] ch = d.GetChildren();
            for(int i = 0;i<ch.Length;i++)
            {
                AddDomNode(l, ch[i], depth + 1);
            }
        }

        private ArrayList[] ExpandArray(ArrayList[] array)
        {
            ArrayList[] longerArray = new ArrayList[array.Length * 2];
            array.CopyTo(longerArray, 0);
            return longerArray;
        }
    }

    [Serializable]
    public class DomNode
    {
        /// <summary>
        /// The possible leaves this Node has
        /// </summary>
        DomNode[] children;

        /// <summary>
        /// The flags required to get this leaf to appear, try avoiding situations where
        /// more than 1 leaf can be activated by an overlapping set of flags
        /// </summary>
        Dictionary<string, bool> flags;

        /// <summary>
        /// The flags that this Node will set
        /// </summary>
        Dictionary<string, bool> flagSets;

        /// <summary>
        /// The character that "owns" this node, IE the character who said this
        /// </summary>
        Character character;

        /// <summary>
        /// The background for the character. Will usually not change, so the first
        /// node can have the background set and all other nodes will inherit it
        /// </summary>
        Background background;

        NodeType type;

        // settings up the properties
        public DomNode[] Children
        {
            get { return children; }
            set { children = value; }
        }

        public Dictionary<string, bool> Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        public Dictionary<string, bool> FlagToggles
        {
            get { return flagSets; }
            set { flagSets = value; }
        }

        public Character Character
        {
            get { return character; }
            set { character = value; }
        }

        public Background Background
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
        public DomNode() :
            this(new DomNode[0])
        {
        }

        /// <summary>
        /// Creates a new Dom Node for a tree.
        /// </summary>
        /// <param name="children"></param>
        public DomNode(DomNode[] children):
            this(children, new Dictionary<string, bool>())
        {
        }
        
        public DomNode(DomNode[] children, Dictionary<string, bool> flags):
            this(children, flags, new Dictionary<string, bool>())
        {
        }

        public DomNode(DomNode[] children, Dictionary<string, bool> flags,
            Dictionary<string, bool> flagSets):
            this(children, flagSets, flagSets, null)
        {

        }

        public DomNode(DomNode[] children, Dictionary<string, bool> flags,
            Dictionary<string, bool> flagSets, Character character)
        {
            this.children = children;
            this.flags = flags;
            this.flagSets = flagSets;
            this.character = character;
        }

        public override string ToString()
        {
            return "Dom Node";
        }

        /// <summary>
        /// Set the possible branches for this node
        /// </summary>
        /// <param name="next"></param>
        public void SetChildren(DomNode[] next)
        {
            if(next.Length == 1 && next[0].type == NodeType.Empty)
            {
                // set this nodes children to the children of the empty node
                SetChildren(next[0].Children);
            }
            children = next;
        }

        /// <summary>
        /// Get all the children of this node
        /// </summary>
        /// <returns></returns>
        public DomNode[] GetChildren()
        {
            return children;
        }
    }

    [Serializable]
    public class Response : DomNode
    {
        public string response;

        public string Text
        {
            get { return response; }
            set { response = value; }
        }


        // i'll add the other ones later
        public Response():
            base()
        {
            this.response = string.Empty;
        }

        public Response(DomNode[] children, Dictionary<string, bool> flags,
            Dictionary<string, bool> flagSets, Character character, string response):
            base(children, flags, flagSets, character)
        {
            this.response = response;
        }

        public override string ToString()
        {
            return "Response";
        }
    }

    [Serializable]
    public class Reply : DomNode
    {
        string[] replies;

        // properties
        public string[] Replies
        {
            get { return replies; }
            set { replies = value; }
        }

        // once again i'll add the other ones later
        public Reply() :
            base()
        {
            this.replies = new string[0];
        }

        public Reply(DomNode[] children, Dictionary<string, bool> flags,
            Dictionary<string, bool> flagSets, Character character, string[] replies):
            base(children, flags, flagSets, character)
        {
            this.replies= replies;
        }

        public override string ToString()
        {
            return "Reply";
        }
    }

    /// <summary>
    /// Contains character info such as portrait location and well that's basicaly it I guess
    /// </summary>
    [Serializable]
    public class Character
    {
        string name;
        string picLocation;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string PictureLocal
        {
            get { return picLocation; }
            set { picLocation = value; }
        }


        /// <summary>
        /// Creates a new Character
        /// </summary>
        /// <param name="n">The characters name</param>
        /// <param name="p">The location where the portrait is located</param>
        public Character(string n, string p)
        {
            name = n;
            picLocation = p;
        }
        
        public Character(string n)
        {
            name = n;
            picLocation = string.Empty;
        }

        public override string ToString()
        {
            return name;
        }
    }

    /// <summary>
    /// contains info for the background that can be set
    /// </summary>
    // TODO add more shit to this
    [Serializable]
    public class Background
    {
        string name;

        // properties
        public string Name
        {
            get { return Name; }
            set { name = value; }
        }

        public Background()
        {
            name = string.Empty;
        }

        public Background(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
