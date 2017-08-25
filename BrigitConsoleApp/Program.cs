using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Parser;
using Brigit.Parser.Stream;
using System.IO;

namespace BrigitConsoleApp
{
    class Program
    {
        static string RootDirectory = Directory.GetCurrentDirectory();

        static void Main(string[] args)
        {
            var tomeFile = args[0];
            TomeStream stream = GetStream(tomeFile);

            BrigitGraph bg = BrigitParser.ParseBrigitGraph(stream);
            Conversation conv = new Conversation(bg);

            int next = 0;

            conv.StartNewRun();

            while (!conv.Complete)
            {
                Info inf = conv.GetInfo();

                // printing info
                switch (inf.type)
                {
                    case Info.Type.Choice:
                        PrintChoice(inf.Text.ToArray());
                        break;
                    case Info.Type.Dialog:
                        PrintDialog(inf.Text[0], inf.Character);
                        break;
                }

                // getting the input from the player
                bool askAgain = false;
                do
                {
                    string input = Console.ReadLine();
                    bool goodParse = int.TryParse(input, out next);
                    if(goodParse && inf.type == Info.Type.Choice)
                    {
                        // setting to true here so I don't have have an else
                        askAgain = true;

                        if( next < inf.Text.Count && next > -1 )
                        {
                            // if the go to next returns false
                            // then something went wrong
                            if(!(conv.GoToNext(next)))
                            {
                                throw new Exception("The conversation could not step into the next section");
                            }
                            // everything is good, so don't ask the the player to input another number
                            askAgain = false;
                        }
                   }
                    else if(inf.type == Info.Type.Dialog)
                    {
                        conv.GoToNext();
                        askAgain = false;
                    }
                    else
                    {
                        Console.WriteLine("Please ask enter a valid input");
                    }
                } while (askAgain);
            }
        }

        public static void PrintDialog(string info, string ch="")
        {
            Console.WriteLine($"{ ch }");
            Console.WriteLine($"\t{info}");
        }

        public static void PrintChoice(string[] stuff)
        {
            for (int i = 0; i < stuff.Length; i++)
            {
                Console.WriteLine($"\t {i}: {stuff[i]}");
            }
        }

        public static TomeStream GetStream(string testFileName)
        {
            string[] tome = File.ReadAllLines($"{RootDirectory}\\{testFileName}");
            string[] tomeNoComments = ComomentRemover.RemoveComments(tome);
            TomeStream stream = new TomeStream(tomeNoComments);

            return stream;
        }
    }
}
