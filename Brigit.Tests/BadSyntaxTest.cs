using System;
using System.IO;
using NUnit.Framework;

namespace Brigit.Test
{
	[TestFixture]
	public class BadSyntaxTest
	{
		[Test]
		public void Parse_and_Fail_TomeTest_7()
		{
			string file = Path.Combine(Config.TomePath, "TomeTest_7.txt");
			Assert.Throws<Exception>(() => Brigit.ConversationLoader.CreateConversation(file));
		}
	}
}