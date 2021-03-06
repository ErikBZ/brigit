﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Brigit.IO;
using NUnit.Framework;
using Brigit.Structure.Exchange;
using Brigit;

namespace Brigit.Test
{
    [TestFixture]
    public class BrigitGraphTest
    {
        public static string TraverseGraph(Conversation conv, int[] choices)
        {
            int choiceTracker = 0;
            StringBuilder sb = new StringBuilder();
            // action
            conv.Start();
            bool errorOccured = false;
            while (!conv.Complete && !errorOccured)
            {
                Info inf = conv.GetInfo();

                // getting the next one
                switch (inf.type)
                {
                    case Info.Type.Dialog:
                        sb.Append(inf.ToString());
                        errorOccured = !(conv.Next());
                        break;
                    case Info.Type.Descision:
						var descision = inf.Data as Decision;
                        if(choices.Length > choiceTracker && descision.Interactive)
                        {
                            errorOccured = !(conv.Next(choices[choiceTracker]));
                            int ch = choices[choiceTracker];
                            sb.Append(String.Format("{0}: ", ch));
                            sb.Append(descision.Choices[ch].ToString());
                            choiceTracker++;
                        }
                        else
                        {
                            errorOccured = conv.Next();
                        }
                        break;
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        [Test]
        public void Traverse_TomeTest2_WithChoice0()
        {
            // arrange
            int[] choices = new int[] { 0 };
            var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_2.yml"));

            // act
            string result = TraverseGraph(conv, choices);
            string expected = "Yulia: What the fuck is this\n" +
                              "Yulia: What are you doing?\n" +
                              "0: Nothing\n" +
                              "Yulia: You're lying\n" +
                              "Diego: Yeah she is\n" +
                              "Diego: There's a lot of yelling going on right now\n";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Traverse_TomeTest3_WithChoice2_UsingBakedInBranches()
        {
            int[] choices = new int[] { 2 };
            var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_3.yml"));

            string result = TraverseGraph(conv, choices);
            string expected = "Diana: I didn't want to be the one to forget\n" +
                              "Diego: I thought of everything I'd never regret\n" +
                              "2: I don't believe in him - his lips on the ground\n" +
                              "Diana: But no one gives us time anymore\n" +
                              "Diego: Will you be my light?\n";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Traverse_TomeTest3_WithChoice0_UsingBranchName()
        {
            // These are inputs, for the traversal
            // for descisions these correspond to the chosen choice not the branch
            // assemble
            int[] choices = new int[] { 0 };
            var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_3.yml"));

            // action
            string result = TraverseGraph(conv, choices);
            string expected = "Diana: I didn't want to be the one to forget\n" +
                              "Diego: I thought of everything I'd never regret\n" +
                              "0: A little time with you is all that I get\n" +
                              "Diego: I gotta be in your arms baby\n" +
                              "Diego: But far away I seek for your light\n" +
                              "Diego: I hold on because for you my heart keeps beating\n" +
                              "Diana: But no one gives us time anymore\n" +
                              "Diego: Will you be my light?\n";

            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Traverse_TomeTest4_Choice0_CheckingForFlagRequirements()
        {
            int[] choices = new int[] { 0 };
            var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_4.yml"));

            //action
            string result = TraverseGraph(conv, choices);
            string expected = "Diego: Hey what's happening\n" +
                              "0: This sets one to true\n" +
                              "Person: Hello\n" +
                              "Person: Blah\n" +
                              "Other: Heyo\n" +
                              "Other: What's going on\n";

            //assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Traverse_TomeTest4_Choice1_CheckingForFlagRequirements()
        {
            // assemble
            int[] choices = new int[] { 1 };
            var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_4.yml"));

            // action
            string result = TraverseGraph(conv, choices);
            string expected = "Diego: Hey what's happening\n" +
                            "1: This sets two to true\n" +
                            "Person: Hey\n" +
                            "Person: Blah\n";

            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Traverse_TomeTest5_Choice0_CheckingForMixedFlagRequirements()
        {
            int[] choices = new int[] { 0 };
            var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_5.yml"));

            //action
            string result = TraverseGraph(conv, choices);
            string expected = "0: choice 1\n" +
                              "Person: hello\n" +
                              "Person: how\n" +
                              "Person: are you\n" +
                              "Other: Another one\n" +
                              "Other: done\n" +
                              "More: Blah\n" +
                              "Last: Hey\n";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Traverse_TomeTest5_Choice1_CheckingForBlockSkipping()
        {
            int[] choices = new int[] { 1 };
            var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_5.yml"));

            // action
            string result = TraverseGraph(conv, choices);
            string expected = "1: choice 2\n" +
                              "Other: Another one\n" +
                              "Other: something\n" +
                              "Other: done\n" +
                              "Last: Hey\n";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Traverse_TomeTest6_Choice0_CheckingBranchNameSelection()
        {
            //assemble
            int[] choices = new int[] { 0 };
            var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_6.yml"));

            //act
            string result = TraverseGraph(conv, choices);
            string expected = "Spongebob: I'm ready\n" +
                              "0: Path 1\n" +
                              "Pearl: Daddy!\n" +
                              "Sandy: Karate chop\n" +
                              "Mr.Krabs: I like money\n";

            //assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Traverse_TomeTest6_Choice1_CheckingBakedInBranchSelection()
        {
            //assemble
            int[] choices = new int[] { 1 };
            var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_6.yml"));

            //act
            string result = TraverseGraph(conv, choices);
            string expected = "Spongebob: I'm ready\n" +
                              "1: Path 2\n" +
                              "Squidward: Arghhh\n" +
                              "Patrick: SPONGEBOB\n" +
                              "Sandy: Karate chop\n";

            //assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Traverse_TomeTest6_Choice2_CheckingPassingSelection()
        {
            //avegers assemble
            int[] choices = new int[] { 2 };
            var conv = ConversationLoader.CreateConversation(Path.Combine(Config.TomePath, "TomeTest_6.yml"));

            //act
            string result = TraverseGraph(conv, choices);
            string expected = "Spongebob: I'm ready\n" +
                              "2: Path 3\n" +
                              "Sandy: Karate chop\n";

            //assert
            Assert.AreEqual(expected, result);
        }
    }
}
