using System;
using Brigit;
using Brigit.Parser;
using Brigit.Parser.Stream;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;

namespace Brigit.Test
{
	[TestFixture]
	public class ParseTomeTests
		{
		const string RootDirectory = @"..\..\Tests\";

		public TomeStream GetStream(string testFileName)
		{
			string[] tome = File.ReadAllLines(RootDirectory + testFileName);
			string[] tomeNoComments = CommentRemover.RemoveComments(tome);
			TomeStream stream = new TomeStream(tomeNoComments);
			return stream;
		}

		[Test]
		public void ParseTomeTest1Complete()
		{
			TomeStream stream = GetStream("TomeTest_1.txt");
			BrigitGraph conv = BrigitParser.ParseBrigitGraph(stream);

			BrigitGraph constructed = new BrigitGraph();
			constructed.Add(new Node(){
				Data = new Dialog("Diego", "Hello")
			});
			constructed.Add(new Node(){
				Data = new Descision(){
					Choices = new List<Choice>(){
						new Choice("Fuck you"),
						new Choice("Hello"),
						new Choice("Blahblah")
					}
				}
			});
			constructed.Add(new Node(){
				Data = new Dialog("Diego", "Ok")
			});

			bool checker = conv.Equals(constructed);

			Assert.AreEqual(true, checker);
		}

		[Test]
		public void ParseTomeTest2Complete()
		{
			TomeStream stream = GetStream("TomeTest_2.txt");
			BrigitGraph conv = BrigitParser.ParseBrigitGraph(stream);

			BrigitGraph constructed = new BrigitGraph();
			constructed.Add(new Node(){
				Data = new Dialog("Yulia", "What the fuck is this", "What are you doing?")
			});

			// the choice sub graph
			BrigitGraph subGraph = new BrigitGraph();
			Descision root = new Descision() {
				Choices = new List<Choice>(){
					new Choice("Nothing", 0),
					new Choice("Everything", 2),
					new Choice("Go away", 1),
				}
			};

			subGraph.Add(new Node(){
				Data = root
			});	

			// the first branch
			BrigitGraph nothingBranch = new BrigitGraph();
			nothingBranch.Add(new Node(){
				Data = new Dialog("Yulia", "You're lying")
			});
			nothingBranch.Add(new Node(){
				Data = new Dialog("Diego", "Yeah she is")
			});

			subGraph.AddBranch(subGraph.Head, nothingBranch);

			// the second branch pointed to by the 3rd choice
			BrigitGraph goAwayBranch = new BrigitGraph();
			goAwayBranch.Add(new Node(){
				Data = new Dialog("Yulia", "NO")
			});
			subGraph.AddBranch(subGraph.Head, goAwayBranch);

			constructed.Add(subGraph);
			constructed.Add(new Node(){
				Data = new Dialog("Diego", "There's a lot of yelling going on right now")
			});


			bool checker = conv.Equals(constructed);

			Assert.AreEqual(true, checker);
		}

		[Test]
		public void ParseTomeTest3Complete()
		{
			TomeStream stream = GetStream("TomeTest_3.txt");
            var conv = BrigitParser.ParseBrigitGraph(stream);
            var constructed = new BrigitGraph();

            constructed.Add(new Node
            {
                Data = new Dialog("Diana", "I didn't want to be the one to forget")
            });
            constructed.Add(new Node
            {
                Data = new Dialog("Diego", "I thought of everything I'd never regret")
            });

            var choice = new Node()
            {
                Data = new Descision()
                {
                    Choices = new List<Choice>()
                    {
                        new Choice("A little time with you is all that I get", 2),
                        new Choice("That's all we need because that's all we can take", 0),
                        new Choice("I don't believe in him - his lips on the ground", 1),
                        new Choice("I wanna take you back to the place by the rock", 1)
                    }
                }
            };
            constructed.Add(choice);

            var diegoChoiceSubGraph = new BrigitGraph();
            diegoChoiceSubGraph.Add(new Node()
            {
                Data = new Dialog("Diego", "One thing I never see the same when you're round")
            });

            // will probably check here to make sure this works
            constructed.AddBranch(choice, diegoChoiceSubGraph);

            constructed.Add(new Node() {
                Data = new Dialog("Diana", "But no one gives us time anymore")
            });

            // chorus creation and then addition
            var chorusSubGraph = new BrigitGraph();
            chorusSubGraph.Add(new Node
            {
                Data = new Dialog("Diego", "I gotta be in your arms baby", "But far away I seek for your light",
                    "I hold on because for you my heart keeps beating")
            });

            constructed.AddInBetween(choice, chorusSubGraph);

            // the last thing that diego says

            constructed.Add(new Node()
            {
                Data = new Dialog("Diego", "Will you be my light?")
            });


            bool checker = conv.Equals(constructed);
            Assert.AreEqual(true, checker);
		}

        [TestMethod]
        public void TestConverstaionLoader()
        {
        }
	}
}
