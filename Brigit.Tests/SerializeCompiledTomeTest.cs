using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.IO;
using Brigit;
using Brigit.Structure.Exchange;
using NUnit.Framework;
using System.Runtime.Serialization;

namespace Brigit.Test
{
	[TestFixture]
	class SerializeCompiledTomeTest
	{
		[Test]
		public void Serialize_Choice()
		{
			Choice ch = new Choice("This is a choice?", 1);
			TomeReader.SaveChoiceToFile("", ch);
		}

		[Test]
		public void Serialize_TomeTest1()
		{
			var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_1.txt"));
			TomeReader.SaveTomeFile(Path.Combine(Config.TomePath, "Compiled.tome"), conv);
		}

		//[Test]
		//public void Serialize_TomeTest2()
		//{

		//}
	}
}
