using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace Brigit
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"..\..\scripts\script_test_2.txt";
            string toParse = string.Empty;
            if (File.Exists(path))
            {
                toParse = File.ReadAllText(path);
            }
            Parser p = new Parser();
            p.muncher = new Eater(toParse);
            DomTree tree = p.ParseBrigitText();
            Console.ReadLine();
        }
    }
}
