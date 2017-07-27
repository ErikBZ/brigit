using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brigit;
using Brigit.Structure;

namespace Brigit.Interface
{
	public class BrigitFrontendInterface
	{
		public static void Run(Conversation convo)
		{
			while(!convo.Complete())
			{
				Renderable rend = convo.GetCurr();
				PrintRenderable(rend);

				int input = Console.Read();
				while(!convo.GoToNext(input))
				{
					Console.WriteLine("Please enter a number between the number of choices");
				}
			}
		}

		public static void PrintRenderable(Renderable r)
		{
			Console.WriteLine(r.CharacterName);
			Console.WriteLine();
			Console.WriteLine(r.Info);
		}
	}
}
