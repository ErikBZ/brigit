using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Brigit.IO;
using Brigit.Parser;
using Brigit.Parser.Stream;
using YamlDotNet.RepresentationModel;
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
            var yaml = new YamlStream();
            var reader = new StringReader(File.ReadAllText(filepath));

            // preprocess
            yaml.Load(reader);
            var rootMapNode = (YamlMappingNode)yaml.Documents[0].RootNode;

            // parsing here
            var brigitParser = new BrigitYamlParser(rootMapNode);
            var bg = brigitParser.CreateGraphFromYaml();

            return new Conversation(bg);
        }
    }
}
