using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brigit.Runtime
{
    class BrigitRuntime
    {
        public static void Run(DomTree tree)
        {
            Console.SetWindowSize(48, 16);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            Console.SetCursorPosition(0, 12);

            DomNode curr = tree.Head;
            while (curr != null)
            {
                if (curr is Dialog)
                {
                    PrintDialog((Dialog)curr);
                }
                else
                {
                    Console.WriteLine("I don't know how to deal with this yet");
                }
                Console.ReadLine();
                ClearSpeechArea();
                curr = curr.GetNext();
            }
        }
        public static void PrintDialog(Dialog node)
        {
            int currCursorPos = 5;
            Console.SetCursorPosition(0, currCursorPos-1);
            Console.Write(node.Character + ':');
            Console.SetCursorPosition(4, currCursorPos);
            string[] lines = Chunkify(node.speechText, 40).ToArray<string>();
            StringBuilder sb = new StringBuilder();
            for (int i=0;i<lines.Length;i++)
            {
                Console.Write(lines[i]);
                currCursorPos++;
                Console.SetCursorPosition(4, currCursorPos);
            }
            // set it at the bottom
            Console.SetCursorPosition(0, 14);
        }

        // in this case the speech area is lines 11 for the character name
        // and lines 12, 13, 14 for the actual speech itself
        private static void ClearSpeechArea()
        {
            for(int i=4; i<15;i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }

        private static IEnumerable<string> Chunkify(string str, int size)
        {
            for(int i=0; i<str.Length;i+=size)
            {
                yield return str.Substring(i, Math.Min(size, str.Length - i));
            }
        }
    }
}
