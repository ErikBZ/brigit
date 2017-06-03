using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using static Brigit.IO.BrigitIO;
using Brigit.TomeParser;
using Brigit.Structure;

namespace Brigit
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = ReadTomeFile(@"..\..\Tests\attribute_test_1.tome");
			TomeParser.TomeParser parser = new TomeParser.TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();
            Dialog node = new Dialog();
            node.Character = "Character1";
            node.Text = "Hello there";

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            bool test = constructedTree.Equals(parsedTree);

            Console.WriteLine(test);
            Console.ReadLine();
        }
    }
}
