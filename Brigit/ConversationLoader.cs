using System;
using System.Collections.Generic;
using System.Text;
using Brigit.IO;
using Brigit.Parser;
using Brigit.Parser.Stream;
using Brigit.Structure;

namespace Brigit
{
    // Reads a tome or text file, parses it if need be, and returns
    // the conversaion
    // i gotta remember to make classes public. woops
    public static class ConversationLoader
    {
        public static Conversation CreateConversation(string filepath)
        {
            // Getting the text file
            string[] text = TomeReader.ReadTextFile(filepath);

            // preprocess
            text = CommentRemover.RemoveComments(text);
            var tome = new TomeStream(text);

            // parsing here
            var bg = BrigitParser.Parse(tome);

            var conv = new Conversation(bg);

            return conv;
        }
    }
}
