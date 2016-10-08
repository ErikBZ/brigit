using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BrigitVisualizer
{
    /*
     * EndNode may not be necessary since if a set has
     * a startNode then the the Node after the last MUST
     * be an EndNode and can be assumed, denoting this node
     * just makes it easier to understand in may opinion
     */
    public enum NodeType { StartNode, EndNode, ObjNode };

    /*
     * These nodes are temporary. Just trying to test
     * my algorithmn and then i'll try implementing it with TOME nodes
     *
     */

    public class Node
    {
        string someData;
        NodeType type;

        public NodeType Type
        {
            get { return type; }
        }
   
        // Constructors
        public Node(NodeType type)
        {
            this.type = type;
        }

        public Node(NodeType type, string data)
        {
            this.type = type;
            someData = data;
        }

        public string Data
        {
            get { return someData; }
            set { someData = value; }
        }
    }

    /// <summary>
    /// A set of BranchSets or nodes
    /// </summary>
    public class StraightSet
    {
        int width;
        int center;
        // needs to be able to add all objects
        // these objects will only be BrigitSets
        // and Nodes
        ArrayList list;

        // Constructors
        public StraightSet()
        {
            width = 0;
            center = 0;
            list = new ArrayList();
        }

        // Properties
        /// <summary>
        /// The width of the widest branch in the set
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        public int Count
        {
            get { return list.Count; }
        }

        public int Center
        {
            get { return center; }
            set { center = value; }
        }

        public ArrayList Set
        {
            get { return list; }
        }

        /// <summary>
        /// Adds an obj to the list inside brigit set
        /// </summary>
        /// <param name="obj"></param>
        public void AddToSet(object obj)
        {
            if(obj is BranchSet)
            {
                if(width < ((BranchSet)obj).Width)
                {
                    width = ((BranchSet)obj).Width;
                }
            }

            list.Add(obj);
        }

        /// <summary>
        /// Returns the obj found at index x
        /// </summary>
        /// <param name="x"></param>
        
        //TODO chage this to some abstract Node later
        public Object GetObjAt(int x)
        {
            return list[x];
        }
    }

    /// <summary>
    /// A set of straight sets
    /// </summary>
    public class BranchSet
    {
        // it is the sum of all the widths of the child sets
        // of this set minus the number of sets
        // Sigma(list) - list.length
        int widthSum;
        ArrayList list;

        // Constructors
        public BranchSet()
        {
            widthSum = 0;
            list = new ArrayList();
        }

        // Properties 
        public int Width
        {
            get { return widthSum - list.Count; }
            set { widthSum = value; }
        }

        public int Count
        {
            get { return list.Count; }
        }

        public ArrayList List
        {
            get { return list; }
        }

        // Methods
        /// <summary>
        /// Add straight set to the list
        /// </summary>
        /// <param name="obj"></param>
        public void AddToSet(StraightSet obj)
        {
            widthSum += obj.Width;
            list.Add(obj);
        }

        public Object GetObjAt(int x)
        {
            return list[x];
        }
    }

}
