using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrigitVisualizer
{
    static class Tester
    {
        public static void TestGetFirstMethod()
        {
            StraightSet set = GetTestBaseTest();
            for(int i=0;i<set.Count;i++)
            {
                if(set.Set[i] is BranchSet)
                {
                    BranchSet bSet = (BranchSet)set.Set[i];
                    Node[] nodes = bSet.GetFirstElementsInSet();   
                }
            }
        }

        public static StraightSet GetTestBaseTest()
        {
            Node a = new Node(NodeType.ObjNode, "a");
            Node b1 = new Node(NodeType.ObjNode, "b1");
            Node b2 = new Node(NodeType.ObjNode, "b2");
            Node b3 = new Node(NodeType.ObjNode, "b3");
            Node c1 = new Node(NodeType.ObjNode, "c1");
            Node c2 = new Node(NodeType.ObjNode, "c2");
            Node c3 = new Node(NodeType.ObjNode, "c3");
            Node c4 = new Node(NodeType.ObjNode, "c4");
            Node d1 = new Node(NodeType.ObjNode, "d1");
            Node d2 = new Node(NodeType.ObjNode, "d2");
            Node d3 = new Node(NodeType.ObjNode, "d3");
            Node d4 = new Node(NodeType.ObjNode, "d4");
            Node f = new Node(NodeType.ObjNode, "f");

            StraightSet D4 = new StraightSet(d4);
            StraightSet D3 = new StraightSet(d3);
            BranchSet D3_4 = new BranchSet(D3, D4);
            StraightSet B3 = new StraightSet(b3, c4, D3_4);
            StraightSet C4 = new StraightSet(c4);
            StraightSet C3 = new StraightSet(c3);
            BranchSet C3_4 = new BranchSet(C3, C4);
            StraightSet B2 = new StraightSet(b2, C3_4, d2);
            StraightSet B1 = new StraightSet(b1, c1, d1);
            BranchSet B1_2_3 = new BranchSet(B1, B2, B3);
            StraightSet BaseSet = new StraightSet(a, B1_2_3, f);
            return BaseSet;
        }

        public static void SetRepresentationTest()
        {
            Node a = new Node(NodeType.ObjNode, "a");
            Node b1 = new Node(NodeType.ObjNode, "b1");
            Node b2 = new Node(NodeType.ObjNode, "b2");
            Node b3 = new Node(NodeType.ObjNode, "b3");
            Node c1 = new Node(NodeType.ObjNode, "c1");
            Node c2 = new Node(NodeType.ObjNode, "c2");
            Node c3 = new Node(NodeType.ObjNode, "c3");
            Node c4 = new Node(NodeType.ObjNode, "c4");
            Node d1 = new Node(NodeType.ObjNode, "d1");
            Node d2 = new Node(NodeType.ObjNode, "d2");
            Node d3 = new Node(NodeType.ObjNode, "d3");
            Node d4 = new Node(NodeType.ObjNode, "d4");
            Node f = new Node(NodeType.ObjNode, "f");

            StraightSet D4 = new StraightSet(d4);
            StraightSet D3 = new StraightSet(d3);
            BranchSet D3_4 = new BranchSet(D3, D4);
            StraightSet B3 = new StraightSet(b3, c4, D3_4);
            StraightSet C4 = new StraightSet(c4);
            StraightSet C3 = new StraightSet(c3);
            BranchSet C3_4 = new BranchSet(C3, C4);
            StraightSet B2 = new StraightSet(b2, C3_4, d2);
            StraightSet B1 = new StraightSet(b1, c1, d1);
            BranchSet B1_2_3 = new BranchSet(B1, B2, B3);
            StraightSet BaseSet = new StraightSet(a, B1_2_3, f);

            StringBuilder str = new StringBuilder();
            StraightSet.CenterSet(BaseSet);
            str.Append(BaseSet.Width);

            string output = str.ToString();

            Console.Write("hello");
            Console.WriteLine("Ho yah doing");
        }
    }
}
