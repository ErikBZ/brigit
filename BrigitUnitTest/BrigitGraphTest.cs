using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.IO;
using NUnit.Framework;
using Brigit;

namespace Brigit.Test
{
    [TestFixture]
    public class BrigitGraphTest
    {
        string root = @"E:\Users\zapat\Documents\brigit\BrigitUnitTest\Tests\{0}";
        public string TraverseGraph(Conversation conv, int[] choices)
        {
            int choiceTracker = 0;
            StringBuilder sb = new StringBuilder();
            // action
            conv.StartNewRun();
            bool errorOccured = false;
            while (!conv.Complete && !errorOccured)
            {
                Info inf = conv.GetInfo();

                // getting the next one
                switch (inf.type)
                {
                    case Info.Type.Dialog:
                        sb.Append(inf.ToString());
                        errorOccured = !(conv.GoToNext());
                        break;
                    case Info.Type.Descision:
                        int ch = choices[choiceTracker];
                        int next = inf.Descision.Choices[ch].NextNode;
                        sb.Append(String.Format("{0}: ", ch));
                        sb.Append(inf.Descision.Choices[ch].ToString());
                        sb.Append("-- " + next.ToString());
                        // going to the next node specificed by the choice
                        errorOccured = !(conv.GoToNext(inf.Descision.Choices[ch].NextNode));
                        break;
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
        
        [Test]
        public void TomeTest2_Graph_Traversal_Choice_1()
        {
            int[] choices = new int[] { 0 };
            var conv = ConversationLoader.CreateConversation(String.Format(root, "TomeTest_2.txt"));
            string result = TraverseGraph(conv, choices);
            string expected = "Yulia: What the fuck is this\n" +
                              "Yulia: What are you doing?\n" +
                              "0: Nothing-- 0\n" +
                              "Yulia: You're lying\n" +
                              "Diego: Yeah she is\n" +
                              "Diego: There's a lot of yelling going on right now\n";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TomeTest3_Graph_Traversal_Choice_2()
        {
            int[] choices = new int[] { 2 };
            var conv = ConversationLoader.CreateConversation(String.Format(root, "TomeTest_3.txt"));

            string result = TraverseGraph(conv, choices);
            string expected = "Diana: I didn't want to be the one to forget\n" +
                              "Diego: I thought of everything I'd never regret\n" +
                              "2: I don't believe in him - his lips on the ground-- 1\n" +
                              "Diana: But no one gives us time anymore\n" +
                              "Diego: Will you be my light?\n";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TomeTest3_Graph_Traversal_Choice_0()
        {
            // These are inputs, for the traversal
            // for descisions these correspond to the chosen choice not the branch
            // assemble
            int[] choices = new int[]{0};
            var conv = ConversationLoader.CreateConversation(String.Format(root, "TomeTest_3.txt"));

            // action
            string result = TraverseGraph(conv, choices);
            string expected = "Diana: I didn't want to be the one to forget\n" +
                              "Diego: I thought of everything I'd never regret\n" +
                              "0: A little time with you is all that I get-- 2\n" +
                              "Diego: I gotta be in your arms baby\n" +
                              "Diego: But far away I seek for your light\n" +
                              "Diego: I hold on because for you my heart keeps beating\n" +
                              "Diana: But no one gives us time anymore\n" +
                              "Diego: Will you be my light?\n";

            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TomeTest4_Graph_Traversal_Choice_0()
        {
            int[] choices = new int[] { 0 };
            var conv = ConversationLoader.CreateConversation(String.Format(root, "TomeTest_4.txt"));

            //action
            string result = TraverseGraph(conv, choices);
            string expected = "Diego: Hey what's happening\n" +
                              "0: This sets one to true-- 0\n" +
                              "Person: Hey\n" +
                              "Person: Blah\n" +
                              "Other: Heyo" +
                              "Other: What is going on";

            //assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TomeTest4_Graph_Traversal_Choice_1()
        {
            // assemble
            int[] choices = new int[] { 1 };
            var conv = ConversationLoader.CreateConversation(String.Format(root, "TomeTest_4.txt"));

            // action
            string result = TraverseGraph(conv, choices);
            string expected = "Diego: Hey what's happening:\n" +
                            "1: This sets two to true-- 0\n" +
                            "Person: Hello\n" +
                            "Person: Blah\n";

            // assert
            Assert.AreEqual(expected, result);
        }
    }
}
