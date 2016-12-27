using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

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
    [Serializable]
    public enum NodeType { Start, End, Dual, Object, Empty };
    /*
     * If a tree is an inner type tree then it only
     * needs to have tails and head correctly tracked
     */
    [Serializable]
    public enum TreeType { Outer, Inner };

    /// <summary>
    /// Static class for loading and writing
    /// DomTree's and lists of DomTrees maybe even Characters
    /// </summary>
    // this can probably be deleted soon
    public static class DomAdmin
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

    // TODO add a "scene" class that keeps track of what characters,
    // backgrounds, and flags are required and used

    [Serializable, DataContract]
    public class DomTree
    {
        /// <summary>
        /// The DomTree's name
        /// </summary>
        [DataMember]
        string name;

        /// <summary>
        /// The beginning of the script
        /// </summary>
        [DataMember]
        DomNode head;

        /// <summary>
        /// Keeps track of the end of the list
        /// </summary>
        [DataMember]
        DomNode[] tail;

        /// <summary>
        /// Trees can have multiple inner trees but
        /// only one outer tree
        /// </summary>
        [DataMember]
        TreeType type;

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

        public TreeType Type
        {
            get { return type; }
        }


        public DomTree()
        {
            head = null;
            chars = new List<string>();
            backgrounds = new List<string>();
            LocalFlags = new Dictionary<string, bool>();
            GlobalFlags = new Dictionary<string, bool>();
        }

        public DomTree(List<string> cArray)
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
            if(head == null && nodes.Length == 1)
            {
                head = nodes[0];
            }
            else
            {
                // there wasn't a head before
                if(head == null)
                {
                    head = new DomNode();
                    head.Type = NodeType.Empty;
                    tail = new DomNode[] { head };
                }
                // now that we have either a "empty node" or
                // actual nodes in tail
                foreach (DomNode t in tail)
                {
                    t.SetChildren(nodes);
                }
            }
            tail = nodes;
        }

        public void Add(DomTree tree)
        {
            // add the head node
            this.Add(tree.head);
            this.tail = tree.tail;

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
        /// Connects trees by giving them the same Head, an empty head
        /// and setting the "tail" to the tails of both trees
        /// </summary>
        /// <param name="trees"></param>
        public static DomTree ConnectTrees(params DomTree[] trees)
        {
            DomTree newTree = new DomTree();
            newTree.type = TreeType.Inner;
            // I don't set tail here so maybe that's why there's an error later on
            //newTree.head = new DomNode();
            //newTree.head.Type = NodeType.Empty;

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
            if (obj is Choice)
                s = ((Choice)obj).ToString();
            else if (obj is Dialog)
                s = ((Dialog)obj).ToString();

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
            Children = null;
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
        public virtual DomNode GetNext(DomTree scene)
        {
            if(Children != null)
            {
                return Children[0];
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Given the current context of the scene with flags, it returns
        /// a bool indicating if it's flag requirements are met
        /// </summary>
        /// <returns></returns>
        public bool EvaluateFlags(DomTree scene)
        {
            if(scene.GlobalFlags.ContainsKey(RequiredFlags))
            {
                return scene.GlobalFlags[RequiredFlags];
            }
            else if(scene.LocalFlags.ContainsKey(RequiredFlags))
            {
                return scene.LocalFlags[RequiredFlags];
            }
            else
            {
                return false;
            }
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
                // set this nodes Children to the Children of the empty node
                SetChildren(next[0].Children);
            }
            Children = next;
        }

        /// <summary>
        /// Get all the Children of this node
        /// </summary>
        /// <returns></returns>
        public DomNode[] GetChildren()
        {
            return Children;
        }
    }

    [Serializable]
    public class Dialog : DomNode
    {
        public string speechText;

        public string Text
        {
            get { return speechText; }
            set { speechText = value; }
        }


        // i'll add the other ones later
        public Dialog():
            base()
        {
            this.speechText = string.Empty;
        }

        public override string ToString()
        {
            return this.speechText;
        }
    }

    [Serializable]
    public class Choice : DomNode
    {
        public string[] Choices { get; set; }
        public Dictionary<int, Dictionary<string, bool>> FlagsRasiedByChoices { get; set; }


        // once again i'll add the other ones later
        public Choice() :
            base()
        {
            this.Choices = new string[0];
            FlagsRasiedByChoices = new Dictionary<int, Dictionary<string, bool>>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for(int i=0;i<Choices.Length;i++)
            {
                string c = Choices[i];
                sb.Append(i);
                sb.Append(c);
                sb.Append('\n');
            }

            return sb.ToString();
        }

        public override DomNode GetNext(DomTree scene)
        {
            foreach(DomNode child in Children)
            {
                if(child.EvaluateFlags(scene))
                {
                    return child;
                }
            }
            // really throw an excpetion here if none of the nodes meet the requirements
            return Children[0];
        }
        /// <summary>
        /// Sets the global and local variables and 
        /// </summary>
        /// <param name="ch"></param>
        // TODO keep working on this
        public void MakeChoice(string ch, DomTree scene)
        {
            int choice = -1;
            if(int.TryParse(ch, out choice))
            {
                Dictionary<string, bool> flags = FlagsRasiedByChoices[choice];
                foreach(KeyValuePair<string, bool> entry in flags)
                {
                    if(scene.GlobalFlags.ContainsKey(entry.Key))
                    {
                        scene.GlobalFlags[entry.Key] = entry.Value;
                    }
                    else if(scene.LocalFlags.ContainsKey(entry.Key))
                    {
                        scene.LocalFlags[entry.Key] = entry.Value;
                    }
                    else
                    {
                        throw new Exception("Flags set by this choice are not present in either Local or Global flags within the scene");
                    }
                }
            }
            else
            {
                // do something here?
            }
        }
    }

    /// <summary>
    /// contains info for the background that can be set
    /// </summary>
    // TODO remove this at some point
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
