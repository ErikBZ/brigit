using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Brigit.Parser;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Parser.Stream;
using System.IO;
using System.Collections.Generic;

namespace Brigit.Test
{
	[TestClass]
	public class ParserUnitTests
	{
		const string RootDirectory = @"..\..\Tests\";

		public TomeStream GetStream(string testFileName)
		{
			string[] tome = File.ReadAllLines(RootDirectory + testFileName);
			string[] tomeNoComments = ComomentRemover.RemoveComments(tome);
			TomeStream stream = new TomeStream(tomeNoComments);
			return stream;
		}

		[TestMethod]
		public void ParseDialogTestTome()
		{
			string character = "Character1";
			string text = "I'm going to say something";
			TomeStream stream = GetStream("DialogTestTome.txt");
			Node n = BrigitParser.ParseDialog(stream);
			bool passed = false;
			if (n.Data is Dialog)
			{
				Dialog data = n.Data as Dialog;
				passed = data.Character == character;
				if (data.Text.Count == 1)
				{
					passed &= data.Text[0].Text == text;
				}
				else
				{
					passed &= false;
				}
			}

			Assert.AreEqual(true, passed);
		}

		[TestMethod]
		public void ParseDialogWithMultipleSpeechTexts()
		{
			TomeStream stream = GetStream("MultipleSpeechText.txt");
			Node n = BrigitParser.ParseDialog(stream);

			var other = new Dialog()
			{
				Character = "Diego", };
			other.Text.Add(new SpeechText { Text = "Where\n" });
			other.Text.Add(new SpeechText { Text = "\tAre" });
			other.Text.Add(new SpeechText { Text = "The" });
			other.Text.Add(new SpeechText { Text = "White Women?" });

			Dialog data = n.Data as Dialog;
			bool passed = data.Equals(other);

			Assert.AreEqual(true, passed);
		}

		[TestMethod]
		public void ParseConversationWithOnlyDialog()
		{
			TomeStream stream = GetStream("MultipleCharacterExchange.txt");
			BrigitGraph conv = BrigitParser.ParseBrigitGraph(stream);

			BrigitGraph constructed = new BrigitGraph();
			// lol this looks digusting
			constructed.Add(new Node()
			{
				Data = new Dialog("Diego", "Heyo!", "How ya doing?", "You look cute today")
			});
			constructed.Add(new Node()
			{
				Data = new Dialog("Diana", "Haha thanks", "I'm fine, how are you?")
			});

			bool checker = conv.Equals(constructed);

			Assert.AreEqual(true, checker);
		}

		public void ParseConversationExchangeWithAttributes()
		{
			TomeStream stream = GetStream(".txt");
			BrigitGraph conv = BrigitParser.ParseBrigitGraph(stream);

			BrigitGraph constructed = new BrigitGraph();
			// lol this looks digusting
			constructed.Add(new Node()
			{
				Data = new Dialog("Diego", "Heyo!")
			});
			constructed.Add(new Node()
			{
				Data = new Dialog("Diana", "Hey whatsup?")
			});

			bool checker = conv.Equals(constructed);

			Assert.AreEqual(true, checker);
		}

		[TestMethod]
		public void ParseSimpleChoiceWithDescisionMethod()
		{
			TomeStream stream = GetStream("SimpleChoiceNoBranches.txt");
			(BrigitGraph conv, var notUsed) = BrigitParser.ParseDescision(stream);
			BrigitGraph constructed = new BrigitGraph();

			constructed.Add(new Node()
			{
				Data = new Descision()
				{
					Choices = new List<Choice>()
					{
						new Choice("You have either this choice"),
						new Choice("Or this other choice too"),
						new Choice("Maybe one more choice as well")
					}
				}
			});

			bool checker = conv.Equals(constructed);

			Assert.AreEqual(true, checker);
		}

		[TestMethod]
		public void ParaseChoiceWithBranchesWithDescisionMethod()
		{
			TomeStream stream = GetStream("ChoiceWithBranches.txt");
			(BrigitGraph conv, var thing) = BrigitParser.ParseDescision(stream);
			BrigitGraph constructed = new BrigitGraph();

			constructed.Add(new Node()
			{
				Data = new Descision()
				{
					Choices = new List<Choice>()
					{
						new Choice("This is one choice", 0),
						new Choice("Second choice is here after the branch", 1)
					}
				}
			});

			BrigitGraph branch = new BrigitGraph();
			branch.Add(new Node()
			{
				Data = new Dialog("Diego", "You chose the first choice")
			});

			constructed.AddBranch(constructed.Head, branch);

			bool checker = conv.Equals(constructed);

			Assert.AreEqual(true, checker);
		}

		[TestMethod]
		public void ParseChoiceWithBranchNames()
		{
			TomeStream stream = GetStream("ChoiceWithBranchName.txt");
			(BrigitGraph conv, var names) = BrigitParser.ParseDescision(stream);
			BrigitGraph constructed = new BrigitGraph();

			BrigitGraph subGraph = new BrigitGraph();
			subGraph.Add(new Node()
			{
				Data = new Dialog("Char1", "We can mix both up")
			});

			constructed.Add(new Node()
			{
				Data = new Descision()
				{
					Choices = new List<Choice>()
					{
						new Choice("this goes to a branchname", 1),
						new Choice("it ends withwhitespace or any none alpha character", 0),
						new Choice("and it will work just fine", 1)
					}
				}
			});
			// this is the completed parsing
			constructed.AddBranch(constructed.Head, subGraph);

			// creating the dictionary
			bool checker = conv.Equals(constructed);
			checker &= names.ContainsKey("SomeName") && names.ContainsKey("OtherName");

			if(checker)
			{
				(Node n, Choice ch) = names["SomeName"];
				Descision decide = constructed.Head.Data as Descision;
				checker &= n.Equals(constructed.Head);
				checker &= ch.Equals(decide.Choices[0]);
				
				(n, ch) = names["OtherName"];
				checker &= n.Equals(constructed.Head);
				checker &= ch.Equals(decide.Choices[2]);
			}

			Assert.AreEqual(true, checker);
		}

		[TestMethod]
		public void ParseBranchOnlyTest()
		{
			TomeStream stream = GetStream("ParseBranchOnlyTest.txt");
			(string name, BrigitGraph graph) = BrigitParser.ParseBranch(stream);
			BrigitGraph constructed = new BrigitGraph();
			constructed.Add(new Node()
			{
				Data = new Dialog("Character1", "hello how are you")
			});
			constructed.Add(new Node()
			{
				Data = new Dialog("Diego", "That's a stupid name")
			});

			bool checker = name == "BranchName";
			checker &= graph.Equals(constructed);

			Assert.AreEqual(true, checker);
		}

		[TestMethod]
		public void CleanStringOfMultipleSpacesTest()
		{
			string dirtyString = "  hello    how are*(&^* you\n\n doing; ";
			string clean = BrigitParser.CleanString(dirtyString);
			bool checker = clean.Equals("hello how are*(&^* you doing;");

			Assert.AreEqual(true, checker);
		}

		[TestMethod]
		public void DecodeStringThatPassesTest()
		{
			string notDecoded = @"helo\n how\* are you doing\today";
			string decoded = BrigitParser.DecodeString(notDecoded);
			bool checker = decoded.Equals("helo\n how* are you doing\today");

			Assert.AreEqual(true, checker);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void DecodeStringThatFailsTest()
		{
			string notDecoded = @"hello\t\y this should fail";
			string decoded = BrigitParser.DecodeString(notDecoded);
		}

		[TestMethod]
		public void LoadStringThenDecodeString()
		{
			TomeStream ts = GetStream("StringCleaning.txt");
			string decoded = BrigitParser.ParseAndCleanTextWithEscape(ts);
			bool checker = decoded.Equals("This\nis going*to s\\ay something");

			Assert.AreEqual(true, checker);
		}
	}
}
