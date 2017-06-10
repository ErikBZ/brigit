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
using Brigit.Runtime;

namespace Brigit
{
    class Program
    {
        static void Main(string[] args)
        {
			string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
			path += @"\..\BrigitUnitTest\";
			Console.WriteLine(path);


			string[] lines = ReadTomeFile(path + @"Tests\choice_test_1.tome");
			TomeParser.TomeParser parser = new TomeParser.TomeParser(lines);

            DomTree parsedTree = parser.Parse();
			BrigitRuntime.Run(parsedTree);
			
            Console.ReadLine();
        }
    }
}
