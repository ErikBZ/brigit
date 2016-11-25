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
            string path = @"..\..\scripts\comment_test.tome";
            string[] toParse = null;
            if (File.Exists(path))
            {
                toParse = File.ReadAllLines(path);
            }

            BrigitParser p = new BrigitParser(toParse);
            Dictionary<string, string[]> arguments = new Dictionary<string, string[]>();
            arguments.Add("ids", new string[] { "hello", "hi" });
            arguments.Add("def", new string[] { "hello", "hi" });
            arguments.Add("char", new string[] { "hello", "hi" });
            try
            {
                if (p.RequireArguments(arguments, "derp"))
                {
                    Console.WriteLine("Hey it worked!");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            Console.ReadLine();
        }
    }
}
