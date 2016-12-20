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

namespace Brigit
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"..\..\scripts\syntax_test.tome";
            string[] toParse = null;
            if (File.Exists(path))
            {
                toParse = File.ReadAllLines(path);
            }

            DomTree thing = ReadTree(@"..\..\tree.xml");
            Runtime.BrigitRuntime.Run(thing);
            Console.ReadLine();
        }
    }
}
