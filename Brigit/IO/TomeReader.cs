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
			if(filepath[filepath.Length-1] == '/')
			{
				throw new ArgumentException("Filepath passed in must point to file and not a folder");
			}

			// makes sure that directory exists
			Directory.CreateDirectory(Directory.GetParent(filepath).FullName);

			DataContractSerializer dcs = new DataContractSerializer(typeof(Conversation));
			FileStream fs = new FileStream(filepath, FileMode.Create);
			dcs.WriteObject(fs, conv);
			fs.Close();
		}

		public static Conversation OpenTomeFile(string filepath)
		{
			if(filepath[filepath.Length-1] == '/')
			{
				throw new ArgumentException("Filepath passed in must point to file and not a folder");
			}

			DataContractSerializer dcs = new DataContractSerializer(typeof(Conversation));
			FileStream fs = new FileStream(filepath, FileMode.Open);

			Conversation conv = dcs.ReadObject(fs) as Conversation;
			fs.Close();

			return conv;
		}
    }
}
