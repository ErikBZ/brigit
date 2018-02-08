using System;
using Brigit;
using Brigit.Parser;
using Brigit.Parser.Stream;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Attributes.Operators;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;

namespace Brigit.Test
{
    [TestFixture]
    public class ParseTomeTests
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
        public void ParseTomeTest1Complete()
        {
            TomeStream stream = GetStream("TomeTest_1.txt");
            BrigitParser bParser = new BrigitParser(stream);
            BrigitGraph conv = bParser.ParseBrigitGraph(stream);

            BrigitGraph constructed = new BrigitGraph();
            constructed.Add(new Node() {
                Data = new Dialog("Diego", "Hello")
            });
            constructed.Add(new Node() {
                Data = new Descision() {
                    Choices = new List<Choice>(){
                        new Choice("Fuck you"),
                        new Choice("Hello"),
                        new Choice("Blahblah")
                    }
                }
            });
            constructed.Add(new Node() {
                Data = new Dialog("Diego", "Ok")
            });

            bool checker = conv.Equals(constructed);

            Assert.AreEqual(true, checker);
        }

        [Test]
        public void ParseTomeTest2Complete()
        {
            TomeStream stream = GetStream("TomeTest_2.txt");
            BrigitParser bParser = new BrigitParser(stream);
            BrigitGraph conv = bParser.ParseBrigitGraph(stream);

            BrigitGraph constructed = new BrigitGraph();
            constructed.Add(new Node() {
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

            subGraph.Add(new Node() {
                Data = root
            });

            // the first branch
            BrigitGraph nothingBranch = new BrigitGraph();
            nothingBranch.Add(new Node() {
                Data = new Dialog("Yulia", "You're lying")
            });
            nothingBranch.Add(new Node() {
                Data = new Dialog("Diego", "Yeah she is")
            });

            subGraph.AddBranch(subGraph.Head, nothingBranch);

            // the second branch pointed to by the 3rd choice
            BrigitGraph goAwayBranch = new BrigitGraph();
            goAwayBranch.Add(new Node() {
                Data = new Dialog("Yulia", "NO")
            });
            subGraph.AddBranch(subGraph.Head, goAwayBranch);

            constructed.Add(subGraph);
            constructed.Add(new Node() {
                Data = new Dialog("Diego", "There's a lot of yelling going on right now")
            });


            bool checker = conv.Equals(constructed);

            Assert.AreEqual(true, checker);
        }

        [Test]
        // The multiple lines to the tail node are being created
        // because of the recurisve nature of the ToString function i wrote
        public void ParseTomeTest3Complete()
        {
            TomeStream stream = GetStream("TomeTest_3.txt");
            BrigitParser bParser = new BrigitParser(stream);
            var conv = bParser.ParseBrigitGraph(stream);
            var constructed = new BrigitGraph();

            constructed.Add(new Node
            {
                Data = new Dialog("Diana", "I didn't want to be the one to forget")
            });
            constructed.Add(new Node
            {
                Data = new Dialog("Diego", "I thought of everything I'd never regret")
            });

            // looks like they're routing to the wrong places
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
            // the error may happen some where around here
            constructed.AddBranch(choice, diegoChoiceSubGraph);

            // everything seems fine up to this point
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

            // this right here is fucked up
            // trying to fix in with the new method
            constructed.AddInBetween(choice, constructed.Tails, chorusSubGraph);

            // the last thing that diego says

            constructed.Add(new Node()
            {
                Data = new Dialog("Diego", "Will you be my light?")
            });


            bool checker = conv.Equals(constructed);
            Assert.AreEqual(true, checker);
        }

        [Test]
        public void ParseTomeTest4_With_Attributes()
        {
            TomeStream stream = GetStream("TomeTest_4.txt");
            BrigitParser brigitP = new BrigitParser(stream);
            var conv = brigitP.Parse();

            var constructed = new BrigitGraph();
            constructed.Add(new Node
            {
                Data = new Dialog("Diego", "Hey what's happening")
            });

            // first choice
            Choice ch1 = new Choice("This sets one to true", 0);
            ch1.Attributes.SetFlags.Add("one", Attributes.Flag.True);
            Choice ch2 = new Choice("This sets two to true", 0);
            ch2.Attributes.SetFlags.Add("two", Attributes.Flag.True);

            // the decsion block
            var choices = new Node()
            {
                Data = new Descision()
                {
                    Choices = new List<Choice>
                    {
                        ch1,
                        ch2
                    }
                }
            };
            constructed.Add(choices);

            // Dialog Node
            var dialog = new Dialog("Person");
            var speech1 = new SpeechText("Hello");
            speech1.Attributes.Expression = new Variable("two");
            var speech2 = new SpeechText("Hey");
            speech2.Attributes.Expression = new Variable("one");
            dialog.Text = new List<SpeechText>()
            {
                speech1,
                speech2,
                new SpeechText("Blah")
            };

            constructed.Add(new Node()
            {
                Data = dialog
            });

            // second dialog node
            var dialog2 = new Dialog("Other", "Heyo", "What is going on");
            dialog2.Attributes.Expression = new Variable("one");

            constructed.Add(new Node()
            {
                Data = dialog2
            });

            //assertion
            bool checker = conv.Equals(constructed);
            Assert.AreEqual(true, checker);
        }
	}
}
