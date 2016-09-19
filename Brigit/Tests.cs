using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /*
        /// <summary>
        /// Testing the eater class from Parser
        /// </summary>
        public static void TestEaterClass()
        {
            string path = @".\scripts\script_test.txt";
            string toParse = string.Empty;
            if (File.Exists(path))
            {
                toParse = File.ReadAllText(path);
            }

                Queue<string> que = new Queue<string>();
            Eater muncher = new Eater(toParse);

            while (!muncher.Complete())
            {
                if (Char.IsLetter(muncher.SniffChar()))
                {
                    que.Enqueue(muncher.SpitUpAlpha());
                }
                else
                {
                    // Should eat chars that are not letters
                    muncher.EatWhile(
                        delegate (char c)
                        { return !Char.IsLetter(c); });
                }
            }
            while (que.Count != 0)
            {
                Console.WriteLine(que.Dequeue());
            }

            Console.ReadLine();
        }

        public static void ParserParseSet()
        {
            // gettings the script test
            string path = @".\scripts\script_test.txt";
            string toParse = string.Empty;
            if (File.Exists(path))
            {
                toParse = File.ReadAllText(path);
            }

            Parser thing = new Parser();
            thing.muncher = new Eater(toParse);
            string[] people = thing.ParseSetOfStrings();
            for (int i = 0; i < people.Length; i++)
            {
                Console.WriteLine(people[i]);
            }

            Console.Read();
        }
        */
    }
}
