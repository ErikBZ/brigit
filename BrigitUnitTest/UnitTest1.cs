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
        public void TestParseChoice2()
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
        public void TestParseChoice3()
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
        public void TestPraseDialog1()
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
        public void TestParseDialog2()
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
        public void TestParseSyntax1()
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
        public void TestParseSyntax2()
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
    }
}
