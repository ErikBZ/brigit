using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Brigit;
using Brigit.IO;
using Brigit.Parser;
using Brigit.Structure;

namespace BrigitUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        /*
         * This first set of functions is here to make sure that the overriden equals
         * function is working just fine
         */
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
            Dialog node1 = new Dialog();
            node1.Character = "char";
            node1.RequiredFlags = "flag1";
            node1.FlagToggles = new System.Collections.Generic.Dictionary<string, bool>();
            node1.Children = new DomNode[3];
            node1.speechText = "Say something";
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
        public void TestParseAttribute1()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\attribute_test_1.tome");
            TomeParser parser = new TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            Assert.AreEqual(constructedTree, parsedTree, "Tree's are not equal");
        }

        [TestMethod]
        public void TestParseChoice1()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\choice_test_1.tome");
            TomeParser parser = new TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            Assert.AreEqual(constructedTree, parsedTree, "Tree's are not equal");
        }

        [TestMethod]
        public void TestParseChoice2()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\choice_test_2.tome");
            TomeParser parser = new TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            Assert.AreEqual(constructedTree, parsedTree, "Tree's are not equal");
        }

        [TestMethod]
        public void TestParseChoice3()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\test_choice_3.tome");
            TomeParser parser = new TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            Assert.AreEqual(constructedTree, parsedTree, "Tree's are not equal");
        }

        [TestMethod]
        public void TestPraseDialog1()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\dialog_exchange_1.tome");
            TomeParser parser = new TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            Assert.AreEqual(constructedTree, parsedTree, "Tree's are not equal");
        }

        [TestMethod]
        public void TestParseDialog2()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\dialog_exchange_2.tome");
            TomeParser parser = new TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            Assert.AreEqual(constructedTree, parsedTree, "Tree's are not equal");
        }

        [TestMethod]
        public void TestParseSyntax1()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\syntax_test_1.tome");
            TomeParser parser = new TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            Assert.AreEqual(constructedTree, parsedTree, "Tree's are not equal");
        }

        [TestMethod]
        public void TestParseSyntax2()
        {
            // arrange
            // i'll do this at some point

            // setting up the parser to parse
            string[] lines = BrigitIO.ReadTomeFile(@"..\..\Tests\syntax_test_2.tome");
            TomeParser parser = new TomeParser(lines);
            // arrange a tree that should be the product of this tome
            /*
             * TODO write all the shit out at some point
             */
            DomTree constructedTree = new DomTree();

            // act
            DomTree parsedTree = parser.Parse();

            // assert
            Assert.AreEqual(constructedTree, parsedTree, "Tree's are not equal");
        }
    }
}
