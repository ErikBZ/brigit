using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit;
using Brigit.IO;
using Brigit.Parser;
using System.IO;

namespace ExampleBrigit
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] text = BrigitLoader.OpenFile("../../Examples/Example.txt");
			text = ComomentRemover.RemoveComments(text);

			File.WriteAllLines("../../Examples/removedComments.txt", text);
		}
	}
}
