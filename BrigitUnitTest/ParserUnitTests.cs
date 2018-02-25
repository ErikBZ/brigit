using System;
using Brigit.Parser;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Parser.Stream;
using Brigit.Parser.Wrapper;
using Brigit.Attributes;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using Brigit.Attributes.ExpressionParser;

namespace Brigit.Test
{
	[TestFixture]
	public class ParserUnitTests
	{
		const string RootDirectory = @"E:\Users\zapat\Documents\brigit\BrigitUnitTest\Tests\";

		public TomeStream GetStream(string testFileName)
		{
			string[] tome = File.ReadAllLines(RootDirectory + testFileName);
			string[] tomeNoComments = CommentRemover.RemoveComments(tome);
			TomeStream stream = new TomeStream(tomeNoComments);
			return stream;
		}

		[Test]
		public void ParseDialogTestTome()
		{
			string character = "Character1";
			string text = "I'm going to say something";
			TomeStream stream = GetStream("DialogTestTome.txt");
            BrigitParser bParser = new BrigitParser(stream);
			Node n = bParser.ParseDialog(stream);
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

		[Test]
		public void ParseDialogWithMultipleSpeechTexts()
		{
			TomeStream stream = GetStream("MultipleSpeechText.txt");
            BrigitParser bParser = new BrigitParser(stream);
			Node n = bParser.ParseDialog(stream);

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

		[Test]
		public void ParseConversationWithOnlyDialog()
		{
			TomeStream stream = GetStream("MultipleCharacterExchange.txt");
            BrigitParser bParser = new BrigitParser(stream);
			BrigitGraph conv = bParser.ParseBrigitGraph(stream);

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


		[Test]
		public void ParseSimpleChoiceWithDescisionMethod()
		{
			TomeStream stream = GetStream("SimpleChoiceNoBranches.txt");
            var notUsed = new Dictionary<string, OpenChoice>();
            BrigitParser bParser = new BrigitParser(stream);
			BrigitGraph conv = bParser.ParseDescision(stream, notUsed);
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

		[Test]
		public void ParaseChoiceWithBranchesWithDescisionMethod()
		{
			TomeStream stream = GetStream("ChoiceWithBranches.txt");
            var thing = new Dictionary<string, OpenChoice>();
            BrigitParser bParser = new BrigitParser(stream);
			BrigitGraph conv = bParser.ParseDescision(stream, thing);
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

		[Test]
		public void ParseChoiceWithBranchNames()
		{
			TomeStream stream = GetStream("ChoiceWithBranchName.txt");
            var names = new Dictionary<string, OpenChoice>();
            BrigitParser bParser = new BrigitParser(stream);
			BrigitGraph conv = bParser.ParseDescision(stream, names);
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
                var openCh = names["SomeName"];
                var n = openCh.EnclosingNode;
                var ch = openCh.BranchingChoice;

				Descision decide = constructed.Head.Data as Descision;
				checker &= n.Equals(constructed.Head);
				checker &= ch.Equals(decide.Choices[0]);

                openCh = names["OtherName"];
                n = openCh.EnclosingNode;
                ch = openCh.BranchingChoice;
				checker &= n.Equals(constructed.Head);
				checker &= ch.Equals(decide.Choices[2]);
			}

			Assert.AreEqual(true, checker);
		}

		[Test]
		public void ParseBranchOnlyTest()
		{
			TomeStream stream = GetStream("ParseBranchOnlyTest.txt");
            String name = String.Empty;
            BrigitParser bParser = new BrigitParser(stream);
			BrigitGraph graph = bParser.ParseBranch(stream, ref name);
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

		[Test]
		public void CleanStringOfMultipleSpacesTest()
		{
			string dirtyString = "  hello    how are*(&^* you\n\n doing; ";
            BrigitParser bParser = new BrigitParser(new TomeStream());
			string clean = bParser.CleanString(dirtyString);
			bool checker = clean.Equals("hello how are*(&^* you doing;");

			Assert.AreEqual(true, checker);
		}

		[Test]
		public void DecodeStringThatPassesTest()
		{
			string notDecoded = @"helo\n how\* are you doing\today";
			string decoded = BrigitParser.DecodeString(notDecoded);
			bool checker = decoded.Equals("helo\n how* are you doing\today");

			Assert.AreEqual(true, checker);
		}

		[Test]
		public void DecodeStringThatFailsTest()
		{
            // arrange
			string notDecoded = @"hello\t\y this should fail";
            // act and assert
            Assert.Throws<Exception>(() => BrigitParser.DecodeString(notDecoded));
		}

		[Test]
		public void LoadStringThenDecodeString()
		{
			TomeStream ts = GetStream("StringCleaning.txt");
            BrigitParser bParser = new BrigitParser(ts);
			string decoded = bParser.ParseAndCleanTextWithEscape(ts);
			bool checker = decoded.Equals("This\nis going*to s\\ay something");

			Assert.AreEqual(true, checker);
		}

        [Test]
        public void Parse_Flag_Setting_Valid_Parameter()
        {
            //assemble
            string valid = "[SetT: yes this    \nwill work    ]";
            TomeStream stream = new TomeStream(new string[] { valid });
            BrigitParser bParser = new BrigitParser(stream);

            AttributeManager constructed = new AttributeManager();
            constructed.SetFlags.Add("yes", Flag.True);
            constructed.SetFlags.Add("this", Flag.True);
            constructed.SetFlags.Add("will", Flag.True);
            constructed.SetFlags.Add("work", Flag.True);

            // act
            var manager = bParser.ParseAttributes(stream);

            // assert
            bool checker = manager.Equals(constructed);
            Assert.AreEqual(true, checker);
        }

        [Test]
        public void Parse_Branch_Selection()
        {
            //assemble
            TomeStream stream = GetStream("BranchSelector.txt");
            BrigitParser bParser = new BrigitParser(stream);

            BrigitGraph constructed = new BrigitGraph();
            Dictionary<string, OpenChoice> endings = new Dictionary<string, OpenChoice>();

            Descision descision = new Descision();
            descision.Interactive = false;
            var ch1 = new Choice();
            ch1.Attributes.Expression = BrigitExpressionParser.Parse("x|s");
            ch1.NextNode = 2;
            var ch2 = new Choice();
            ch2.Attributes.Expression = BrigitExpressionParser.Parse("b&d");
            ch2.NextNode = 0;
            var ch3 = new Choice();
            ch3.NextNode = 1;
            descision.Choices = new List<Choice>()
            {
                ch1, ch2, ch3
            };

            Node root = new Node() {
                Data = descision
            };

            constructed.Add(root);
            // adding the branches
            Node br1 = new Node()
            {
                Data = new Dialog("Diego", "Hi")
            };
            BrigitGraph bg1 = new BrigitGraph();
            bg1.Add(br1);

            Node br2 = new Node()
            {
                Data = new Dialog("James", "Hello")
            };
            BrigitGraph bg2 = new BrigitGraph();
            bg2.Add(br2);

            constructed.AddBranch(root, bg1);
            constructed.AddBranch(root, bg2);

            //act
            BrigitGraph bg = bParser.ParseBranchSelector(endings);

            //assert
            bool checker = bg.Equals(constructed);
            Assert.AreEqual(true, checker);
        }
	}
}
