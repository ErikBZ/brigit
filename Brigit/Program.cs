using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace Brigit
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @".\scripts\script_test.txt";
            string toParse = string.Empty;
            if(File.Exists(path))
            {
                toParse = File.ReadAllText(path);
            }

            Queue<string> que = new Queue<string>();
            Eater muncher = new Eater(toParse);

            while (!muncher.Complete())
            {
                if(Char.IsLetter(muncher.SniffChar()))
                {
                    que.Enqueue(muncher.SpitUpAlpha());
                }
                else
                {
                    // Should eat chars that are not letters
                    muncher.EatWhile(
                        delegate (char c)
                        { return !Char.IsLetter(c);});
                }
            }
            while(que.Count != 0)
            {
                Console.WriteLine(que.Dequeue());
            }

            Console.ReadLine();
        }
    }
}
