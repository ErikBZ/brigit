using System;
using System.IO;
using System.Text;
using Brigit;
using Brigit.IO;
using Brigit.Parser;
using Brigit.Parser.Stream;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Attributes.Operators;
using YamlDotNet.RepresentationModel;
using System.Collections.Generic;
using NUnit.Framework;

namespace Brigit.Test
{
    [TestFixture]
    public class ParseTomeTests
    {
        public TomeStream GetStream(string testFileName)
        {
			string path = Path.Combine(Config.TomePath, testFileName);
            string[] tome = File.ReadAllLines(path);
            string[] tomeNoComments = CommentRemover.RemoveComments(tome);
            TomeStream stream = new TomeStream(tomeNoComments);
            return stream;
        }

        public StringReader GetReader(string fileName)
        {
            string path = Path.Combine(Config.TomePath, fileName);
            if (File.Exists(path))
            {
                return new StringReader(File.ReadAllText(path));
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        [Test]
        public void Open_YmlTestFiles()
        {
            string[] files = Directory.GetFiles(Config.TomePath, "*.yml");

            foreach (string file in files)
            {
                TomeReader.LoadBrigitYamlFile(file);
            }

        }
        
        [Test]
        public void Parse_Multi_Character_Exchange()
        {
            var yaml = new YamlStream();
            yaml.Load(GetReader("MultipleCharacterExchange.yml"));
            var mapping = (YamlMappingNode) yaml.Documents[0].RootNode;
            var yamlParser = new BrigitYamlParser(mapping);
            var conv = yamlParser.CreateGraphFromYaml();

            var constructed = new BrigitGraph();
            constructed.AddNode(new Node()
            {
                Data = new Dialog("Diego", "Heyo!", "How ya doing?", "You look cute today")
            });
            constructed.AddNode(new Node()
            {
                Data = new Dialog("Diana", "Haha thanks.", "I'm fine, how are you?")
            });

            Assert.AreEqual(constructed, conv); 
        }


        [Test]
        public void Parse_TomeTest1()
        {
            var yaml = new YamlStream();
            yaml.Load(GetReader("TomeTest_1.yml"));
            var mapping = (YamlMappingNode) yaml.Documents[0].RootNode;
            var yamlParser = new BrigitYamlParser(mapping);
            var conv = yamlParser.CreateGraphFromYaml();

            BrigitGraph constructed = new BrigitGraph();
            constructed.AddNode(new Node() {
                Data = new Dialog("Diego", "Hello")
            });
            constructed.AddNode(new Node() {
                Data = new Decision() {
                    Choices = new List<Choice>(){
                        new Choice("Fuck you"),
                        new Choice("Hello"),
                        new Choice("Blahblah")
                    }
                }
            });
            constructed.AddNode(new Node() {
                Data = new Dialog("Diego", "Ok")
            });

            bool checker = conv.Equals(constructed);

            Assert.AreEqual(true, checker);
        }

        [Test]
        public void Parse_TomeTest2()
        {
            var yaml = new YamlStream();
            yaml.Load(GetReader("TomeTest_2.yml"));
            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            var yamlParser = new BrigitYamlParser(mapping);
            var conv = yamlParser.CreateGraphFromYaml();

            BrigitGraph constructed = new BrigitGraph();
            constructed.AddNode(new Node() {
                Data = new Dialog("Yulia", "What the fuck is this", "What are you doing?")
            });

            // the choice sub graph
            BrigitGraph subGraph = new BrigitGraph();
            Decision root = new Decision() {
                Choices = new List<Choice>(){
                    new Choice("Nothing", 0),
                    new Choice("Everything", 2),
                    new Choice("Go away", 1),
                }
            };

            subGraph.AddNode(new Node() {
                Data = root
            });

            // the first branch
            BrigitGraph nothingBranch = new BrigitGraph();
            nothingBranch.AddNode(new Node() {
                Data = new Dialog("Yulia", "You're lying")
            });
            nothingBranch.AddNode(new Node() {
                Data = new Dialog("Diego", "Yeah she is")
            });

            subGraph.AddBranch(subGraph.Head, nothingBranch);

            // the second branch pointed to by the 3rd choice
            BrigitGraph goAwayBranch = new BrigitGraph();
            goAwayBranch.AddNode(new Node() {
                Data = new Dialog("Yulia", "NO")
            });
            subGraph.AddBranch(subGraph.Head, goAwayBranch);

            constructed.AddGraph(subGraph);
            constructed.AddNode(new Node() {
                Data = new Dialog("Diego", "There's a lot of yelling going on right now")
            });


            bool checker = conv.Equals(constructed);

            Assert.AreEqual(true, checker);
        }

        [Test]
        // The multiple lines to the tail node are being created
        // because of the recurisve nature of the ToString function i wrote
        public void Parse_TomeTest3()
        {
            var yaml = new YamlStream();
            yaml.Load(GetReader("TomeTest_3.yml"));
            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            var yamlParser = new BrigitYamlParser(mapping);
            var conv = yamlParser.CreateGraphFromYaml();

            var constructed = new BrigitGraph();

            constructed.AddNode(new Node
            {
                Data = new Dialog("Diana", "I didn't want to be the one to forget")
            });
            constructed.AddNode(new Node
            {
                Data = new Dialog("Diego", "I thought of everything I'd never regret")
            });

            // looks like they're routing to the wrong places
            var choice = new Node()
            {
                Data = new Decision()
                {
                    Choices = new List<Choice>()
                    {
                        new Choice("A little time with you is all that I get", 0),
                        new Choice("That's all we need because that's all we can take", 1),
                        new Choice("I don't believe in him - his lips on the ground", 2),
                        new Choice("I wanna take you back to the place by the rock", 2)
                    }
                }
            };
            constructed.AddNode(choice);

            // chorus creation and then addition
            var chorusSubGraph = new BrigitGraph();
            chorusSubGraph.AddNode(new Node
            {
                Data = new Dialog("Diego", "I gotta be in your arms baby", "But far away I seek for your light",
                    "I hold on because for you my heart keeps beating")
            });
            constructed.AddBranch(choice, chorusSubGraph);


            var diegoChoiceSubGraph = new BrigitGraph();
            diegoChoiceSubGraph.AddNode(new Node()
            {
                Data = new Dialog("Diego", "One thing I never see the same when you're round")
            });

            // will probably check here to make sure this works
            // the error may happen some where around here
            constructed.AddBranch(choice, diegoChoiceSubGraph);

            // everything seems fine up to this point
            constructed.AddNode(new Node() {
                Data = new Dialog("Diana", "But no one gives us time anymore")
            });

            constructed.AddNode(new Node()
            {
                Data = new Dialog("Diego", "Will you be my light?")
            });


            bool checker = conv.Equals(constructed);
            Assert.AreEqual(true, checker);
        }

        [Test]
        public void Parse_TomeTest4()
        {
            var yaml = new YamlStream();
            yaml.Load(GetReader("TomeTest_4.yml"));
            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            var yamlParser = new BrigitYamlParser(mapping);
            var conv = yamlParser.CreateGraphFromYaml();

            var constructed = new BrigitGraph();
            constructed.AddNode(new Node
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
                Data = new Decision()
                {
                    Choices = new List<Choice>
                    {
                        ch1,
                        ch2
                    }
                }
            };
            constructed.AddNode(choices);

            // Dialog Node
            var dialog = new Dialog("Person");
            var speech1 = new SpeechText("Hello");
            speech1.Attributes.Expression = new Variable("one");
            var speech2 = new SpeechText("Hey");
            speech2.Attributes.Expression = new Variable("two");
            dialog.Text = new List<SpeechText>()
            {
                speech1,
                speech2,
                new SpeechText("Blah")
            };

            constructed.AddNode(new Node()
            {
                Data = dialog
            });

            // second dialog node
            var dialog2 = new Dialog("Other", "Heyo", "What's going on");
            dialog2.Attributes.Expression = new Variable("one");

            constructed.AddNode(new Node()
            {
                Data = dialog2
            });

            //assertion
            bool checker = conv.Equals(constructed);
            Assert.AreEqual(true, checker);
        }
	}
}
