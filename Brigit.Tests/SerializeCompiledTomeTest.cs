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
using System.Diagnostics;

namespace Brigit.Test
{
	[TestFixture]
	class SerializeCompiledTomeTest
	{
		[Test]
		public void Serialize_TomeTest1()
		{
			Stopwatch watch = new Stopwatch();
			watch.Start();
			Conversation conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_1.txt"));
			string textFileAndCompile = watch.Elapsed.TotalSeconds.ToString();

			TomeReader.SaveTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest1.tome"), conv);

			watch.Restart();
			Conversation newConv = TomeReader.OpenTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest1.tome"));
			string deserialzedTome = watch.Elapsed.TotalSeconds.ToString();

			bool areEqual = conv.Equals(newConv);

			Assert.AreEqual(true, areEqual);
		}

		[Test]
		public void Serialize_TomeTest2()
		{
			Stopwatch watch = new Stopwatch();
			// set up
			watch.Start();
			var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_2.txt"));
			string compile = watch.Elapsed.TotalSeconds.ToString();

			TomeReader.SaveTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest2.tome"), conv);

			watch.Restart();
			var newConv = TomeReader.OpenTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest2.tome"));
			string deserialize = watch.Elapsed.TotalSeconds.ToString();

			var areEqual = conv.Equals(newConv);

			// assert
			Assert.AreEqual(true, areEqual);
		}

		[Test]
		public void Serialize_And_Run_TomeTest2()
		{
			// set up
			var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_2.txt"));
			TomeReader.SaveTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest2.tome"), conv);
			var newConv = TomeReader.OpenTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest2.tome"));

			// act
            int[] choices = new int[] { 0 };
            string result = BrigitGraphTest.TraverseGraph(newConv, choices);
            string expected = "Yulia: What the fuck is this\n" +
                              "Yulia: What are you doing?\n" +
                              "0: Nothing\n" +
                              "Yulia: You're lying\n" +
                              "Diego: Yeah she is\n" +
                              "Diego: There's a lot of yelling going on right now\n";

			// assert
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void Serialize_TomeTest3()
		{
			// arrange
			var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_3.txt"));
			TomeReader.SaveTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest3.tome"), conv);
			var newConv = TomeReader.OpenTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest3.tome"));

			var areEqual = conv.Equals(newConv);

            Assert.AreEqual(true, areEqual);
		}
		
		[Test]
		public void Serialize_And_Run_TomeTest3()
		{
			// arrange
			var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_3.txt"));
			TomeReader.SaveTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest3.tome"), conv);
			var newConv = TomeReader.OpenTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest3.tome"));

			// act
            int[] choices = new int[] { 2 };
            string result = BrigitGraphTest.TraverseGraph(newConv, choices);
            string expected = "Diana: I didn't want to be the one to forget\n" +
                              "Diego: I thought of everything I'd never regret\n" +
                              "2: I don't believe in him - his lips on the ground\n" +
                              "Diana: But no one gives us time anymore\n" +
                              "Diego: Will you be my light?\n";

			// assert
            Assert.AreEqual(expected, result);
		}

		[Test]
		public void Serialize_TomeTest4()
		{
			// assemble
			var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_4.txt"));
			TomeReader.SaveTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest4.tome"), conv);
			var newConv = TomeReader.OpenTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest4.tome"));

			var areEqual = conv.Equals(newConv);

			// assert
			Assert.AreEqual(true, areEqual);
		}

		[Test]
		public void Serialize_And_RunTomeTest4()
		{
			// assemble
			var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_4.txt"));
			TomeReader.SaveTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest4.tome"), conv);
			var newConv = TomeReader.OpenTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest4.tome"));
            int[] choices = new int[] { 1 };

            // action
            string result = BrigitGraphTest.TraverseGraph(newConv, choices);
            string expected = "Diego: Hey what's happening\n" +
                            "1: This sets two to true\n" +
                            "Person: Hello\n" +
                            "Person: Blah\n";

            // assert
            Assert.AreEqual(expected, result);
		}

		[Test]
		public void Serialize_TomeTest5()
		{
			var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_5.txt"));
			TomeReader.SaveTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest5.tome"), conv);
			var newConv = TomeReader.OpenTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest5.tome"));

			var areEqual = conv.Equals(newConv);

			Assert.AreEqual(true, areEqual);
		}

		[Test]
		public void Serialize_And_Run_TomeTest5()
		{
			var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_5.txt"));
			TomeReader.SaveTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest5.tome"), conv);
			var newConv = TomeReader.OpenTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest5.tome"));

            int[] choices = new int[] { 0 };

            //action
            string result = BrigitGraphTest.TraverseGraph(newConv, choices);
            string expected = "0: choice 1\n" +
                              "Person: hello\n" +
                              "Person: how\n" +
                              "Person: are you\n" +
                              "Other: Another one\n" +
                              "Other: done\n" +
                              "More: Blah\n" +
                              "Last: Hey\n";

            Assert.AreEqual(expected, result);
		}

		[Test]
		public void Serialize_TomeTest6()
		{
			var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_6.txt"));
			TomeReader.SaveTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest6.tome"), conv);
			var newConv = TomeReader.OpenTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest6.tome"));

			var areEqual = conv.Equals(newConv);

			Assert.AreEqual(true, areEqual);
		}

		[Test]
		public void Serialize_And_Run_TomeTest6()
		{
			var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_6.txt"));
			TomeReader.SaveTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest6.tome"), conv);
			var newConv = TomeReader.OpenTomeFile(Path.Combine(Config.TomePath, @"Tomes\TomeTest6.tome"));

            int[] choices = new int[] { 1 };

            //act
            string result = BrigitGraphTest.TraverseGraph(newConv, choices);
            string expected = "Spongebob: I'm ready\n" +
                              "1: Path 2\n" +
                              "Squidward: Arghhh\n" +
                              "Patrick: SPONGEBOB\n" +
                              "Sandy: Karate chop\n";

            //assert
            Assert.AreEqual(expected, result);
		}
	}
}
