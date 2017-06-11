using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Brigit;
using Brigit.IO;
using Brigit.TomeParser;
using Brigit.Structure;
using Brigit.Attributes;
using Brigit.Attributes.Operators;

namespace Brigit.Test
{
    [TestClass]
    public class GeneralTest
    {
        /*
         * This first set of functions is here to make sure that the overriden equals
         * function is working just fine
         */

        [TestMethod]
        public void TestEqualityOfTwoDomNodes_ShouldBeTrue()
        {
            // this should be the same
            // arrange
            DomNode node1 = new DomNode();
            node1.Character = "char";
            node1.Children = new DomNode[3];
            // arraning node 2
            DomNode node2 = new DomNode();
            node2.Character = "char";
            node2.Children = new DomNode[3];

            // acting
            bool areNodesEqual = node1.Equals(node2);

            // asserting
            Assert.AreEqual(areNodesEqual, true, "Two DomNodes were found to be different than each other");
        }

        [TestMethod] 
        public void TwoDifferentDomNodesShouldReturnFalse()
        {
            // arrange
            DomNode node1 = new DomNode();
            node1.Character = "char1";
            node1.Children = new DomNode[3];
            // arraning node 2
            DomNode node2 = new DomNode();
            node2.Character = "char2";
            node2.Children = new DomNode[3];

            // acting
            bool areNodesEqual = node1.Equals(node2);

            // asserting
            Assert.AreEqual(areNodesEqual, false, "Two different DomNodes were found to be equal");
        }

        [TestMethod]
        public void DialogAndChoicesCanNeverBeEqual()
        {
            // arrange
            Dialog node1 = new Dialog();
            node1.Character = "char";
            node1.Children = new DomNode[3];
            node1.speechText = "Say something";
            // arraning node 2
            UserChoice node2 = new UserChoice();
            node2.Character = "char";
            node2.Children = new DomNode[3];
            node2.Choices = new Selection[3];

            // acting
            bool areNodesEqual = node1.Equals(node2);

            Assert.AreEqual(areNodesEqual, false, "A choice node and dialog node were found to be equal");
        }

        [TestMethod]
        public void TwoChoicesWithTheSameChoicesShouldBeEqual()
        {
            // arrange
            UserChoice node1 = new UserChoice();
            node1.Character = "char";
            node1.Children = new DomNode[3];
            node1.Choices = new Selection[3];

            // arraning node 2
            UserChoice node2 = new UserChoice();
            node2.Character = "char";
            node2.Children = new DomNode[3];
            node2.Choices = new Selection[3];

            // acting
            bool areNodesEqual = node1.Equals(node2);

            Assert.AreEqual(areNodesEqual, true, "Two equal DomNodes were found to be different");
        }

        // testing small constructed DomTrees here
        [TestMethod]
        public void TestTreeEquality1()
        {
            // arrange
            DomTree tree1 = new DomTree();
            Dialog node1 = new Dialog();
            node1.Character = "char1";
            node1.Text = "hello";
            Dialog node2 = new Dialog();
            node2.Character = "char2";
            node2.Text = "hello there";
            tree1.Add(node1);
            tree1.Add(node2);

            DomTree tree2 = new DomTree();
            Dialog node3 = new Dialog();
            node3.Character = "char1";
            node3.Text = "hello";
            Dialog node4 = new Dialog();
            node4.Character = "char2";
            node4.Text = "hello there";
            tree2.Add(node3);
            tree2.Add(node4);
            
            // act
            bool areTreesEqual = tree1.Equals(tree2);
            //assert
            Assert.AreEqual(areTreesEqual, true);
        }

        [TestMethod]
        public void TestTreeEqaulity2()
        {
            // arrange
            DomTree tree1 = new DomTree();
            Dialog node1 = new Dialog();
            node1.Character = "char2";
            node1.Text = "What's up";
            Dialog node2 = new Dialog();
            node2.Character = "char1";
            node2.Text = "Hurrderr";
            tree1.Add(node1);
            tree1.Add(node2);

            DomTree tree2 = new DomTree();
            Dialog node3 = new Dialog();
            node3.Character = "char1";
            node3.Text = "hello";
            Dialog node4 = new Dialog();
            node4.Character = "char2";
            node4.Text = "hello there";
            tree2.Add(node3);
            tree2.Add(node4);

            // act
            bool areTreesEqual = tree1.Equals(tree2);
            //assert
            Assert.AreEqual(areTreesEqual, false);
        }

        [TestMethod]
        public void TestTreeEquailty3()
        {
            // arrange
            DomTree tree1 = new DomTree();
            Dialog node1 = new Dialog();
            node1.Character = "char1";
            node1.Text = "hello";
            UserChoice node2 = new UserChoice();
            node2.Character = "char2";
            tree1.Add(node1);
            tree1.Add(node2);

            DomTree tree2 = new DomTree();
            Dialog node3 = new Dialog();
            node3.Character = "char1";
            node3.Text = "hello";
            Dialog node4 = new Dialog();
            node4.Character = "char2";
            node4.Text = "hello there";
            tree2.Add(node3);
            tree2.Add(node4);

            // act
            bool areTreesEqual = tree1.Equals(tree2);
            //assert
            Assert.AreEqual(areTreesEqual, false);
        }

        [TestMethod]
        public void TestTreeEquality4()
        {
            // arrange
            DomTree tree1 = new DomTree();
            Dialog node1 = new Dialog();
            node1.Character = "char1";
            node1.Text = "hello";
            UserChoice node2 = new UserChoice();
            node2.Character = "char2";
            tree1.Add(node1);
            tree1.Add(node2);

            DomTree tree2 = new DomTree();
            Dialog node3 = new Dialog();
            node3.Character = "char1";
            node3.Text = "hello";
            UserChoice node4 = new UserChoice();
            node4.Character = "char2";
            node4.Choices = new Selection[3];
            tree2.Add(node3);
            tree2.Add(node4);

            // act
            bool areTreesEqual = tree1.Equals(tree2);
            //assert
            Assert.AreEqual(areTreesEqual, false);
        }

        [TestMethod]
        public void TestTreeEquality5()
        {
            // arrange
            DomTree tree1 = new DomTree();
            Dialog node1 = new Dialog();
            node1.Character = "char1";
            node1.Text = "hello";
            UserChoice node2 = new UserChoice();
            node2.Character = "char2";
            tree1.Add(node1);
            tree1.Add(node2);

            DomTree tree2 = new DomTree();
            Dialog node3 = new Dialog();
            node3.Character = "char1";
            node3.Text = "hello";
            UserChoice node4 = new UserChoice();
            node4.Character = "char2";
            tree2.Add(node3);
            tree2.Add(node4);

            // act
            bool areTreesEqual = tree1.Equals(tree2);
            //assert
            Assert.AreEqual(areTreesEqual, true);
        }


        [TestMethod]
        public void TestDialgWithAttributeCanParseAndBeEqual()
        {
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\attribute_test_1.tome");
			TomeParser.TomeParser parser = new TomeParser.TomeParser(lines);

            // arrange a tree that should be the product of this tome

            DomTree constructedTree = new DomTree();
			Dialog node = new Dialog()
			{
				Character = "Character1",
				Text = "Hello there",
				Attributes = new Attributes.AttributeManager(),
			};

			Variable setVar = new Variable("set");
			Variable stuffVar = new Variable("stuff");
			And andSetStuff = new And();
			andSetStuff.Add(setVar);
			andSetStuff.Add(stuffVar);
			node.Attributes.Expression = andSetStuff; 

			constructedTree.Add(node);

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            bool test = constructedTree.Equals(parsedTree);
            Assert.AreEqual(true, test, "Tree's are not equal");
        }

        [TestMethod]
        public void ParseAndCheckChoiceWithEmptyAttribute()
        {
            // arrange
            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\choice_test_1.tome");
			TomeParser.TomeParser parser = new TomeParser.TomeParser(lines);
            // arrange a tree that should be the product of this tome
            DomTree constructedTree = new DomTree();
			UserChoice ch = new UserChoice()
			{
				Choices = new Selection[]
				{
					new Selection(){ Text = "ch1"},
					new Selection(){ Text = "ch2"},
					new Selection(){ Text = "ch3"}

				}
			};

            constructedTree.Add(ch);

            Dialog diag = new Dialog();
            diag.Character = "Character1";
            diag.Text = "The player needed to choose something";
            constructedTree.Add(diag);

            // act
            DomTree parsedTree = parser.Parse();

			// assert
			bool test = constructedTree.Equals(parsedTree);
            Assert.AreEqual(test, true, "Tree's are not equal");
        }

        [TestMethod]
        public void TestChoiceWIthInnerBranches()
        {
            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\choice_test_2.tome");
			TomeParser.TomeParser parser = new TomeParser.TomeParser(lines);

            DomTree constructedTree = new DomTree();
			UserChoice ch1 = new UserChoice
			{
				Choices = new Selection[]
				{
					new Selection(){ Text = "ch1"},
					new Selection(){ Text = "ch2"},
					new Selection(){ Text = "ch3"}
				}
			};

			// subTrees
			Dialog char1 = new Dialog
			{
				Character = "Char1",
				Text = "Hello friends"
			};
			Dialog char2 = new Dialog
			{
				Character = "Char2",
				Text = "Hi"
			};
			DomTree subTree1 = new DomTree();
			subTree1.Add(char1);
			DomTree subTree2 = new DomTree();
			subTree2.Add(char2);
			// maybe i should just make that the default constructor?
			DomTree subTreeEmpty = DomTree.CreateEmptyDomTree();

			// ending dialog node
			Dialog char3 = new Dialog
			{
				Character = "Char3",
				Text = "Hello"
			};
			
			// adding them up
			constructedTree.Add(ch1);
			constructedTree.Add(subTree1, subTree2, subTreeEmpty);
			constructedTree.Add(char3);

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            bool test = constructedTree.Equals(parsedTree);
            Assert.AreEqual(test, true, "Tree's are not equal");
        }

        [TestMethod]
        public void TestParseChoice3()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\test_choice_3.tome");
			TomeParser.TomeParser parser = new TomeParser.TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            bool test = constructedTree.Equals(parsedTree);
            Assert.AreEqual(test, true, "Tree's are not equal");
        }

        [TestMethod]
        public void ParseDialogExchangeBetweenTwoCharacters()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\dialog_exchange_1.tome");
			TomeParser.TomeParser parser = new TomeParser.TomeParser(lines);
            // arrange a tree that should be the product of this tome
            DomTree constructedTree = new DomTree();
			Dialog char1 = new Dialog()
			{
				Character = "Character1",
				Text = "Yo what's up",
			};

			Dialog char2 = new Dialog()
			{
				Character = "Character2",
				Text = "Nothing much",
			};

			constructedTree.Add(char1);
			constructedTree.Add(char2);


            // act
            DomTree parsedTree = parser.Parse();

            // assert
            bool test = constructedTree.Equals(parsedTree);
            Assert.AreEqual(test, true, "Tree's are not equal");
        }

        [TestMethod]
        public void TestParseDialog2()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\dialog_exchange_2.tome");
			TomeParser.TomeParser parser = new TomeParser.TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            bool test = constructedTree.Equals(parsedTree);
            Assert.AreEqual(test, true, "Tree's are not equal");
        }

        [TestMethod]
        public void TestParseSyntax1()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\syntax_test_1.tome");
			TomeParser.TomeParser parser = new TomeParser.TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            bool test = constructedTree.Equals(parsedTree);
            Assert.AreEqual(test, true, "Tree's are not equal");
        }

        [TestMethod]
        public void TestParseSyntax2()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\syntax_test_2.tome");
			TomeParser.TomeParser parser = new TomeParser.TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            bool test = constructedTree.Equals(parsedTree);
            Assert.AreEqual(test, true, "Tree's are not equal");
        }
    }
}
