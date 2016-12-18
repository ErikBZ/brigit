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

            TomeParser tp = new TomeParser(toParse);
            tp.characters.Add("character");
            DomTree tree = tp.ParseCharacterDialog();

            DomNode node = tree.Head;
            WriteDomNode(@"..\..\node.xml", node);
            WriteTree(@"..\..\tree.xml", tree);
            DomTree thing = ReadTree(@"..\..\tree.xml");
            Console.ReadLine();
        }
    }
}
