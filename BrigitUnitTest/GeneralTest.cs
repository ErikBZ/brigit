using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Brigit;
using Brigit.IO;
using Brigit.TomeParser;
using Brigit.Structure;

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
        public void TestExpression1()
        {
            // arrange
            // action
            // assert
        }

        [TestMethod]
        public void TestExpression2()
        {
            // arrange
            // action
            // assert
        }

        [TestMethod]
        public void TestEqualNode1()
        {
            // this should be the same
            // arrange
            DomNode node1 = new DomNode();
            node1.Character = "char";
            node1.RequiredFlags = "flag1";
            node1.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node1.Children = new DomNode[3];
            // arraning node 2
            DomNode node2 = new DomNode();
            node2.Character = "char";
            node2.RequiredFlags = "flag1";
            node2.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node2.Children = new DomNode[3];

            // acting
            bool areNodesEqual = node1.Equals(node2);

            // asserting
            Assert.AreEqual(areNodesEqual, true, "Two DomNodes were found to be different than each other");
        }

        [TestMethod] 
        public void TestEqualNode2()
        {
            // arrange
            DomNode node1 = new DomNode();
            node1.Character = "char1";
            node1.RequiredFlags = "flag1";
            node1.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node1.Children = new DomNode[3];
            // arraning node 2
            DomNode node2 = new DomNode();
            node2.Character = "char2";
            node2.RequiredFlags = "flag1";
            node2.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node2.Children = new DomNode[3];

            // acting
            bool areNodesEqual = node1.Equals(node2);

            // asserting
            Assert.AreEqual(areNodesEqual, false, "Two different DomNodes were found to be equal");
        }

        [TestMethod]
        public void TestEqualNode3()
        {
            // arrange
            Dialog node1 = new Dialog();
            node1.Character = "char";
            node1.RequiredFlags = "flag1";
            node1.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node1.Children = new DomNode[3];
            node1.speechText = "Say something";
            // arraning node 2
            Choice node2 = new Choice();
            node2.Character = "char";
            node2.RequiredFlags = "flag1";
            node2.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node2.Children = new DomNode[3];
            node2.Choices = new string[3];

            // acting
            bool areNodesEqual = node1.Equals(node2);

            Assert.AreEqual(areNodesEqual, false, "A choice node and dialog node were found to be equal");
        }

        [TestMethod]
        public void TestEqualNode4()
        {
            // arrange
            Choice node1 = new Choice();
            node1.Character = "char";
            node1.RequiredFlags = "flag1";
            node1.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node1.Children = new DomNode[3];
            node1.Choices = new string[3];

            // arraning node 2
            Choice node2 = new Choice();
            node2.Character = "char";
            node2.RequiredFlags = "flag1";
            node2.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node2.Children = new DomNode[3];
            node2.Choices = new string[3];

            // acting
            bool areNodesEqual = node1.Equals(node2);

            Assert.AreEqual(areNodesEqual, true, "Two equal DomNodes were found to be different");
        }

        [TestMethod]
        public void TestEqualNode5()
        {
			// arrange
			Dialog node1 = new Dialog()
			{
				Character = "char",
				RequiredFlags = "flag1",
				FlagToggles = new System.Collections.Generic.Dictionary<string, bool>(),
				Children = new DomNode[3],
				speechText = "Say something"
			};
			// arraning node 2
			Dialog node2 = new Dialog();
            node2.Character = "char";
            node2.RequiredFlags = "flag1";
            node2.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node2.Children = new DomNode[3];
            node2.speechText = "Say something";

            // acting
            bool areNodesEqual = node1.Equals(node2);

            // asserting
            Assert.AreEqual(areNodesEqual, true, "Two equal dialog nodes were found to be different");
        }

        [TestMethod]
        public void TestEqualNode6()
        {
            // arrange
            Dialog node1 = new Dialog();
            node1.Character = "char";
            node1.RequiredFlags = "flag1";
            node1.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node1.FlagToggles.Add("thing", true);
            node1.FlagToggles.Add("thing2", false);
            node1.Children = new DomNode[3];
            node1.speechText = "Say something";
            // arraning node 2
            Dialog node2 = new Dialog();
            node2.Character = "char";
            node2.RequiredFlags = "flag1";
            node2.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node2.FlagToggles.Add("thing", true);
            node2.FlagToggles.Add("thing2", false);
            node2.Children = new DomNode[3];
            node2.speechText = "Say something";

            // acting
            bool areNodesEqual = node1.Equals(node2);

            // asserting
            Assert.AreEqual(areNodesEqual, true, "Two dialog nodes with equal flags were found to be different");
        }

        [TestMethod]
        public void TestEqualNode7()
        {
            // arrange
            Dialog node1 = new Dialog();
            node1.Character = "char";
            node1.RequiredFlags = "flag1";
            node1.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node1.FlagToggles.Add("thing", false);
            node1.FlagToggles.Add("thing2", false);
            node1.Children = new DomNode[3];
            node1.speechText = "Say something";
            // arraning node 2
            Dialog node2 = new Dialog();
            node2.Character = "char";
            node2.RequiredFlags = "flag1";
            node2.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node2.FlagToggles.Add("thing", true);
            node2.FlagToggles.Add("thing2", false);
            node2.Children = new DomNode[3];
            node2.speechText = "Say something";

            // acting
            bool areNodesEqual = node1.Equals(node2);

            // asserting
            Assert.AreEqual(areNodesEqual, false, "Two equal dialog nodes were found to be different");
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
            Choice node2 = new Choice();
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
            Choice node2 = new Choice();
            node2.Character = "char2";
            tree1.Add(node1);
            tree1.Add(node2);

            DomTree tree2 = new DomTree();
            Dialog node3 = new Dialog();
            node3.Character = "char1";
            node3.Text = "hello";
            Choice node4 = new Choice();
            node4.Character = "char2";
            node4.Choices = new string[3];
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
            Choice node2 = new Choice();
            node2.Character = "char2";
            tree1.Add(node1);
            tree1.Add(node2);

            DomTree tree2 = new DomTree();
            Dialog node3 = new Dialog();
            node3.Character = "char1";
            node3.Text = "hello";
            Choice node4 = new Choice();
            node4.Character = "char2";
            tree2.Add(node3);
            tree2.Add(node4);

            // act
            bool areTreesEqual = tree1.Equals(tree2);
            //assert
            Assert.AreEqual(areTreesEqual, true);
        }


        [TestMethod]
        public void TestParseAttribute1()
        {
            // arrange
            // i'll do this at some point
            DomTree tree1 = new DomTree();
            Dialog node1 = new Dialog();
            node1.Character = "Character1";

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\attribute_test_1.tome");
			TomeParser.TomeParser parser = new TomeParser.TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();
            Dialog node = new Dialog();
            node.Character = "Character1";
            node.Text = "Hello there";

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            bool test = constructedTree.Equals(parsedTree);
            Assert.AreEqual(test, true, "Tree's are not equal");
        }

        [TestMethod]
        public void TestParseChoice1()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\choice_test_1.tome");
			TomeParser.TomeParser parser = new TomeParser.TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();
            Choice ch = new Choice();
            ch.Choices = new string[3];
            ch.Choices[0] = "ch1";
            ch.Choices[1] = "ch2";
            ch.Choices[2] = "ch3";
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
        public void TestParseChoice2()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\choice_test_2.tome");
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
        public void TestPraseDialog1()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\dialog_exchange_1.tome");
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
