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
                throw new FileNotFoundException(String.Format("File path does not exists: {0}", filepath));
            }

            string[] stream = File.ReadAllLines(filepath);
            return stream;
        }
    }
}
