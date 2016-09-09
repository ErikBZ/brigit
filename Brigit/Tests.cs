using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brigit
{
    class Tester
    {
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
    }
}
