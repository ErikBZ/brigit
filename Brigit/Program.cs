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
            string[] toParse = null;
            if (File.Exists(path))
            {
                toParse = File.ReadAllLines(path);
            }

            Eater muncher = new Eater(toParse);
            string s = muncher.SpitUpWhile(delegate (char c)
            {
                return c != 'y';
            });

            Console.WriteLine(s);
            Console.WriteLine();
            Console.WriteLine(muncher.GetRemainingString());
            Console.WriteLine(muncher.Position);
            Console.ReadLine();
        }
    }
}
