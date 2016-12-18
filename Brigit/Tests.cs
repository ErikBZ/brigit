using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit.Parser;

// TODO add some more tests

namespace Brigit
{
    class Tester
    {
        /// <summary>
        /// Tests good sorted listed
        /// </summary>
        public static void TestGoodSortedList()
        {
            GoodSortedList test = new GoodSortedList();

            test.Add("hi", 0);
            test.Add("how", 1);
            test.Add("hello", 4);
            test.Add("asdfasdf", 3);
            test.Add("somethign", 1);
            test.Add("else", 2);
            test.Add("hi", 5);

            for (int i = 0; i < test.Count; i++)
            {
                ArrayList t = test.GetListAtDepth(i);
                for (int j = 0; j < t.Count; j++)
                {
                    if (t[j] is string)
                    {
                        Console.Write((string)t[j]);
                    }
                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        public void SomeTest()
        {
            string path = @"..\..\scripts\script_test_2.txt";
            string[] toParse = null;
            if (File.Exists(path))
            {
                toParse = File.ReadAllLines(path);
            }

            TomeParser p = new TomeParser(toParse);
            DomTree writeTree = p.ParseCharacterDialog();
            DomAdmin.WriteDomTree(writeTree);
            DomTree domTree = DomAdmin.ReadDomTree(@"..\..\doms\test.ctom");

            Console.WriteLine(domTree);
            Console.ReadLine();
        }

        public void RequireArgumentsDictTest()
        {
            string path = @"..\..\scripts\comment_test.tome";
            string[] toParse = null;
            if (File.Exists(path))
            {
                toParse = File.ReadAllLines(path);
            }

            TomeParser p = new TomeParser(toParse);
            System.Collections.Generic.Dictionary<string, string[]> arguments = new System.Collections.Generic.Dictionary<string, string[]>();
            arguments.Add("ids", new string[] { "hello", "hi" });
            arguments.Add("def", new string[] { "hello", "hi" });
            arguments.Add("char", new string[] { "hello", "hi" });
            try
            {
                //if (p.RequireArguments(arguments, "derp"))
                //{
                //    Console.WriteLine("Hey it worked!");
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            Console.ReadLine();
        }
    }

}
