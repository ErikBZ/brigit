using System;
using Brigit.Parser;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Parser.Stream;
using Brigit.Parser.Wrapper;
using Brigit.Attributes;
using System.IO;
using Brigit.IO;
using System.Collections.Generic;
using NUnit.Framework;
using Brigit.Attributes.ExpressionParser;
using YamlDotNet.RepresentationModel;

namespace Brigit.Test
{
	[TestFixture]
	public class ParserUnitTests
	{
		public TomeStream GetStream(string testFileName)
		{
			string path = Path.Combine(Config.TomePath, testFileName);
			string[] tome = File.ReadAllLines(path);
			string[] tomeNoComments = CommentRemover.RemoveComments(tome);
			TomeStream stream = new TomeStream(tomeNoComments);
			return stream;
		}

        [Test]
        public void Create_Dialog_Object_From_Yaml()
        {
            // arrange
            String testString = @"graph:
                                - dialog:
                                  character: Person 1
                                  speech:
                                    - text: Hello how are you
                                ";

            var yaml = new YamlStream();
            yaml.Load(new StringReader(testString));
            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            var brigitNodes = (YamlSequenceNode)mapping.Children[new YamlScalarNode("graph")];
            var parser = new BrigitYamlParser();

            // what it should be
            var expected = new Dialog("Person 1", "Hello how are you");

            // act
            var dialog = parser.CreateDialog((YamlMappingNode) brigitNodes.Children[0]);

            // assert
            Assert.AreEqual(expected, dialog);
        }

        [Test]
        public void Create_Choice_Object_From_Yaml()
        {
            // arrange
            String testString = @"graph:
                                  - decision:
                                    - choice: This is a choice
                                    - choice: This is another choice";

            var expected = new BrigitGraph();
            var decision = new Decision();
            decision.Choices.Add(new Choice("This is a choice"));
            decision.Choices.Add(new Choice("This is another choice"));
            expected.AddNode(new Node(decision));

            var yaml = new YamlStream();
            yaml.Load(new StringReader(testString));
            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            var brigitNodes = (YamlSequenceNode)mapping.Children[new YamlScalarNode("graph")];
            var parser = new BrigitYamlParser();

            // act
            var descision = parser.CreateDecision((YamlMappingNode)brigitNodes.Children[0]);

            // assert
            Assert.AreEqual(expected, descision);
        }
	}
}
