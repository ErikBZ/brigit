using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Structure;
using Brigit.Structure.Exchange;
using Brigit.Attributes;
using YamlDotNet.RepresentationModel;
using Brigit.Attributes.ExpressionParser;

namespace Brigit.Parser
{
    public class BrigitYamlParser
    {
        public YamlMappingNode RootNode { get; }

        public BrigitYamlParser()
        {
            RootNode = new YamlMappingNode();
        }

        public BrigitYamlParser(YamlMappingNode mappingNode)
        {
            RootNode = mappingNode;
        }

        public BrigitGraph CreateGraphFromYaml()
        {
            var graph = new BrigitGraph();

            // The graphs attributes 

            // Getting the graph from the yaml node
            var brigitNodes = (YamlSequenceNode)RootNode.Children[new YamlScalarNode("graph")];

            return CreateGraph(brigitNodes);
        }

        public BrigitGraph CreateGraph(YamlSequenceNode rootGraphNode)
        {
            var graph = new BrigitGraph();
            foreach(YamlNode node in rootGraphNode)
            {
                string brigitNodeType = ((YamlMappingNode)node).Children.Keys.First().ToString();

                switch(brigitNodeType)
                {
                    case "dialog":
                        var brigitNode = new Node(CreateDialog((YamlMappingNode) node));
                        graph.AddNode(brigitNode);
                        break;
                    case "decision":
                        var subGraph = CreateDecision((YamlMappingNode)node);
                        graph.AddGraph(subGraph);
                        break;
                    case "fork":
                        var forkGraph = CreateFork((YamlSequenceNode)node);
                        graph.AddGraph(forkGraph);
                        break;
                    default:
                        throw new Exception(String.Format("Node Type {0} not recognized", brigitNodeType));
                }
            }

            return graph;
        }

        public Dialog CreateDialog(YamlMappingNode yamlNode)
        {
            var dialog = new Dialog();

            // TODO 1 fix this. Ther's probably a much better way to set this
            dialog.Character = GetScalarYamlNodeValue("character", yamlNode);

            if(yamlNode.Children.ContainsKey(new YamlScalarNode("attr")))
            {
                YamlMappingNode attributeMapNode = (YamlMappingNode) yamlNode.Children[new YamlScalarNode("attr")];
                dialog.Attributes = DeserializeAttributes(attributeMapNode);
            }

            foreach (YamlMappingNode text in (YamlSequenceNode)yamlNode.Children[new YamlScalarNode("speech")])
            {
                dialog.Text.Add(DeserializeSpeechText(text));
            }
            
            return dialog;
        }

        public BrigitGraph CreateDecision(YamlMappingNode yamlNode)
        {
            BrigitGraph graph = new BrigitGraph();
            Decision decision = new Decision();
            Node baseNode = new Node(decision);
            graph.AddNode(baseNode);

            foreach (YamlMappingNode choiceYamlNode in (YamlSequenceNode)yamlNode.Children[new YamlScalarNode("decision")])
            {
                Choice choice = DeserializeChoice(choiceYamlNode);
                choice.NextNode = -1;
                decision.Choices.Add(choice);

                // Node has inline branch
                if (choiceYamlNode.Children.ContainsKey(new YamlScalarNode("graph")))
                {
                    var inlineBranch = CreateGraph((YamlSequenceNode)choiceYamlNode.Children[new YamlScalarNode("graph")]);
                    graph.AddBranch(baseNode, inlineBranch);
                    choice.NextNode = baseNode.Next.Count - 1;
                }
            }

            foreach (Choice choice in decision.Choices.Where(x => x.NextNode == -1))
            {
                choice.NextNode = baseNode.Next.Count;
            }

            return graph;
        }

        public BrigitGraph CreateFork(YamlSequenceNode yamlSequence)
        {
            var forkingGraph = new BrigitGraph();
            var nonInteractiveDecision = new Decision();
            nonInteractiveDecision.Interactive = false;
            var baseNode = new Node(nonInteractiveDecision);
            forkingGraph.AddNode(baseNode);

            foreach(YamlMappingNode node in yamlSequence)
            {
                var choice = new Choice();
                string expression = GetScalarYamlNodeValue("path", node);
                choice.Attributes.Expression = BrigitExpressionParser.Parse(expression);

                if(node.Children.ContainsKey(new YamlScalarNode("graph")))
                {
                    var subGraph = CreateGraph((YamlSequenceNode)node.Children[new YamlScalarNode("graph")]);
                    forkingGraph.AddBranch(baseNode, subGraph);
                    choice.NextNode = baseNode.Next.Count - 1;
                }
            }

            foreach( Choice choice in nonInteractiveDecision.Choices.Where(x => x.NextNode == -1))
            {
                choice.NextNode = baseNode.Next.Count;
            }

            return forkingGraph;
        }

        public Choice DeserializeChoice(YamlMappingNode yamlNode)
        {
            Choice choice = new Choice();
            choice.Text = GetScalarYamlNodeValue("choice", yamlNode);

            if(yamlNode.Children.ContainsKey(new YamlScalarNode("attr")))
            {
                YamlMappingNode attributeMapNode = (YamlMappingNode) yamlNode.Children[new YamlScalarNode("attr")];
                choice.Attributes = DeserializeAttributes(attributeMapNode);
            }
            
            return choice;
        }

        public SpeechText DeserializeSpeechText(YamlMappingNode yamlNode)
        {
            SpeechText text = new SpeechText();

            // see TODO 1
            text.Text = GetScalarYamlNodeValue("text", yamlNode);
            
            if (yamlNode.Children.ContainsKey(new YamlScalarNode("attr")))
            {
                YamlMappingNode attributeMapNode = (YamlMappingNode) yamlNode.Children[new YamlScalarNode("attr")];
                text.Attributes = DeserializeAttributes(attributeMapNode);
            }

            return text;
        }

        // This one will need some more looking at since we gotta parse the expression
        public AttributeManager DeserializeAttributes(YamlMappingNode yamlNode)
        {
            var am = new AttributeManager();
            foreach(KeyValuePair<YamlNode, YamlNode> kvp in yamlNode.Children)
            {
                if(kvp.Key.Equals(new YamlScalarNode("onlyIf")))
                {
                    // TODO add expression parsing here
                    string expression = kvp.Value.ToString();
                    am.Expression = BrigitExpressionParser.Parse(expression);
                }
                else if(kvp.Key.Equals(new YamlScalarNode("setTrue")))
                {
                    SetFlagMapping((YamlSequenceNode)kvp.Value, Flag.True, am.SetFlags);
                }
                else if(kvp.Key.Equals(new YamlScalarNode("setFalse")))
                {
                    SetFlagMapping((YamlSequenceNode)kvp.Value, Flag.False, am.SetFlags);
                }
                else if (kvp.Key.Equals(new YamlScalarNode("setDontCare")))
                {
                    SetFlagMapping((YamlSequenceNode)kvp.Value, Flag.DontCare, am.SetFlags);
                }
                else
                {
                    // I should probably not be doing this
                    am.ExtraAttributes[kvp.Key.ToString()] = kvp.Value.ToString();
                }
            }
            return am;
        }

        private void SetFlagMapping(YamlSequenceNode flagsNode, Flag flagType, Dictionary<string, Flag> attributeFlagMap)
        {
            foreach(YamlScalarNode flagNode in flagsNode)
            {
               attributeFlagMap[flagNode.Value] = flagType;
            }           
        }

        private String GetScalarYamlNodeValue(String key, YamlMappingNode map)
        {
            return ((YamlScalarNode)map.Children[new YamlScalarNode(key)]).Value;
        }
    }
}
