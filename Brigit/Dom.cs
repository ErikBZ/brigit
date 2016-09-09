using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit
{

    public class DomTree
    {
        /// <summary>
        /// The beginning of the script
        /// </summary>
        DomNode head;

        public DomTree()
        {
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
                    for(int j=0;j<list.Count;j++)
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

        private GoodSortedList GetSortedDomNodes()
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
                AddDomNode(l, d, depth + 1);
            }
        }

        private ArrayList[] ExpandArray(ArrayList[] array)
        {
            ArrayList[] longerArray = new ArrayList[array.Length * 2];
            array.CopyTo(longerArray, 0);
            return longerArray;
        }
    }

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

        public DomNode[] GetChildren()
        {
            return children;
        }
    }

    public class Response : DomNode
    {
        string response;

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

    public class Reply : DomNode
    {
        string[] replies;

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
    public class Character
    {
        string name;
        string picLocation;

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
    }
}
