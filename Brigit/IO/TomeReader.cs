using System;
using System.IO;

namespace Brigit.IO
{
    public static class TomeReader
    {
        public static string[] ReadTextFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new Exception("File path does not exists");
            }

            string[] stream = File.ReadAllLines(filepath);
            return stream;
        }
    }
}
