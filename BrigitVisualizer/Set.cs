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
     /// <summary>
     /// abstract class to inherit from
     /// </summary>
    public class Node
    {
        int cycle;
        NodeType type;
        Node[] children;

        public NodeType Type
        {
            get { return type; }
            set { type = value; }
        }

        public Node()
        {
            cycle = 0;
            type = NodeType.StartNode;
            children = new Node[0];
        }
        
        public void SetChildren(Node[] NewChildren)
        {
            children = NewChildren;
        }
    }

    public class ObjectNode: Node
    {
        string someData;

        // just some random ass data to use for
        // now. Could repalce this with a dom node later
        // Object domNode. like that
        public ObjectNode() : base()
        {
            Type = NodeType.ObjNode;
        }

        public string Data
        {
            get { return someData; }
            set { someData = value; }
        }
    }

    static class Tester
    {
        public static Node CreateRandomTree()
        {
            ObjectNode head = new ObjectNode();
            head.Data = "head";
            Node[] nodes = CreateChildren();
            head.SetChildren(nodes);

            return head;  
        }

        public static Node[] CreateChildren()
        {
            Random rnd = new Random();
            int rand = rnd.Next() % 5 + 1;
            Node[] nArray = new Node[rand];
            for(int i=0; i<rand; i++)
            {
                nArray[i] = new ObjectNode();
                ((ObjectNode)nArray[i]).Data = i.ToString();
            }
            return nArray;
        }
    }

    class BrigitSet
    {
        int width;
        int center;
        // needs to be able to add all objects
        // these objects will only be BrigitSets
        // and Nodes
        ArrayList list = new ArrayList();

        public int Width
        {
            get { return width; }
            set { width = value; }
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
            list.Add(obj);
        }
        public void GetObjAt(int x)
        {

        }
    }
}
