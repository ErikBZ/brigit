using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Brigit;
using Brigit.Parser;
using Brigit.Parser.Stream;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using System.IO;
using System.Collections.Generic;

namespace Brigit.Test
{
	[TestClass]
	public class ParseTomeTests
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

		[TestMethod]
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

		[TestMethod]
		public void ParseTomeTest3Complete()
		{
			TomeStream stream = GetStream("TomeTest_3.txt");
		}
	}
}
