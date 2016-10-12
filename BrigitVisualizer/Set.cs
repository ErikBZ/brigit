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
     * I'm probably gonna put this somewhere else and I have
     * to create a good namespace structure
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

        // functions
        public override string ToString()
        {
            return $"Node: {someData}";
        }
    }

    /// <summary>
    /// A set of BranchSets or nodes
    /// </summary>
    public class StraightSet
    {
        // 2n+1, a straight set with only nodes is going
        // to be 3 at minimum
        int width;
        int center;

        // toggles if the StraightSet has branches
        bool containsBranches = false;

        // needs to be able to add all objects
        // these objects will only be BrigitSets
        // and Nodes
        ArrayList list;

        // Constructors
        public StraightSet()
        {
            width = 3;
            center = -1;
            list = new ArrayList();
        }

        public StraightSet(params Object[] elements) : this()
        {
            for(int i=0;i<elements.Length;i++)
            {
                AddToSet(elements[i]);
            }
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

        // Functions

        /// <summary>
        /// Adds an obj to the list inside brigit set
        /// </summary>
        /// <param name="obj"></param>
        public void AddToSet(object obj)
        {
            if(obj is BranchSet)
            {
                containsBranches = true;
                if(width < ((BranchSet)obj).Width)
                {
                    width = ((BranchSet)obj).Width;
                }
                list.Add(obj);
            }
            else if(obj is Node)
            {
                list.Add(obj);
            }
            else
            {
                Console.WriteLine("Only Node and BranchSet objects may be added to this set");
            }
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

        // Caculcates all the centers for each of the StraightSets
        // base function, assume s is the highest set
        public static void CenterSet(StraightSet s)
        {
            // the counting starts at 0 so s.wdith/2 works just fine
            s.center = s.width/ 2;
            s.CenterChildSets();
        }
        
        // s is the parent set of the child sets
        // we are assuming that the info for s has already been calculated
        private void CenterChildSets()
        {
            for(int i=0;i<this.Count;i++)
            {
                if(this.GetObjAt(i) is BranchSet)
                {
                    BranchSet bSet = (BranchSet)this.GetObjAt(i);

                    // the width of the node left of the current node
                    int oldWidth = 0;
                    int oldCenter = 0;
                    // looping through the branch set
                    // and calculating the centers for each of the
                    // straight sets in the branch set and there child sets
                    for(int j=0; j<bSet.Count; j++)
                    {
                        // the left most node
                        int leftMost = this.center - this.width / 2;
                        StraightSet strChildSet = bSet.GetObjAt(j);
                        strChildSet.center = oldWidth / 2 + leftMost + strChildSet.width / 2 + 0;
                        // now that the center for this has been calculated, we can calculate it's
                        // child sets if it has one
                        if(strChildSet.containsBranches)
                        {
                            strChildSet.CenterChildSets();
                        }
                        oldWidth = strChildSet.width;
                        oldCenter = strChildSet.center;
                         
                    }
                }
            }
        }

        // overring Object functions
        public override string ToString()
        {
            return "Straigt Set";
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

        public BranchSet(params StraightSet[] elements):
            this()
        {
            for(int i=0; i<elements.Length;i++)
            {
                AddToSet(elements[i]);
            }
        }

        // Properties 
        public int Width
        {
            get { return widthSum - (list.Count - 1); }
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
        public void AddToSet(Object obj)
        {
            if(!(obj is StraightSet))
            {
                Console.WriteLine("Cannot add non StraightSet object to list");
            }
            else
            {
                widthSum += ((StraightSet)obj).Width;
                list.Add(obj);
            }
        }

        public StraightSet GetObjAt(int x)
        {
            return (StraightSet)list[x];
        }

        // overriding Object functions
        public override string ToString()
        {
            return "Branch Set :";
        }
    }

}
