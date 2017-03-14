﻿using System;
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
            string path = @"..\..\scripts\syntax_test_2.tome";
            string[] toParse = null;
            if (File.Exists(path))
            {
                toParse = File.ReadAllLines(path);
            }

            TomeParser tp = new TomeParser(toParse);
            DomTree thing = tp.Parse();
            Runtime.BrigitRuntime.Run(thing);
            Console.ReadLine();
        }
    }
}
