using System;
using System.IO;
using System.Runtime.Serialization;
using Brigit.Structure.Exchange;

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

		public static void SaveTomeFile(string filepath, Conversation conv)
		{
			DataContractSerializer dcs = new DataContractSerializer(typeof(Conversation));
			FileStream fs = new FileStream(filepath, FileMode.Create);
			dcs.WriteObject(fs, conv);
		}

		public static void SaveChoiceToFile(string filePath, Choice choice)
		{
			DataContractSerializer dcs = new DataContractSerializer(typeof(Choice));
			FileStream fs = new FileStream("E:/Users/zapat/Documents/brigit/some.tome", FileMode.Create);
			dcs.WriteObject(fs, choice);
		}
    }
}
