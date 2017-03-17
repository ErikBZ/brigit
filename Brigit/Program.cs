using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using static Brigit.IO.BrigitIO;
using Brigit.Parser;
using Brigit.Structure;

namespace Brigit
{
    class Program
    {
        static void Main(string[] args)
        {
            // this should be the same
            // arrange
            DomNode node1 = new DomNode();
            node1.Character = "char";
            node1.RequiredFlags = "flag1";
            node1.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node1.Children = new DomNode[3];
            // arraning node 2
            DomNode node2 = new DomNode();
            node2.Character = "char";
            node2.RequiredFlags = "flag1";
            node2.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node2.Children = new DomNode[3];

            // acting
            bool areNodesEqual = node1.Equals(node2);

            Console.WriteLine(areNodesEqual);
        }
    }
}
